using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : MonoBehaviour 
{
    public enum PlayerStatus { Alive, Dead, Invincible }
    public enum MoveSkillStatus { Ready, OnCooldown }

    [SerializeField] PlayerIndicator playerIndicator;
    [SerializeField] public SkinnedMeshRenderer meshRenderer;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource spawnSound;
    [SerializeField] AudioSource dashSound;
    [SerializeField] ParticleSystem PlayerSpawnParticles;

    public int MaxHealth = 3;
    public float MoveSpeed = 2f;
    public float SpeedModifier = 1f;
    public float RollDuration = 0.125f;
    public float DashDuration = 0.25f;
    public float MoveSkillCooldown = 1f;
    public float MoveSkillRecoveryTime = .1f;
    public System.Action<int, int> OnDeath;

    public AbstractWeapon Weapon;
    public int PlayerNumber = 0;
    public int Health = 1;
    public bool canMove = true;
    public bool canRotate = true;
	public PlayerStatus status = PlayerStatus.Alive;
    public MoveSkillStatus moveStatus = MoveSkillStatus.Ready;
    float currentRollTime = 0f;
    float currentDashTime = 0f;
    public bool isRolling = false;
    public bool isDashing = false;
    float rollX;
    float rollY;
    float MoveSkillTimer = 0f;


    Vector3 GROUNDED_DOWNWARD_VELOCITY = new Vector3(0, -10f, 0);
   void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
        Gizmos.color = controller.isGrounded ? Color.green : Color.gray;
        Gizmos.DrawCube(transform.position + transform.up * 3f, Vector3.one * .2f);
    }

    void Update() 
    {
        controller.Move(GROUNDED_DOWNWARD_VELOCITY);

        //Roll logic
        //keep for now
        // if(isRolling && currentRollTime <= RollDuration){
        //     this.canMove = false;
        //     this.canRotate = false;

        //     var m = Vector3.zero;
                
        //     m.x = rollX * Time.deltaTime * MoveSpeed * RollModifier;
        //     m.z = rollY * Time.deltaTime * MoveSpeed * RollModifier;

        //     controller.Move(m);
        //     currentRollTime += Time.deltaTime;
        // }

        if(isDashing && currentDashTime <= DashDuration){
            var dashDir = transform.forward.normalized;
            var m = Vector3.zero;
            m.x = dashDir.x  * Time.deltaTime * MoveSpeed * SpeedModifier;
            m.z = dashDir.z * Time.deltaTime * MoveSpeed * SpeedModifier;
            controller.Move(m);
            currentDashTime += Time.deltaTime;
        }

        if(currentDashTime > DashDuration){
            DashEnd();
        }

        if(moveStatus == MoveSkillStatus.OnCooldown){
            MoveSkillTimer += Time.deltaTime;
            if (MoveSkillTimer >= MoveSkillCooldown){
                MoveSkillTimer = 0f;
                moveStatus = MoveSkillStatus.Ready;
            } 
        }

        
    }

    public void SetColor(Color color)
    {
        playerIndicator.meshRenderer.material.color = color;
        meshRenderer.material.color = color;
    }

    public void Move(float xAxis, float yAxis)
    {
        var m = Vector3.zero;

        m.x = xAxis * Time.deltaTime * MoveSpeed * SpeedModifier;
        m.z = yAxis * Time.deltaTime * MoveSpeed * SpeedModifier;
        controller.Move(m);
        animator.SetFloat("Forward", m.magnitude);
    }

    public void SetWeapon(AbstractWeapon newWeapon)
    {
        if (Weapon)
        {
            Destroy(Weapon.gameObject);
        }
        Weapon = Instantiate(newWeapon, transform);
        Weapon.player = this;
        canRotate = true;
        canMove = true;
    }

    public void Damage(int amountOfDamage, int attackerIndex)
    {
        if (status == PlayerStatus.Dead)
            return;

        Health = Mathf.Max(0, Health - amountOfDamage);
        animator.SetTrigger("Hit");
        takeDamageSound.Play();

        if (Health <= 0)
        {
            Kill(attackerIndex);
        }
    }

    public void Kill(int attackerIndex)
    {
        status = PlayerStatus.Dead;
        Health = 0;
        canMove = false;
        canRotate = false;

        OnDeath.Invoke(PlayerNumber, attackerIndex);
        animator.SetBool("PlayDeathAnimation", true);
        deathSound.Play();
    }

	public void Spawn(Transform t)
	{
        status = PlayerStatus.Alive;
		Health = MaxHealth;
        canMove = true;
        canRotate = true;
		transform.SetPositionAndRotation(t.position, t.rotation);

        var particles = Instantiate(PlayerSpawnParticles, new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), transform.rotation);
        var children = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (var p in children){
            var particleScript = p.GetComponent<SetParticleColor>();
            if(particleScript != null){
                particleScript.startColor = meshRenderer.material.color != null ? meshRenderer.material.color : Color.white;
                particleScript.SetColor();
            }
        }

        particles.Play();
        Destroy(particles, 2f);

        animator.SetBool("PlayDeathAnimation", false);
        spawnSound.Play();
	}

    public void RollInDirection(float xAxis, float yAxis){
        rollX  = xAxis;
        rollY = yAxis;
        isRolling = true;
        animator.SetBool("PerformRoll", true);
    }

    public void RollEnd(){
        isRolling = false;
        currentRollTime = 0f;
        this.canMove = true;
        this.canRotate = true;
        animator.SetBool("PerformRoll", false);
    }

    public void Dash()
    {
        if(!Weapon.CompareTag("MeleeWeapon") || moveStatus != MoveSkillStatus.Ready) return;
        dashSound.Play();
        SpeedModifier = 2.5f;
        isDashing = true;
        this.canMove = false;
        this.canRotate = false;
        animator.SetBool("PerformDash", true);

    }
    public void DashEnd()
    {
        isDashing = false;
        moveStatus = MoveSkillStatus.OnCooldown;
        StartCoroutine("MoveSkillRecovery");
        currentDashTime = 0f;
        animator.SetBool("PerformDash", false);
        
    }

    IEnumerator MoveSkillRecovery(){
        yield return new WaitForSeconds(MoveSkillRecoveryTime);
        SpeedModifier = Weapon.SpeedModifier;
        this.canMove = true;
        this.canRotate = true;
    }

    public void SetAsVictor(){
        animator.SetBool("PlayerWins", true);
    }
     
}
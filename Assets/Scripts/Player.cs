using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : MonoBehaviour 
{
    #region variables
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
    [SerializeField]	GameObject Reticle;
    [SerializeField]	SpriteRenderer Crosshair;
    [SerializeField]	PlayerHitbox hitbox;

    public int MaxHealth = 3;
    public float MoveSpeed = 2f;
    public float SpeedModifier = 1f;
    public float DashDuration = 0.25f;
    public float MoveSkillCooldown = 2f;
    public float MoveSkillRecoveryTime = .1f;
    public System.Action<int, int> OnDeath;

    public AbstractWeapon Weapon;
    public int PlayerNumber = 0;
    public int Health = 1;
    public bool canMove = true;
    public bool canRotate = true;
	public PlayerStatus status = PlayerStatus.Alive;
    
    //Moveskill 
    public MoveSkillStatus moveStatus = MoveSkillStatus.Ready;
    float currentDashTime = 0f;
    public bool isDashing = false;
    float MoveSkillTimer = 0f;
    Vector3 dashDir = Vector3.zero;

    //IK Targets
    Transform LeftHandIKTarget;
    Transform RightHandIKTarget;
    public float IkWeight =1f;
    Vector3 GROUNDED_DOWNWARD_VELOCITY = new Vector3(0, -10f, 0);
    #endregion

    void Start(){
        hitbox.player = this;
    }

    void Update() 
    {
        //maybe can set this once somewhere
        Crosshair.color = meshRenderer.material.color;
        //apply constant downward vel
        controller.Move(GROUNDED_DOWNWARD_VELOCITY);
        //calculate and execute dash
        if(isDashing && currentDashTime <= DashDuration){
            var m = Vector3.zero;
            m.x = dashDir.x  * Time.deltaTime * MoveSpeed * SpeedModifier;
            m.z = dashDir.z * Time.deltaTime * MoveSpeed * SpeedModifier;
            controller.Move(m);
            currentDashTime += Time.deltaTime;
        }
        //end dash
        if(currentDashTime > DashDuration) DashEnd();

        //dash cooldown
        if(moveStatus == MoveSkillStatus.OnCooldown){
            MoveSkillTimer += Time.deltaTime;
            if (MoveSkillTimer >= MoveSkillCooldown){
                MoveSkillTimer = 0f;
                moveStatus = MoveSkillStatus.Ready;
            } 
        }        
    }

    //Inverse Kinematics for guns
    private void OnAnimatorIK(int layerIndex) {
        if(LeftHandIKTarget != null && RightHandIKTarget != null){
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IkWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IkWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTarget.position);
        }
    }
    //Sets the color of the player and player indicator
    public void SetColor(Color color)
    {
        playerIndicator.meshRenderer.material.color = color;
        meshRenderer.material.color = color;
    }
    //move character controller
    public void Move(float xAxis, float yAxis)
    {
        if(status == PlayerStatus.Dead) return;

        var m = Vector3.zero;
        m.x = xAxis * Time.deltaTime * MoveSpeed * SpeedModifier;
        m.z = yAxis * Time.deltaTime * MoveSpeed * SpeedModifier;
        controller.Move(m);
        animator.SetFloat("Forward", m.magnitude);
    }
    //set the equipped weapon
    public void SetWeapon(AbstractWeapon newWeapon)
    {
        if (Weapon)
        {
            Destroy(Weapon.gameObject);
        }
        Weapon = Instantiate(newWeapon, transform);
        Weapon.player = this;

        //set IK targets
        LeftHandIKTarget = Weapon.LeftHandIKTarget;
        RightHandIKTarget = Weapon.RightHandIKTarget;

        canRotate = true;
        canMove = true;
    }
    //damages the player
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
    //kill player
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
    //spawn new player
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

    #region moveskills
    public void Dash(Vector3 direction)
    {
        //if(!Weapon.CompareTag("MeleeWeapon") || moveStatus != MoveSkillStatus.Ready) return;
        if (direction == Vector3.zero) return;
        dashDir = direction;
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
    #endregion
    //helper to kick off the victory animation
    public void SetAsVictor(){
        animator.SetBool("PlayerWins", true);
    }
     
}
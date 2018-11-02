using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : AbstractCharacter 
{
    #region variables
    public enum MoveSkillStatus { Ready, OnCooldown }

    [SerializeField] PlayerIndicator playerIndicator;
    [SerializeField] public SkinnedMeshRenderer meshRenderer;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageSound;
    [SerializeField] AudioSource fallDeathSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource spawnSound;
    [SerializeField] AudioSource dashSound;
    [SerializeField] ParticleSystem PlayerSpawnParticles;
    [SerializeField] SpriteRenderer Crosshair;
    [SerializeField] PlayerHitbox hitbox;

    public float MoveSpeed = 2f;
    public float SpeedModifier = 1f;
    public float DashDuration = 0.25f;
    public float MoveSkillCooldown = 2f;
    public float MoveSkillRecoveryTime = .1f;

    public AbstractWeapon Weapon;

    public int Health = 1;
    public bool canMove = true;
    public bool canRotate = true;
	
    
    //Moveskill 
    public MoveSkillStatus moveStatus = MoveSkillStatus.Ready;
    float currentDashTime = 0f;
    public bool isDashing = false;
    public float MoveSkillTimer = 0f;
    Vector3 dashDir = Vector3.zero;

    public float IkWeight = 1f;
    
    #endregion

    Player(){
        ENTITY_TYPE = "PLAYERCHARACTER";
    }

    void Update() 
    {
        //maybe can set this once somewhere
        Crosshair.color = meshRenderer.material.color;
        //apply constant downward vel
        controller.Move(GROUNDED_DOWNWARD_VELOCITY);

        //calculate and execute dash
        if (isDashing && currentDashTime <= DashDuration)
        {
            var m = Vector3.zero;
            m.x = dashDir.x  * Time.deltaTime * MoveSpeed * SpeedModifier;
            m.z = dashDir.z * Time.deltaTime * MoveSpeed * SpeedModifier;
            controller.Move(m);
            currentDashTime += Time.deltaTime;
        }

        //end dash
        if (currentDashTime > DashDuration) 
        {
            isDashing = false;
            moveStatus = MoveSkillStatus.OnCooldown;
            StartCoroutine(MoveSkillRecovery());
            currentDashTime = 0f;
            animator.SetBool("PerformDash", false);
        }

        //dash cooldown
        if (moveStatus == MoveSkillStatus.OnCooldown)
        {
            MoveSkillTimer += Time.deltaTime;
            if (MoveSkillTimer >= MoveSkillCooldown)
            {
                MoveSkillTimer = 0f;
                moveStatus = MoveSkillStatus.Ready;
            } 
        }        
    }

    //Inverse Kinematics for guns
    void OnAnimatorIK(int layerIndex) 
    {
        if (Weapon.LeftHandIKTarget != null && Weapon.RightHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IkWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IkWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, Weapon.LeftHandIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, Weapon.RightHandIKTarget.position);
        }
    }

    public void SetColor(Color color)
    {
        playerIndicator.color = color;
        meshRenderer.material.color = color;
    }

    public void Move(Vector3 v)
    {
        if (!canMove)
            return;

        if (status == CharacterStatus.Dead) 
            return;

        var speed = Time.deltaTime * MoveSpeed * SpeedModifier;
        var velocity = v * speed;

        controller.Move(velocity);
        animator.SetFloat("Forward", velocity.magnitude);
    }

    public void SetWeapon(AbstractWeapon newWeapon)
    {
        if (Weapon)
        {
            Destroy(Weapon.gameObject);
        }
        canRotate = true;
        canMove = true;
        Weapon = Instantiate(newWeapon, transform);
        Weapon.player = this;
    }
    public void Damage(int damageAmount)
    {
        Health = Mathf.Max(0, Health - damageAmount);
        animator.SetTrigger("Hit");
        takeDamageSound.Play();
    }

    public void Kill()
    {
        status = CharacterStatus.Dead;
        Health = 0;
        canMove = false;
        canRotate = false;
        animator.SetBool("PlayDeathAnimation", true);
        deathSound.Play();
    }

    public void KillByFalling()
    {
        status = CharacterStatus.Dead;
        fallDeathSound.Play();
    }

	public void Spawn(Transform t)
	{
        status = CharacterStatus.Alive;
		Health = MaxHealth;
        canMove = true;
        canRotate = true;
		transform.SetPositionAndRotation(t.position, t.rotation);

        var particles = Instantiate(PlayerSpawnParticles, new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), transform.rotation);
        var children = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (var p in children)
        {
            var particleScript = p.GetComponent<SetParticleColor>();
            if(particleScript != null)
            {
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
    public void Dash()
    {
        if (moveStatus != MoveSkillStatus.Ready)
            return;

        dashDir = transform.forward;
        dashSound.Play();
        SpeedModifier = 2.5f;
        isDashing = true;
        this.canMove = false;
        this.canRotate = false;
        animator.SetBool("PerformDash", true);
    }

    IEnumerator MoveSkillRecovery()
    {
        yield return new WaitForSeconds(MoveSkillRecoveryTime);
        //SpeedModifier = Weapon.SpeedModifier;
        this.canMove = true;
        this.canRotate = true;
    }
    #endregion

    //helper to kick off the victory animation
    public void SetAsVictor()
    {
        animator.SetBool("PlayerWins", true);
    }

}
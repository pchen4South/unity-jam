﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MoveSkillStatus { Ready, OnCooldown }

public class Player : AbstractCharacter 
{
    public PlayerIndicator playerIndicator;
    public SkinnedMeshRenderer meshRenderer;
    public CharacterController controller;
    public Animator animator;
    public AudioSource takeDamageSound;
    public AudioSource fallDeathSound;
    public AudioSource deathSound;
    public AudioSource spawnSound;
    public AudioSource dashSound;
    public ParticleSystem PlayerSpawnParticles;
    public SpriteRenderer Crosshair;
    public PlayerHitbox hitbox;

    public float MoveSpeed = 2f;
    public float SpeedModifier = 1f;
    public float DashDuration = 0.25f;
    public float MoveSkillCooldown = 2f;
    public float MoveSkillRecoveryTime = .1f;

    public AbstractWeapon Weapon;

    public bool canMove = true;
    public bool canRotate = true;
    public float respawnTimeRemaining = 0f;
    public int weaponIndex;
    
    public MoveSkillStatus moveStatus = MoveSkillStatus.Ready;
    float currentDashTime = 0f;
    public bool isDashing = false;
    public float MoveSkillTimer = 0f;
    Vector3 dashDir = Vector3.zero;

    public float IkWeight = 1f;

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
            moveStatus = MoveSkillStatus.OnCooldown;
        }

        //end dash
        if (currentDashTime > DashDuration) 
        {
            isDashing = false;
            StartCoroutine(MoveSkillRecovery());
            currentDashTime = 0f;
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
        animator.SetFloat("Forward", velocity.magnitude > 0 ? 1 : 0);
    }

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);

        if (status == CharacterStatus.Dead)
        {
            canMove = false;
            canRotate = false;
            animator.SetBool("PlayDeathAnimation", true);
            deathSound.Play();
        }
        else 
        {
            animator.SetTrigger("Hit");
            takeDamageSound.Play();
        }
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
    public void Dash(Vector3 direction)
    {
        if (moveStatus != MoveSkillStatus.Ready)
            return;

        dashDir = direction != Vector3.zero ? direction : transform.forward;
        dashSound.Play();
        SpeedModifier = 2.5f * SpeedModifier;
        isDashing = true;
        this.canMove = false;
        this.canRotate = false;
    }

    IEnumerator MoveSkillRecovery()
    {
        yield return new WaitForSeconds(MoveSkillRecoveryTime);
        SpeedModifier = Weapon.SpeedModifier;
        this.canMove = true;
        this.canRotate = true;
    }
    #endregion

    //helper to kick off the victory animation
    public void SetAsVictor()
    {
        animator.SetBool("PlayerWins", true);
    }

    //Buffs
    public IEnumerator ReturnToNormalSettings(float buffTime){
        yield return new WaitForSeconds(buffTime);
        damageMultiplier = 1f;
        SpeedModifier = Weapon.SpeedModifier;
    }
    public void SetDamageMultiplier(float multiplier, float buffTime){
        damageMultiplier = multiplier;
        StartCoroutine(ReturnToNormalSettings(buffTime));
    }
    public void SetSpeedMultiplier(float multiplier, float buffTime){
        SpeedModifier = multiplier * Weapon.SpeedModifier;
        StartCoroutine(ReturnToNormalSettings(buffTime));
    }
    public void HealForAmount(int recoverAmount){
        Health = Mathf.Min(MaxHealth, Health + recoverAmount);
    }

}
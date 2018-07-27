using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mallet : AbstractWeapon {
    enum WeaponState { Resting, Swinging, Charging, Spinning }

    [Header("Config")]
    public Animator animator;
    public float chargeDelay = .1f;
    public float maxChargeTime = 1f;

    [Header("Prefabs")]
    public ParticleSystem Dust;
    public ParticleSystem Trail;
    public ParticleSystem Spin;

    [Header("State")]
    private bool swingingHammer = false;
    private float hammerSwingingTime = 0;
    private float chargeTime = 0f;
    private ParticleSystem SpinAnimation;
    WeaponState state = WeaponState.Resting;

    void Start () {
		
	}

    void Update() {
        Debug.Log("charge: " + chargeTime);
        /*
        switch (state) {
            case WeaponState.Swinging:
                hammerSwingingTime += Time.deltaTime;
                if (hammerSwingingTime >= 0.5f)
                {
                    hammerSwingingTime = 0f;
                    animator.SetBool("swingHammer", false);
                    state = WeaponState.Resting;
                }
                break;
            case WeaponState.Charging:
                break;
        }
        */
	}

    public override void PullTrigger(Player player)
    {
        chargeTime += Time.deltaTime;
        if (chargeTime >= chargeDelay) { 
            animator.SetBool("charging", true);
            state = WeaponState.Charging;
        }
    }


    public override void ReleaseTrigger(Player player)
    {
        if (state == WeaponState.Resting && chargeTime < chargeDelay)
        {
            animator.SetBool("swingHammer", true);
            state = WeaponState.Swinging;
        }
        else if (chargeTime < maxChargeTime + chargeDelay)
        {
            animator.SetBool("charging", false);
            state = WeaponState.Resting;
        }
        else if (chargeTime >= maxChargeTime + chargeDelay) {
            animator.SetBool("charging", false);
            animator.SetBool("hammerSpin", true);
            state = WeaponState.Spinning;
        }

        chargeTime = 0f;
    }

    public void HammerSpinStarted()
    {

        SpinAnimation = Instantiate(Spin, player.transform);
    }

    public void HammerSpinEnded() {
        animator.SetBool("hammerSpin", false);
        state = WeaponState.Resting;
    }

    public void MalletSwingEnded()
    {
        animator.SetBool("swingHammer", false);
        state = WeaponState.Resting;
    }

    public void MalletImpactGround() {
            Instantiate(Dust, this.transform);
    }

    public void MalletSwingStarted() {

        Instantiate(Trail, this.transform);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mallet : AbstractWeapon {
    enum WeaponState { Resting, Swinging, Charging, Spinning }

    [Header("Config")]
    public Animator animator;
    public float chargeDelay = .1f;
    public float maxChargeTime = 1f;
    public Color BarColor1 = Color.white;
    public Color BarColor2 = Color.yellow;
    public Color BarColor3 = Color.red;
    public Rigidbody body;
    public CapsuleCollider hitbox;

    [Header("Prefabs")]
    public ParticleSystem Dust;
    public ParticleSystem Trail;
    public ParticleSystem Spin;
    public AudioSource malletSwing;
    public AudioSource malletSpin;
    public WeaponBar ChargeBar;

    [Header("State")]
    private bool swingingHammer = false;
    private float hammerSwingingTime = 0;
    private float chargeTime = 0f;
    private ParticleSystem SpinAnimation;
    WeaponState state = WeaponState.Resting;
    private WeaponBar BarInstance;
    private List<int> PlayersHitOnSpin = new List<int>();

    void Start () {
        var player = this.player;

        hitbox.enabled = false;

        if (ChargeBar != null)
        {
            Debug.Log(ChargeBar);
            var bar = Instantiate(ChargeBar, player.transform.position + player.transform.up * 1.02f, player.transform.rotation, player.transform);

            bar.player = player;
            bar.maxBarColor = BarColor3;
            BarInstance = bar;
        }
    }

    void Update()
    {
        var percentCharged = Math.Max((chargeTime  - chargeDelay),0)/ maxChargeTime;

        switch (state)
        {
            case WeaponState.Resting:
                body.isKinematic = true;
                break;
            case WeaponState.Swinging:
            case WeaponState.Spinning:
                body.isKinematic = false;
                BarInstance.slider.value = 0;
                break;
            case WeaponState.Charging:
                body.isKinematic = true;
                // Weapon bar stuff
                var img = BarInstance.img;

                BarInstance.slider.value = percentCharged;

                if (percentCharged < 0.5)
                {
                    img.color = Color.Lerp(BarColor1, BarColor2, (float)percentCharged / 0.5f);
                }
                else if (percentCharged > 0.5 && percentCharged < 1)
                {
                    img.color = Color.Lerp(BarColor2, BarColor3, (float)((percentCharged - 0.5f) / 0.5f));
                }
                else if (percentCharged == 1)
                {
                    img.color = BarColor3;
                }
                break;



        }
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

    void OnCollisionEnter(Collision other)
    {
        if (hitbox.enabled == false)
            return;

        if (other.gameObject.tag == "Player")
        {
            var otherPlayer = other.gameObject.GetComponent<Player>();
            var otherPNum = otherPlayer.PlayerNumber;

            if ( !PlayersHitOnSpin.Contains(otherPNum)
                && otherPNum != player.PlayerNumber )
            {
                PlayersHitOnSpin.Add(otherPlayer.PlayerNumber);
                otherPlayer.Damage(1, player.PlayerNumber);
            }
        }
    }


    // Animation events below
    public void HammerSpinStarted()
    {
        PlayersHitOnSpin = new List<int>();
        hitbox.enabled = true;
        SpinAnimation = Instantiate(Spin, player.transform);
        malletSpin.Play();
    }

    public void HammerSpinEnded() {
        hitbox.enabled = false;
        animator.SetBool("hammerSpin", false);
        state = WeaponState.Resting;
    }

    public void MalletSwingEnded()
    {
        hitbox.enabled = false;
        animator.SetBool("swingHammer", false);
        state = WeaponState.Resting;
    }

    public void MalletImpactGround() {
            Instantiate(Dust, this.transform);
    }

    public void MalletSwingStarted() {
        PlayersHitOnSpin = new List<int>();
        hitbox.enabled = true;
        malletSwing.Play();
        Instantiate(Trail, this.transform);
    }

}

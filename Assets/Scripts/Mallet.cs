using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start () {
        var player = this.player;

        if (ChargeBar != null)
        {
            var bar = Instantiate(ChargeBar, player.transform.position + player.transform.up * 1.02f, player.transform.rotation, player.transform);

            bar.player = player;
            bar.maxBarColor = BarColor3;
            BarInstance = bar;
        }
    }

    void Update()
    {
        var percentCharged = chargeTime / maxChargeTime;

        switch (state)
        {
            case WeaponState.Resting:
            case WeaponState.Swinging:
            case WeaponState.Spinning:
                BarInstance.slider.value = 0;
                break;
            case WeaponState.Charging:
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
        if (other.gameObject.tag == "Player" 
            && other.gameObject.GetComponent<Player>().PlayerNumber != player.PlayerNumber)
        {
            other.gameObject.GetComponent<Player>().Health -= 1;
        }
    }

    public void HammerSpinStarted()
    {
        SpinAnimation = Instantiate(Spin, player.transform);
        malletSpin.Play();
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
        malletSwing.Play();
        Instantiate(Trail, this.transform);
    }

}

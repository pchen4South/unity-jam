using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : AbstractWeapon 
{
    enum WeaponState { NotFiring, Firing, OverHeating };

    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

    [Header("Prefabs")]
    public MachineGunBullet Ammo;
    public WeaponBar ChargeBar;

    [Header("Config")]
    public float TravelSpeed = 25f;
    public float OverHeatTime = 3f;
    public float OverHeatResetTime = 2f;
    public float FireRatePenalty = 1.5f;
    public Color BarColor1 = Color.white;
    public Color BarColor2 = Color.yellow;
    public Color BarColor3 = Color.red;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.1f;
    WeaponState state = WeaponState.NotFiring;

    private float firingTime = 0f;
    private bool maxChargeReached = false;
    private WeaponBar BarInstance;

    void Start()
    {
        var player = this.player;

        if(ChargeBar != null)
        {
            var bar = Instantiate(ChargeBar, player.transform.position + player.transform.up * 1.02f , player.transform.rotation, player.transform);

            bar.player = player;
            bar.maxBarColor = BarColor3;
            BarInstance = bar;
        }
    }

    public void FireBullet(){
        
        if (Time.time < nextFire)
            return;
        
        var weapon = player.Weapon;        
        var bullet = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, weapon.transform.rotation);

        bullet.body.AddForce( TravelSpeed * bullet.transform.forward, ForceMode.Impulse);
        bullet.PlayerNumber = player.PlayerNumber;
        fireSound.Play();

        if(firingTime >= OverHeatTime / 2)
        {
            nextFire = Time.time + fireRate * FireRatePenalty ;
        } 
        else 
        {
            nextFire = Time.time + fireRate;
        }
    }
    public override void PullTrigger(Player player)
    {
        // increase meter or hold at max
        if (state == WeaponState.NotFiring)
        {
            // if (heat != HeatState.MaxHeat)
            if (state != WeaponState.OverHeating)
            {
                state = WeaponState.Firing;
                firingTime += Time.deltaTime;
            }
        }
    }


    public override void ReleaseTrigger(Player player)
    {
        if (state == WeaponState.OverHeating)
        {
            StartCoroutine(OverHeatReset());
        } 
        else 
        {
            state = WeaponState.NotFiring;
        }
    }

    IEnumerator OverHeatReset()
    {
        yield return new WaitForSeconds(OverHeatResetTime);
        firingTime = 0;
        BarInstance.slider.value = 0;
        state = WeaponState.NotFiring;
    }

    void Update()
    {
        var percentHeat = (firingTime <= OverHeatTime ? firingTime : OverHeatTime) / OverHeatTime;

        switch(state)
        {
            case WeaponState.NotFiring:
                if (firingTime != 0)
                {
                    //reduce heat by y amount per unit time
                    if (firingTime > 0)
                    {
                        firingTime -= Time.deltaTime * 1.5f;
                    } 
                    else 
                    {
                        firingTime = 0;
                    }
                    BarInstance.slider.value = percentHeat;
                }
            break;
            case WeaponState.Firing:
                firingTime += Time.deltaTime;
                // Weapon bar stuff
				var img = BarInstance.img;				

                BarInstance.slider.value = percentHeat;

				if (percentHeat < 0.5)
				{
					img.color = Color.Lerp(BarColor1, BarColor2, (float)percentHeat / 0.5f);
                    FireBullet();
				}
				else if (percentHeat > 0.5 && percentHeat < 1)
				{
					img.color = Color.Lerp(BarColor2, BarColor3, (float)((percentHeat - 0.5f)/ 0.5f));
                    FireBullet();
				} 
				else if (percentHeat == 1)
				{
                    firingTime = OverHeatTime;
					img.color = BarColor3;
                    state = WeaponState.OverHeating;
				}
            break;
       }
    }
}
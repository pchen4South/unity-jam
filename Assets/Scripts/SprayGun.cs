using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayGun : AbstractWeapon {


    enum WeaponState { NotFiring, Firing, OverHeating };
    // enum HeatState { NoHeat, Heated, MaxHeat };

    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

    [Header("Prefabs")]
    public Grenade Ammo;
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
    // HeatState heat = HeatState.NoHeat;

    private float firingTime = 0f;
    private bool maxChargeReached = false;
    
    void Start()
    {
        var player = this.player;

    }

    public override void PullTrigger(Player player)
    {
		if (Time.time < nextFire)
            return;


		nextFire = Time.time + fireRate;	
		
		var weapon = player.Weapon;
        var nade = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, weapon.transform.rotation);
		var nade2 = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, Quaternion.Euler(new Vector3(0, 0, 150)));
		var nade3 = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, Quaternion.Euler(new Vector3(0, 0, 315)));



        nade.body.AddForce(weapon.transform.forward * TravelSpeed, ForceMode.Impulse);
		nade2.body.AddForce(weapon.transform.forward * TravelSpeed, ForceMode.Impulse);
		nade3.body.AddForce(weapon.transform.forward * TravelSpeed, ForceMode.Impulse);
        nade.PlayerNumber = player.PlayerNumber;
		nade2.PlayerNumber = player.PlayerNumber;
		nade3.PlayerNumber = player.PlayerNumber;
        fireSound.Play();
        nextFire = Time.time + fireRate;
    }


    public override void ReleaseTrigger(Player player)
    {

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : AbstractWeapon
{
    // Sound effect when the grenade launcher is first switched to
    // public AudioSource SFX_Weapon_Switch_GrenadeLauncher;

    // Sound effect for firing the launcher
    public AudioSource SFX_Weapon_Fire_GrenadeLauncher;

    private float nextFire;
    public float fireRate = 0.5f;
    // Currently Equipped Ammo (Grenade)
    public GameObject Ammo;
    
    public float GrenadeTravelSpeed = 8f;

    private void Start()
    {
        //SFX_Weapon_Switch_GrenadeLauncher.Play();
        SFX_Weapon_Fire_GrenadeLauncher = GetComponent<AudioSource>();
    }

    public override void PullTrigger(Player player)
    {
        if(Time.time > nextFire){
            nextFire = Time.time + fireRate;

            var wep = player.Weapon;
            var nade = (GameObject)Instantiate(Ammo, wep.transform.position, wep.transform.rotation);

            nade.GetComponent<Rigidbody>().velocity = nade.transform.forward * GrenadeTravelSpeed;
            nade.GetComponent<Grenade>().SetPlayerNumber(player);

            SFX_Weapon_Fire_GrenadeLauncher.enabled = true;
            SFX_Weapon_Fire_GrenadeLauncher.Play();
        }
    }
}

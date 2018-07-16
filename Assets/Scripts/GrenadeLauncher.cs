using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : AbstractWeapon
{
    // Sound effect when the grenade launcher is first switched to
    // public AudioSource SFX_Weapon_Switch_GrenadeLauncher;

    // Sound effect for firing the launcher
    public AudioSource SFX_Weapon_Fire_GrenadeLauncher;

    // Currently Equipped Ammo (Grenade)
    public GameObject Ammo;
    //private GameObject PlayerFiringShot;


    private void Start()
    {
        //SFX_Weapon_Switch_GrenadeLauncher.Play();
        SFX_Weapon_Fire_GrenadeLauncher = GetComponent<AudioSource>();
    }

    public override void Fire(Player player)
    {
        /*
        Vector3 playerPos = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Quaternion playerRotation = player.transform.rotation;
        float spawnDistance = 1.01f;
        */
        var wep = player.Weapon;
        //Vector3 grenadeSpawn = playerPos + playerDirection * spawnDistance;
        var nade = (GameObject)Instantiate(Ammo, wep.transform.position, wep.transform.rotation);
        nade.GetComponent<Rigidbody>().velocity = nade.transform.forward * 8;
        
        SFX_Weapon_Fire_GrenadeLauncher.enabled = true;
        SFX_Weapon_Fire_GrenadeLauncher.Play();
    }
}

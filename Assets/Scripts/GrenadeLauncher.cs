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
    private GameObject PlayerFiringShot;


    private void Start()
    {
        //SFX_Weapon_Switch_GrenadeLauncher.Play();
    }

    public override void Fire(Player player)
    {
        Debug.Log("Firing Grenade");
    }
}

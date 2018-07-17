using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadoukenLauncher : AbstractWeapon {

    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

    [Header("Prefabs")]
    public Hadouken Ammo;

    [Header("Config")]
    public float TravelSpeed = 25f;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.5f;

    public override void PullTrigger(Player player)
    {
        if (Time.time < nextFire)
            return;
        
        var wep = player.Weapon;
        var nade = Instantiate(Ammo, wep.transform.position + wep.transform.forward * 1.025f, wep.transform.rotation);
        nade.body.AddForce(nade.transform.forward * TravelSpeed, ForceMode.Impulse);

        /*
        nade.PlayerNumber = player.PlayerNumber;
        fireSound.Play();
         */
        nextFire = Time.time + fireRate;
    
    }
}

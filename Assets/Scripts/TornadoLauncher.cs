using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoLauncher : AbstractWeapon {

 [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

    [Header("Prefabs")]
    public Tornado Ammo;

    [Header("Config")]
    public float TornadoTravelSpeed = 8f;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    public override void PullTrigger(Player player)
    {
        if (Time.time < nextFire)
            return;
        
        var weapon = player.Weapon;
        var tornado = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * 1f, weapon.transform.rotation);

        tornado.body.AddForce(weapon.transform.forward * TornadoTravelSpeed, ForceMode.Impulse);
        tornado.PlayerNumber = player.PlayerNumber;
        fireSound.Play();
        nextFire = Time.time + fireRate;
    }
}
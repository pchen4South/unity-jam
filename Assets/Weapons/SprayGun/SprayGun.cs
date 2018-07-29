using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayGun : AbstractWeapon 
{
    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

    [Header("Prefabs")]
    public SprayNade Ammo;

    [Header("Config")]
    public float TravelSpeed = 25f;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.1f;

    public override void PullTrigger(Player player)
    {
		if (Time.time < nextFire)
            return;

		nextFire = Time.time + fireRate;	
		
		var weapon = player.Weapon;
		var vector = Quaternion.AngleAxis(-30, Vector3.up) * weapon.transform.forward;
		var vector2 = Quaternion.AngleAxis(30, Vector3.up) * weapon.transform.forward;

        var nade = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, weapon.transform.rotation);
		var nade2 = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, Quaternion.Euler(vector));
		var nade3 = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, Quaternion.Euler(vector2));

        nade.body.AddForce(weapon.transform.forward * TravelSpeed, ForceMode.Impulse);
		nade2.body.AddForce(vector * TravelSpeed, ForceMode.Impulse);
		nade3.body.AddForce(vector2 * TravelSpeed, ForceMode.Impulse);
        nade.PlayerNumber = player.PlayerNumber;
		nade2.PlayerNumber = player.PlayerNumber;
		nade3.PlayerNumber = player.PlayerNumber;
        fireSound.Play();
        nextFire = Time.time + fireRate;
    }
}
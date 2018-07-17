using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadoukenLauncher : AbstractWeapon {

    [Header("Cached references")]
    [SerializeField]
    AudioSource[] fireSound;

    [Header("Prefabs")]
    public Hadouken[] Ammo;

    [Header("Config")]
    public float TravelSpeed = 25f;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.5f;

    private float chargeTime = 0f;


    public override void PullTrigger(Player player)
    {
        if (Time.time < nextFire)
            return;
        

        chargeTime += Time.deltaTime;

    
    }

    public override void ReleaseTrigger(Player player){

        var ChargeLevel = 0;
        var wep = player.Weapon;
        
        if (chargeTime >= 1f && chargeTime <2f)
            ChargeLevel = 1;
        else if (chargeTime >= 2f){
            ChargeLevel = 2;
        }
        fireSound[ChargeLevel].Play();

        if(ChargeLevel == 2)
            StartCoroutine(DelayFire(0.6f, ChargeLevel, wep, player));
        else
        {
         ShootFireball(ChargeLevel, wep, player);
        }
        
        nextFire = Time.time + fireRate;
        chargeTime = 0f;
    }

    IEnumerator DelayFire(float delayTime, int ChargeLevel, AbstractWeapon wep, Player player){
            yield return new WaitForSeconds(delayTime);
            ShootFireball(ChargeLevel, wep, player);
    }
    void ShootFireball(int ChargeLevel, AbstractWeapon wep, Player player){
            float forceMultiplier = 0;

            switch (ChargeLevel){
                case 0:
                    forceMultiplier = 1;
                    break;
                case 1:
                    forceMultiplier = 1.5f;
                    break;
                case 2:
                    forceMultiplier = 2;
                    break;
                default:
                    forceMultiplier = 1;
                    break;
            }


            var fireball = Instantiate(Ammo[ChargeLevel], wep.transform.position + wep.transform.forward * 1.02f , wep.transform.rotation);
            fireball.body.AddForce(fireball.transform.forward * TravelSpeed * forceMultiplier, ForceMode.Impulse);
            fireball.PlayerNumber = player.PlayerNumber;
    }

}

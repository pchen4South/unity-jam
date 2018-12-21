using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : AbstractWeapon 
{
    [Header("Config")]
    public float kickBackGrowthRate = 1f;
    public float kickBackDecayRate = -2f;

    [Header("State")]
    float timeTillNextShot = 0f;
    //GameObject FlashInstance = null;


    void Awake()
    {
        //AmmoCount = MagazineSize;
        AmmoCount = -1;
        //FlashInstance = Instantiate(muzzleFlash, transform);
        WeaponName = "Machine Gun";
    }

    void LateUpdate()
    {
        timeTillNextShot -= Time.deltaTime;   
        //Commented out kickback code for now
        /* 
             
        kickbackScale += Time.deltaTime * (isFiring ? kickBackGrowthRate : kickBackDecayRate);
        kickbackScale = Mathf.Clamp01(kickbackScale);

        if (isFiring)
        {
            var yKickAngle = Random.Range(-kickbackScale, kickbackScale);
            var xKickAngle = -Random.Range(0, kickbackScale);

            transform.Rotate(xKickAngle, yKickAngle, 0, Space.Self);
        }
        else
        {
            transform.rotation = player.transform.rotation;
        }
        */
    }

    public override void PullTrigger(Player player)
    {
        if (timeTillNextShot > 0 || AmmoCount == 0) return;

        AmmoCount -= 1;        
        var muzzle = transform.position + transform.forward * muzzleOffset;

        StartCoroutine(PostShotCleanup());
        timeTillNextShot = fireRate;
        
        fireSound.Play();
        if(!aimAssistOn)
            CheckForValidHitscan(muzzle, transform.forward, layerMask);
        else {
            var tArea = GetComponentInChildren<WeaponTargettingArea>();
            CheckForValidTargetInRange(tArea, transform.forward, layerMask);
        }
    }
    IEnumerator PostShotCleanup()
    {
        yield return new WaitForSeconds(shotTime);
        bulletTracer.enabled = false;
        // muzzleFlashLight.enabled = false;
    }
}
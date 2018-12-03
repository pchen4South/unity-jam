using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : AbstractWeapon {

    [Header("Cached references")]
    [SerializeField]    GameObject muzzleFlash = null;
    [SerializeField]    Light muzzleFlashLight = null;

    [Header("State")]
    float timeTillNextShot = 0f;
	private GameObject FlashInstance = null;
    void Awake(){
        MagazineSize = 12;
        AmmoCount = MagazineSize;   
        FlashInstance = Instantiate(muzzleFlash, transform);     
        WeaponName = "Blaster";
        LeftHandIKTarget = IKTarget_L;
        RightHandIKTarget = IKTarget_R;
    }

    void LateUpdate()
    {
        timeTillNextShot -= Time.deltaTime;   
    }

      public override void PullTrigger(Player player)
    {
        if (timeTillNextShot > 0 || isReloading || AmmoCount == 0) return;

        var muzzle = transform.position + transform.forward * muzzleOffset;
        StartCoroutine(PostShotCleanup());

        AmmoCount -= 1;        
        timeTillNextShot = fireRate;
        
		FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        FlashInstance.GetComponentInChildren<ParticleSystem>().Play();
        muzzleFlashLight.enabled = true;
        fireSound.Play();
    
        CheckForValidHitscan(muzzle, transform.forward, layerMask);
        
        if (AmmoCount == 0)
            Reload();
    }


    IEnumerator PostShotCleanup()
    {
        yield return new WaitForSeconds(shotTime);
        bulletTracer.enabled = false;
        muzzleFlashLight.enabled = false;
    }

}

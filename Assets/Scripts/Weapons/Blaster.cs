using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : AbstractWeapon {
//  public Transform Muzzle;
// 	public Player player;
// 	public string WeaponName = "gun";
// 	public float SpeedModifier = 1f;
// 	public int AmmoCount = 0;
// 	public int MagazineSize = 0;
// 	public Transform LeftHandIKTarget;
// 	public Transform RightHandIKTarget;
// 	public virtual void PullTrigger(Player player){}
// 	public virtual void ReleaseTrigger(Player player){}

    [Header("Cached references")]
    [SerializeField]    GameObject muzzleFlash;
    [SerializeField]    Light muzzleFlashLight;

    [Header("State")]
    float timeTillNextShot = 0f;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
	private GameObject FlashInstance;
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

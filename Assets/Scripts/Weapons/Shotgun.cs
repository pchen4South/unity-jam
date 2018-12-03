using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : AbstractWeapon {

 	[Header("Cached references")]
    
    [SerializeField]    public GameObject muzzleFlash = null;

    [SerializeField]    Light muzzleFlashLight = null;
    [SerializeField]	ParticleSystem BlastRadius = null;

    [Header("State")]
    float timeTillNextShot = 0f;
	GameObject FlashInstance = null;

	void Start(){
        WeaponName = "Shotgun";
        AmmoCount = MagazineSize;       
		FlashInstance = Instantiate(muzzleFlash, transform);
        LeftHandIKTarget = IKTarget_L;
        RightHandIKTarget = IKTarget_R;
    }


    void LateUpdate()
    {
        timeTillNextShot -= Time.deltaTime;   
    }

	
    public override void PullTrigger(Player player)
    {
        if (timeTillNextShot > 0 || isReloading || AmmoCount == 0)
            return;

        AmmoCount -= 1;        
        var muzzle = transform.position + transform.forward * muzzleOffset;

        StartCoroutine(PostShotCleanup());
        timeTillNextShot = fireRate;
        
        if(FlashInstance != null){
            FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
            FlashInstance.GetComponentInChildren<ParticleSystem>().Play();
        }

        muzzleFlashLight.enabled = true;
        fireSound.Play();
		
        if (AmmoCount == 0)
            Reload();

        BlastRadius.Emit(8);

    }

    IEnumerator PostShotCleanup()
    {
        yield return new WaitForSeconds(shotTime);
        muzzleFlashLight.enabled = false;
		// var br = BlastRadius.GetComponent<MeshRenderer>();
		// br.enabled = false;
    }


}

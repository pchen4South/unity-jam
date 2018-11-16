﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : AbstractWeapon {

 	[Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

	[SerializeField]
    AudioSource reloadSound;
    [SerializeField]
    ParticleSystem HitParticlePrefab;
    [SerializeField]
    ParticleSystem HitPlayerParticlePrefab;
    [SerializeField]
    GameObject muzzleFlash;

    [SerializeField]
    Light muzzleFlashLight;
    [SerializeField]
	ParticleSystem BlastRadius;


    [Header("State")]
    float timeTillNextShot = 0f;
    bool isReloading = false;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
	private GameObject FlashInstance;

	void Start(){
        WeaponName = "Shotgun";
        AmmoCount = MagazineSize;       
		FlashInstance = Instantiate(muzzleFlash, transform);
        LeftHandIKTarget = IKTarget_L;
        RightHandIKTarget = IKTarget_R;
    }

    void Reload(){
        isReloading = true;
        reloadSound.Play();
        StartCoroutine(ReloadTimer());
    }

    IEnumerator ReloadTimer(){
        yield return new WaitForSeconds(ReloadTime);
        AmmoCount = MagazineSize;
        isReloading = false;
    }

	void Update(){	}

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
        
		FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        FlashInstance.GetComponentInChildren<ParticleSystem>().Play();

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
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deagle : AbstractWeapon 
{
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
    GameObject BulletHole;
    [SerializeField]
    Light muzzleFlashLight;
    [SerializeField]
    LineRenderer bulletTracer;

    [Header("Config")]
    public float fireRate = .1f;
    public float shotTime = .01f;
    public float muzzleOffset = .5f;
    public LayerMask layerMask = new LayerMask();
    float AmmoCount = 0f;
	public float MagazineSize = 7f;
	public float ReloadTime = 1f;

    [Header("State")]
    float timeTillNextShot = 0f;
    float kickbackScale = 0f;
    bool isFiring = false;
    bool isReloading = false;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
	private GameObject FlashInstance;

    void Awake(){
        AmmoCount = MagazineSize;   
        FlashInstance = Instantiate(muzzleFlash, transform);     
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
        isFiring = true;
        timeTillNextShot = fireRate;
        
		FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        FlashInstance.GetComponentInChildren<ParticleSystem>().Play();

        muzzleFlashLight.enabled = true;
        fireSound.Play();
        ray.origin = muzzle;
        ray.direction = transform.forward;

        var didHit = Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask);

        if (AmmoCount == 0)
            Reload();

        if (!didHit)
            return;

        bulletTracer.SetPosition(0, muzzle);
        bulletTracer.SetPosition(1, rayHit.point);
        bulletTracer.enabled = true;
        var isPlayer = rayHit.collider.CompareTag("Player");

        // should move some of this code to player
        if (isPlayer)
        {
            var target = rayHit.collider.GetComponent<Player>();
            if(target.status != Player.PlayerStatus.Invincible){
                var hitParticles = Instantiate(HitPlayerParticlePrefab, rayHit.point, transform.rotation);
                target.Damage(1, player.PlayerNumber);
                Destroy(hitParticles.gameObject, 2f);
            } else {
                target.InvicibleSound.Play();  
            }
        }
        else
        {
            GameObject bulletHole = Instantiate(BulletHole, rayHit.point, Quaternion.FromToRotation(Vector3.up, rayHit.normal));
            var particleSystems = bulletHole.GetComponentsInChildren<ParticleSystem>();

            foreach(var p in particleSystems){
                ParticleSystem.MainModule psmain = p.main;
                psmain.startColor = player.color;
            }
            Destroy(bulletHole, 3f);
        }
  
    }

    IEnumerator PostShotCleanup()
    {
        yield return new WaitForSeconds(shotTime);
        bulletTracer.enabled = false;
        muzzleFlashLight.enabled = false;
    }
    public override void ReleaseTrigger(Player player)
    {
        isFiring = false;
    }
}
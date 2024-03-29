﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractWeapon : MonoBehaviour 
{
	public Player player;
	
	[Header("Config")]
	public bool aimAssistOn = false;
	public float weaponRange = 20f;
	public float weaponArc = 20f;
	[SerializeField] public Transform Muzzle;
	[SerializeField] public AudioSource fireSound= null;
	[SerializeField] public AudioSource reloadSound = null;
	[SerializeField] ParticleSystem HitPlayerParticlePrefab = null;
	public string WeaponName = "gun";
	public float SpeedModifier = 1f;
	public int AmmoCount = 0;
	public int MagazineSize = 0;
	[SerializeField] public int DamageAmount = 1;
	public Transform LeftHandIKTarget;
	public Transform RightHandIKTarget;
    public float fireRate = .1f;
    public float shotTime = .01f;
    public float muzzleOffset = .5f;
	public float ReloadTime = 1f;
	public bool isReloading = false;
	[SerializeField] public LineRenderer bulletTracer;
	[SerializeField] public GameObject BulletHole;
	[SerializeField] public LayerMask layerMask = new LayerMask();
	[SerializeField] public Transform IKTarget_L;
    [SerializeField] public Transform IKTarget_R;
	[SerializeField] public Light MuzzleFlashLight;
	

	float reloadtimer = 0f;
	
	public virtual void PullTrigger(Player player){
		if(!player.IsAlive()) return;
	}

	public virtual void ReleaseTrigger(Player player){
		if(!player.IsAlive()) return;
	}

	public virtual void CheckForValidHitscan(Vector3 muzzle, Vector3 weaponDir, LayerMask layerMask){
		Ray ray = new Ray();
		RaycastHit rayHit = new RaycastHit();

		if( Muzzle != null)
			muzzle = Muzzle.transform.position;
		
		ray.origin = muzzle;
		ray.direction = weaponDir;

        var didHit = Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask);
        if (!didHit)
            return;

		DrawTracer(muzzle, rayHit.point);
        var isPlayer = rayHit.collider.CompareTag("PlayerHitbox");
		var isNPC = rayHit.collider.CompareTag("NPCHitbox");

		//Can prob refactor if player / npc have common baseclass
        if (isPlayer || isNPC)
        {
			// Debug.Log("playerhit");
			AbstractCharacter target;
			
			if (isNPC)
				target = rayHit.collider.GetComponent<BossMonster>();
			else
				target = rayHit.collider.GetComponentInParent<PlayerHitbox>().player;
			
			//reject targets that are not alive
			if(!target.IsAlive()) return;

			player.RegisterNewValidHit(player, target, DamageAmount);
			CreateBloodSpray(rayHit.point, transform.rotation);
        }
		// if not player or npc then it hit terrain
		else {
			// if bullethole graphic create one at the impact point
			if(BulletHole != null){
				GameObject bulletHole = Instantiate(BulletHole, rayHit.point, Quaternion.FromToRotation(Vector3.up, rayHit.normal));
            	var particleSystems = bulletHole.GetComponentsInChildren<ParticleSystem>();
            	Destroy(bulletHole, 3f);
			}
		}
	}


	public virtual void CheckForValidTargetInRange(WeaponTargettingArea WeaponTargettingArea, Vector3 forward, int layerMask){
		Player target = WeaponTargettingArea.playertarget;
		// valid player in target area
		if(target != null && WeaponTargettingArea.HasValidTarget()){
			if(!target.IsAlive()) return;
			DrawTracer(Muzzle.transform.position, target.transform.position);
			player.RegisterNewValidHit(player, target, DamageAmount);
			CreateBloodSpray(target.transform.position, transform.rotation);
		//else miss
		} else {
			Ray ray = new Ray();

			ray.origin = Muzzle.transform.position;
			ray.direction = transform.forward;

			DrawTracer(Muzzle.transform.position, transform.position + transform.forward * weaponRange);
		}
	}


	public virtual void DrawTracer(Vector3 muzzle, Vector3 destination){
		if(bulletTracer != null){
        	bulletTracer.SetPosition(0, muzzle);
        	bulletTracer.SetPosition(1, destination);
        	bulletTracer.enabled = true;
		}
	}

	
	public virtual void CreateBloodSpray(Vector3 location, Quaternion rotation){
		Instantiate(HitPlayerParticlePrefab, location, rotation);
	}


	void Update(){
		reloadtimer += Time.deltaTime;
	}

    public void Reload(){
		if(AmmoCount == MagazineSize || isReloading) return;

        isReloading = true;
		reloadtimer = 0;
        reloadSound.Play();
        StartCoroutine(ReloadTimer());
    }

    public IEnumerator ReloadTimer(){
        yield return new WaitForSeconds(ReloadTime);
        AmmoCount = MagazineSize;
        isReloading = false;
    }
	
	public int ReloadProgressPercent(){
		int reloadProgress = 0;
		if(isReloading)
			reloadProgress = Mathf.RoundToInt(100 * reloadtimer / ReloadTime);
		else	
			reloadProgress = -1;
		return reloadProgress;
	}

}
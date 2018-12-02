using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractWeapon : MonoBehaviour 
{
	public Transform Muzzle;
	public Player player;
	
	[Header("Config")]
	[SerializeField] public AudioSource fireSound;

	[SerializeField] public AudioSource reloadSound;
	[SerializeField] ParticleSystem HitPlayerParticlePrefab;
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

		ray.origin = muzzle;
		ray.direction = weaponDir;

        var didHit = Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask);
		if(bulletTracer != null){
        	bulletTracer.SetPosition(0, muzzle);
        	bulletTracer.SetPosition(1, rayHit.point);
        	bulletTracer.enabled = true;
		}
        if (!didHit)
            return;


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
	
	public virtual void CreateBloodSpray(Vector3 location, Quaternion rotation){
		Instantiate(HitPlayerParticlePrefab, location, rotation);
	}


	void Update(){
		reloadtimer += Time.deltaTime;
	}

    public void Reload(){
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
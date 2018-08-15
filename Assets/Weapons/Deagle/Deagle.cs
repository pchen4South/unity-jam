using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deagle : AbstractWeapon 
{
    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;
    [SerializeField]
    AudioSource hitSound;
    [SerializeField]
    AudioSource hitPlayerSound;
    [SerializeField]
    ParticleSystem HitParticlePrefab;
    [SerializeField]
    ParticleSystem HitPlayerParticlePrefab;
    [SerializeField]
    ParticleSystem muzzleFlash;

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
    public float kickBackGrowthRate = 1f;
    public float kickBackDecayRate = -2f;
    public LayerMask layerMask = new LayerMask();

    [Header("State")]
    float timeTillNextShot = 0f;
    float kickbackScale = 0f;
    bool isFiring = false;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();

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
        if (timeTillNextShot > 0)
            return;
        
        var muzzle = transform.position + transform.forward * muzzleOffset;

        StartCoroutine(PostShotCleanup());
        isFiring = true;
        timeTillNextShot = fireRate;
        muzzleFlash.Stop();
        muzzleFlash.Play();
        muzzleFlashLight.enabled = true;
        fireSound.Play();
        ray.origin = muzzle;
        ray.direction = transform.forward;

        var didHit = Physics.Raycast(ray, out rayHit, layerMask);

        if (!didHit)
            return;

        bulletTracer.SetPosition(0, muzzle);
        bulletTracer.SetPosition(1, rayHit.point);
        bulletTracer.enabled = true;
        
        var isPlayer = rayHit.collider.CompareTag("Player");

		// shoudl move some of this code to player
        if (isPlayer)
        {
            var target = rayHit.collider.GetComponent<Player>();

			if(target.status != Player.PlayerStatus.Invincible){
				var hitParticles = Instantiate(HitPlayerParticlePrefab, rayHit.point, transform.rotation);
				target.Damage(1, player.PlayerNumber);
				//hitPlayerSound.Play();
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
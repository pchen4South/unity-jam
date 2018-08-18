using System.Collections;
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
    ParticleSystem muzzleFlash;
	// [SerializeField]
    // GameObject BulletHole;
    [SerializeField]
    Light muzzleFlashLight;
    [SerializeField]
	GameObject BlastRadius;
	// [SerializeField]
    // LineRenderer bulletTracer;


    [Header("Config")]
    public float fireRate = .75f;
    public float shotTime = .01f;
    public float muzzleOffset = .5f;
    public LayerMask layerMask = new LayerMask();
    float AmmoCount = 0f;
	public float MagazineSize = 5f;
	public float ReloadTime = 2f;

    [Header("State")]
    float timeTillNextShot = 0f;
    float kickbackScale = 0f;
    bool isFiring = false;
    bool isReloading = false;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
	private ParticleSystem FlashInstance;

	void Start(){
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

	void Update()
	{
		FlashInstance.transform.localRotation = Quaternion.FromToRotation(Vector3.up, transform.forward);		
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
        
		// FlashInstance.Stop();
        // FlashInstance.Play();

        muzzleFlashLight.enabled = true;
        fireSound.Play();
		
		var br = BlastRadius.GetComponent<MeshRenderer>();
		br.enabled = true;
		var brCollider = br.GetComponent<MeshCollider>();


        ray.origin = muzzle;
        ray.direction = transform.forward;

        var didHit = Physics.Raycast(ray, out rayHit, layerMask);

        // if (!didHit)
        //     return;

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
        if (AmmoCount == 0)
            Reload();
    }

    IEnumerator PostShotCleanup()
    {
        yield return new WaitForSeconds(shotTime);
        muzzleFlashLight.enabled = false;
		var br = BlastRadius.GetComponent<MeshRenderer>();
		br.enabled = false;
		//FlashInstance.Stop();
    }
    public override void ReleaseTrigger(Player player)
    {
        isFiring = false;
    }
}

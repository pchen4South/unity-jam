using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : AbstractWeapon {

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
	GameObject Projectile;

    [SerializeField]
    GameObject LaserSight;

    [Header("Config")]
    public float fireRate = 2f;
    public float FireDelayTime = 1f;
    public float shotTime = .01f;
    public float muzzleOffset = .5f;
    public LayerMask layerMask = new LayerMask();
    float AmmoCount = 0f;
	public float MagazineSize = 5f;
	public float ReloadTime = 2f;

    [Header("State")]
    float timeTillNextShot = 0f;
    bool isFiring = false;
    bool isReloading = false;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
	private GameObject FlashInstance;
    GameObject ProjectileInstance;
    bool inPrefire = false;

	void Start(){
        AmmoCount = MagazineSize;       
		FlashInstance = Instantiate(muzzleFlash, transform);
        LaserSight.gameObject.SetActive(false);
    }

    void AlignProjectileParts() {
        var pieces = ProjectileInstance.transform.Find("Pieces");
        var piecesMain = pieces.GetComponent<ParticleSystem>().main;
        var playerRotY = player.transform.eulerAngles.y;
        piecesMain.startRotationZ = Mathf.Deg2Rad * (playerRotY);
    }


	void Update(){}

    void LateUpdate()
    {
        timeTillNextShot -= Time.deltaTime;   
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
	
    public override void PullTrigger(Player player)
    {
        if (timeTillNextShot > 0 || isReloading || AmmoCount == 0 || inPrefire)
            return;
        StartCoroutine(PrefireRoutine());
        //FireBullet();
    }


    void FireBullet() {

        AmmoCount -= 1;
        var muzzle = transform.position + transform.forward * muzzleOffset;

        StartCoroutine(PostShotCleanup());
        isFiring = true;
        timeTillNextShot = fireRate;

        FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        FlashInstance.GetComponentInChildren<ParticleSystem>().Play();

        muzzleFlashLight.enabled = true;
        fireSound.Play();

        ProjectileInstance = Instantiate(Projectile, transform.position, transform.rotation);
        Destroy(ProjectileInstance, 1);

        ray.origin = muzzle;
        ray.direction = transform.forward;

        var didHit = Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask);

        if (AmmoCount == 0)
            Reload();

        if (!didHit)
            return;

        var isPlayer = rayHit.collider.CompareTag("Player");

        // should move some of this code to player
        if (isPlayer)
        {
            var target = rayHit.collider.GetComponent<Player>();
            if (target.status != Player.PlayerStatus.Invincible)
            {
                var hitParticles = Instantiate(HitPlayerParticlePrefab, rayHit.point, transform.rotation);
                target.Damage(1, player.PlayerNumber);
                Destroy(hitParticles.gameObject, 2f);
            }
            else
            {
                target.InvicibleSound.Play();
            }
        }
    }


    IEnumerator PrefireRoutine() {
        inPrefire = true;
        player.canMove = false;
        LaserSight.gameObject.SetActive(true);
        yield return new WaitForSeconds(FireDelayTime);
        FireBullet();
    }

    IEnumerator PostShotCleanup()
    {
        yield return new WaitForSeconds(shotTime);
        muzzleFlashLight.enabled = false;
        LaserSight.gameObject.SetActive(false);
        player.canMove = true;
        inPrefire = false;
        //FlashInstance.Stop();
    }
    public override void ReleaseTrigger(Player player)
    {
        isFiring = false;
    }
}

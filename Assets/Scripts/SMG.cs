using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : AbstractWeapon {
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
    LineRenderer bulletTracer;

    [SerializeField]
    ParticleSystem BulletCasings;

    [Header("Config")]
    public float fireRate = .15f;
    public float shotTime = .01f;
    public float muzzleOffset = .5f;
    public LayerMask layerMask = new LayerMask();
    float AmmoCount = 0f;
    public float MagazineSize = 25f;
    public float ReloadTime = 1f;

    public float maxBulletSpread;
    public float timeToMaxSpread;



    [Header("State")]
    float timeTillNextShot = 0f;
    bool isReloading = false;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
    private GameObject FlashInstance;
    ParticleSystem CasingsInstance;
    ParticleSystem.EmissionModule em;
    float fireTime;

    void Awake()
    {
        AmmoCount = MagazineSize;
        FlashInstance = Instantiate(muzzleFlash, transform);
        CasingsInstance = Instantiate(BulletCasings, transform);
        CasingsInstance.Play();

        em = CasingsInstance.emission;
        em.enabled = false;
    }

    void Reload()
    {
        isReloading = true;
        em.enabled = false;
        reloadSound.Play();
        fireTime = 0f;
        StartCoroutine(ReloadTimer());
    }

    IEnumerator ReloadTimer()
    {
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

        fireTime += Time.deltaTime;

        AmmoCount -= 1;
        var muzzle = transform.Find("silencer").position + (-transform.right) * muzzleOffset;

        StartCoroutine(PostShotCleanup());
        timeTillNextShot = fireRate;

        FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        FlashInstance.GetComponentInChildren<ParticleSystem>().Play();

        muzzleFlashLight.enabled = true;
        fireSound.Play();

        Vector3 fireDirection = player.transform.forward;        
        float currentSpread = Mathf.Lerp(0.0f, maxBulletSpread, fireTime / timeToMaxSpread);
        float randomOffsetX= Random.Range(-currentSpread, currentSpread);
        float randomOffsetY= Random.Range(-currentSpread, currentSpread);

        //apply random inaccuracy to raycast
        ray.origin = muzzle;
        ray.direction = new Vector3(fireDirection.x  + randomOffsetX, fireDirection.y, fireDirection.z + randomOffsetY);

        var didHit = Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask);

        if (AmmoCount == 0)
            Reload();

        if (!didHit)
            return;

        if (!isReloading)
            em.enabled = true;

        bulletTracer.SetPosition(0, muzzle);
        bulletTracer.SetPosition(1, rayHit.point);
        bulletTracer.enabled = true;
        var isPlayer = rayHit.collider.CompareTag("Player");

        // should move some of this code to player
        if (isPlayer)
        {
            if(player.PlayerNumber != rayHit.collider.gameObject.GetComponent<Player>().PlayerNumber){
                var target = rayHit.collider.GetComponent<Player>();
                var hitParticles = Instantiate(HitPlayerParticlePrefab, rayHit.point, transform.rotation);
                target.Damage(1, player.PlayerNumber);
                Destroy(hitParticles.gameObject, 2f);
            }
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
        em.enabled = false;
        fireTime = 0f;
    }

}

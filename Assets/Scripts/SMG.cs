using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : AbstractWeapon {
    [Header("Cached references")]
    [SerializeField]    AudioSource fireSound;
    [SerializeField]    AudioSource reloadSound;
    [SerializeField]    ParticleSystem HitParticlePrefab;
    [SerializeField]    ParticleSystem HitPlayerParticlePrefab;
    [SerializeField]    GameObject muzzleFlash;
    [SerializeField]    Light muzzleFlashLight;
    [SerializeField]    ParticleSystem BulletCasings;

    [Header("Config")]
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
        MagazineSize = 25;
        AmmoCount = MagazineSize;
        FlashInstance = Instantiate(muzzleFlash, transform);
        CasingsInstance = Instantiate(BulletCasings, transform);
        CasingsInstance.Play();

        em = CasingsInstance.emission;
        em.enabled = false;
        WeaponName = "SMG";

        LeftHandIKTarget = IKTarget_L;
        RightHandIKTarget = IKTarget_R;
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

        //fireTime is for bullet spread
        fireTime += Time.deltaTime;
        AmmoCount -= 1;
        var muzzle = transform.position + transform.forward * muzzleOffset;
        StartCoroutine(PostShotCleanup());

        timeTillNextShot = fireRate;

        FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        FlashInstance.GetComponentInChildren<ParticleSystem>().Play();
        muzzleFlashLight.enabled = true;
        fireSound.Play();

        Vector3 fireDirection = player.transform.forward;        
        float currentSpread = Mathf.Lerp(0.0f, maxBulletSpread, fireTime / timeToMaxSpread);
        float randomOffsetX= Random.Range(-currentSpread, currentSpread);

        //apply random inaccuracy to raycast
        ray.origin = muzzle;
        ray.direction = new Vector3(fireDirection.x  + randomOffsetX, fireDirection.y, fireDirection.z);

        //CheckForValidHitscan(muzzle, ray.direction, layerMask);
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

    public override void ReleaseTrigger(Player player)
    {
        FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        em.enabled = false;
        fireTime = 0f;
    }

}

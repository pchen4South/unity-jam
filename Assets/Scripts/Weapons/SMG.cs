using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : AbstractWeapon {
    [Header("Cached references")]
    [SerializeField]    GameObject muzzleFlash = null;
    [SerializeField]    Light muzzleFlashLight = null;
    [SerializeField]    ParticleSystem BulletCasings = null;

    [Header("Config")]
    public float maxBulletSpread;
    public float timeToMaxSpread;

    [Header("State")]
    float timeTillNextShot = 0f;
    private GameObject FlashInstance = null;
    ParticleSystem CasingsInstance = null;
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
        
        //bullet casings, disabled for now cuz they look fucked up
        //em.enabled = true;

        Vector3 fireDirection = player.transform.forward;        
        float currentSpread = Mathf.Lerp(0.0f, maxBulletSpread, fireTime / timeToMaxSpread);
        float randomOffsetX= Random.Range(-currentSpread, currentSpread);

        //apply random inaccuracy to raycast
        Vector3 shotDir = new Vector3(fireDirection.x  + randomOffsetX, fireDirection.y, fireDirection.z);
        
        if(!aimAssistOn)
            CheckForValidHitscan(muzzle, shotDir, layerMask);
        else {
            var tArea = GetComponentInChildren<WeaponTargettingArea>();
            CheckForValidTargetInRange(tArea, transform.forward, layerMask);
        }

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

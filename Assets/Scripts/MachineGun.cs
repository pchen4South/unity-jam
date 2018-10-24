using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : AbstractWeapon 
{
    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;
    [SerializeField]
    AudioSource hitSound;
    [SerializeField]
    AudioSource hitPlayerSound;
    [SerializeField]
    GameObject HitParticlePrefab;
    [SerializeField]
    ParticleSystem HitPlayerParticlePrefab;
    // [SerializeField]
    // GameObject muzzleFlash;
    // [SerializeField]
    // Light muzzleFlashLight;
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
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
    GameObject FlashInstance;


    void Awake()
    {
        //AmmoCount = MagazineSize;
        AmmoCount = -1;
        //FlashInstance = Instantiate(muzzleFlash, transform);
        WeaponName = "Machine Gun";
    }

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

        StartCoroutine(PostShotCleanup());
        timeTillNextShot = fireRate;

        // FlashInstance.GetComponentInChildren<ParticleSystem>().Stop();
        // FlashInstance.GetComponentInChildren<ParticleSystem>().Play();

        // muzzleFlashLight.enabled = true;
        fireSound.Play();
        ray.origin = Muzzle.position;
        ray.direction = Muzzle.transform.forward;

        var didHit = Physics.Raycast(ray, out rayHit, layerMask);
        /*
        if (!didHit)
            return;
             */

        bulletTracer.SetPosition(0, Muzzle.position);
        bulletTracer.SetPosition(1, rayHit.point);
        bulletTracer.enabled = true;
        
        var isPlayer = rayHit.collider.CompareTag("PlayerHitbox");

        if (isPlayer)
        {
            var target = rayHit.collider.GetComponentInParent<PlayerHitbox>().player;
            var hitParticles = Instantiate(HitPlayerParticlePrefab, rayHit.point, transform.rotation);

            target.Damage(1, player.PlayerNumber);
            hitPlayerSound.Play();
            Destroy(hitParticles.gameObject, 2f);
        }
        else
        {
            var hitParticles = Instantiate(HitParticlePrefab) as GameObject;

            hitSound.Play();
            hitParticles.transform.position = rayHit.point;
            hitParticles.transform.LookAt(transform, -Vector3.up);
            //hitParticles.transform.Find("Burst").transform.LookAt(transform, Vector3.up);

            Destroy(hitParticles, 2f);
        }
    }

    IEnumerator PostShotCleanup()
    {
        yield return new WaitForSeconds(shotTime);
        bulletTracer.enabled = false;
        // muzzleFlashLight.enabled = false;
    }
}
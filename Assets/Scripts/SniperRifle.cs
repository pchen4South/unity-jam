using UnityEngine;

public class SniperRifle : AbstractWeapon
{
    enum SniperRifleState { Ready, Prefire, Reload };

    [SerializeField] AudioSource fireSound;
    [SerializeField] Animator animator;
    [SerializeField] LineRenderer laserSight;
    [SerializeField] ParticleSystem muzzleFlash;

    [SerializeField] ParticleSystem HitParticlePrefab;
    [SerializeField] ParticleSystem HitPlayerParticlePrefab;
    [SerializeField] GameObject ProjectilePrefab;

    public float PreFireDuration = 1f;
    public float ReloadDuration = 1f;
    public LayerMask layerMask = new LayerMask();

    SniperRifleState state = SniperRifleState.Ready;
    float remainingReloadTime = 0f;
    float remainingPrefireTime = 0f;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();

    private void Awake() {
        WeaponName = "Sniper"; 
        AmmoCount = -1;   
    }
    void Update() 
    { 
        if (state == SniperRifleState.Reload && remainingReloadTime <= 0f)
        {
            state = SniperRifleState.Ready;
        }
        if (state == SniperRifleState.Prefire && remainingPrefireTime <= 0f)
        {
            FireBullet();
            state = SniperRifleState.Reload;
            animator.SetBool("PreFire", false);
        }
        else
        {
            remainingReloadTime -= Time.deltaTime;
            remainingPrefireTime -= Time.deltaTime;
        }
        player.canMove = state != SniperRifleState.Prefire;
    }

    public override void PullTrigger(Player player)
    {
        if (state != SniperRifleState.Ready)
            return;
        
        state = SniperRifleState.Prefire;
        remainingPrefireTime = PreFireDuration;
        animator.SetBool("PreFire", true);
    }

    void FireBullet()
    {
        var projectile = Instantiate(ProjectilePrefab, transform.position, transform.rotation);

        Destroy(projectile, 1f);
        state = SniperRifleState.Reload;
        remainingReloadTime = ReloadDuration;
        muzzleFlash.Stop();
        muzzleFlash.Play();
        fireSound.Play();

        ray.origin = Muzzle.transform.position;
        ray.direction = Muzzle.transform.forward;

        if (!Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask))
            return;

        // TODO: should move some of this code to player
        if (rayHit.collider.CompareTag("Player"))
        {
            var target = rayHit.collider.GetComponent<Player>();
            var hitParticles = Instantiate(HitPlayerParticlePrefab, rayHit.point, transform.rotation);

            target.Damage(1, player.PlayerNumber);
            Destroy(hitParticles.gameObject, 2f);
        }
    }
}
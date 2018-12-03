using UnityEngine;

public class SniperRifle : AbstractWeapon
{
    enum SniperRifleState { Ready, Prefire, Reload };
    [SerializeField] Animator animator  = null;
    [SerializeField] ParticleSystem muzzleFlash = null;
    [SerializeField] GameObject ProjectilePrefab = null;

    public float PreFireDuration = 1f;
    public float ReloadDuration = 1f;

    SniperRifleState state = SniperRifleState.Ready;
    float remainingReloadTime = 0f;
    float remainingPrefireTime = 0f;

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

        var muzzle = transform.position + transform.forward * muzzleOffset;
        CheckForValidHitscan(muzzle, transform.forward, layerMask);
    }
}
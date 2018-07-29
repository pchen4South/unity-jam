using UnityEngine;

public class RocketLauncher : AbstractWeapon
{
    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

    [Header("Prefabs")]
    public Rocket Ammo;

    [Header("Config")]
    public float RocketMoveSpeed = 8f;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.5f;

    void OnDrawGizmos()
    {
        if (player == null)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    public override void PullTrigger(Player player)
    {
        if (Time.time < nextFire)
            return;
        
        var rocket = Instantiate(Ammo, transform.position + transform.forward, transform.rotation);

        rocket.Body.AddForce(transform.forward * RocketMoveSpeed, ForceMode.Impulse);
        rocket.PlayerNumber = player.PlayerNumber;
        fireSound.Play();
        nextFire = Time.time + fireRate;
    }
}
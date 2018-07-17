using UnityEngine;

public class GrenadeLauncher : AbstractWeapon
{
    [Header("Cached references")]
    [SerializeField]
    AudioSource fireSound;

    [Header("Prefabs")]
    public Grenade Ammo;

    [Header("Config")]
    public float GrenadeTravelSpeed = 8f;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(player.Weapon.transform.position, player.Weapon.transform.forward);
    }

    public override void PullTrigger(Player player)
    {
        if (Time.time < nextFire)
            return;
        
        var weapon = player.Weapon;
        var nade = Instantiate(Ammo, weapon.transform.position + weapon.transform.forward * .5f, weapon.transform.rotation);

        nade.body.AddForce(weapon.transform.forward * GrenadeTravelSpeed, ForceMode.Impulse);
        nade.PlayerNumber = player.PlayerNumber;
        fireSound.Play();
        nextFire = Time.time + fireRate;
    }
}
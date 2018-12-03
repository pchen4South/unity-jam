using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : AbstractWeapon
{
    enum RiotShieldState {Ready, Swinging, Damaging, UsingMoveSkill}
    RiotShieldState shieldState = RiotShieldState.Ready;

    [Header("Cached References")]
    [SerializeField]    Animator anim = null;
    [SerializeField]    public float swingTime = 1f;
    [SerializeField]    ParticleSystem hitBoxActiveParticles = null;
    [SerializeField]    AudioSource swingSound = null;
    
    float DamageCooldown;

    void Start()
    {
        WeaponName = "Shield";
        //SpeedModifier = 0.5f;        
        player.SpeedModifier = SpeedModifier;
        DamageCooldown = swingTime;
        hitBoxActiveParticles.Stop();
        AmmoCount = -1;
    }

    #region Weapon Trigger Overrides
    public override void PullTrigger(Player player){
        if(shieldState != RiotShieldState.Ready || !player.IsAlive())
            return;
        swingSound.Play();
        anim.SetBool("swinging", true);
    }

    public override void ReleaseTrigger(Player player){
        anim.SetBool("swinging", false);
    }
    #endregion

    #region Animation Event Functions
    public void AttackEnd(){
        shieldState = RiotShieldState.Ready;
        hitBoxActiveParticles.Stop();
    }

    public void AttackHitBoxActive(){
        hitBoxActiveParticles.Play();
        shieldState = RiotShieldState.Swinging;
    }
    #endregion

    // When Shield is gone, set speed back to normal
    private void OnDestroy() {
        player.SpeedModifier = 1f;
    }

    //hit detection
    private void OnTriggerStay(Collider other) {
        if(shieldState != RiotShieldState.Swinging || !other.CompareTag("Player")) return;
        var hitPlayer = other.gameObject.GetComponent<Player>();

        if(hitPlayer.ID != player.ID){
            player.RegisterNewValidHit(player, hitPlayer, DamageAmount);
            CreateBloodSpray(hitPlayer.transform.position, transform.rotation);

            //set shieldstate into Damaging here so that only 1 hit per swing is applied as damage
            shieldState = RiotShieldState.Damaging;
            
        }
    }


}

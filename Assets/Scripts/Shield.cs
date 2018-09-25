using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : AbstractWeapon
{
    enum RiotShieldState {Ready, Swinging, Damaging, UsingMoveSkill}
    RiotShieldState shieldState = RiotShieldState.Ready;

    [Header("Cached References")]
    [SerializeField]
    Animator anim;
    [SerializeField]
    public float swingTime = 1f;

    [SerializeField]
    AudioSource swingSound;

    [SerializeField]
    BoxCollider hitBox;

    public int DamageAmount = 1;

    float DamageCooldown;

    void Start()
    {
        SpeedModifier = 0.5f;        
        player.SpeedModifier = SpeedModifier;
        DamageCooldown = swingTime;
    }

    #region Weapon Trigger Overrides
    public override void PullTrigger(Player player){
        if(shieldState != RiotShieldState.Ready)
            return;
        //player.canRotate = false;
        
        swingSound.Play();
        anim.SetBool("swinging", true);
    }

    public override void ReleaseTrigger(Player player){
        anim.SetBool("swinging", false);
    }
    #endregion

    #region Animation Event Functions
    public void AttackEnd(){
        //player.canRotate = true;
        shieldState = RiotShieldState.Ready;
    }

    public void AttackHitBoxActive(){
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

        if(hitPlayer.PlayerNumber != player.PlayerNumber){
            hitPlayer.Damage(DamageAmount, player.PlayerNumber);

            //set shieldstate into Damaging here so that only 1 hit per swing is applied as damage
            shieldState = RiotShieldState.Damaging;
        }
    }


}

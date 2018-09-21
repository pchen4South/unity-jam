using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : AbstractWeapon
{
    enum RiotShieldState {Ready, Swinging, Charging}
    RiotShieldState shieldState = RiotShieldState.Ready;

    [Header("Cached References")]
    [SerializeField]
    Animator anim;
    [SerializeField]
    public float swingTime = 1f;
    [SerializeField]
    AudioSource swingSound;

    void Start()
    {
        SpeedModifier = 0.5f;        
    }

    public override void PullTrigger(Player player){
        if(shieldState != RiotShieldState.Ready)
            return;
        shieldState = RiotShieldState.Swinging;
        swingSound.Play();
        anim.SetBool("swinging", true);
    }

    public override void ReleaseTrigger(Player player){
        shieldState = RiotShieldState.Ready;
        anim.SetBool("swinging", false);
    }

    private void OnDestroy() {
        player.SpeedModifier = 1f;
    }


}

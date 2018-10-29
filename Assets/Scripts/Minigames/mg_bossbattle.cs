using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_bossbattle : Minigame
{

    // Parent methods
    // public virtual void HandleMove(PlayerState p) {}
    // public virtual void HandleDash(PlayerState p) {}
    // public virtual void HandleRotate(PlayerState p) {}
    // public virtual void HandleFire(PlayerState p) {}
    // public virtual void HandlePlayerDamage(PlayerState attacker, PlayerState victim) {}
    void Start()
    {
        Debug.Log("boss battle has started");
    }

    public override void HandleMinigameCompleted(){
        Debug.Log("boss battle ended");
    }
    public override void HandleMove(PlayerState p) {
        InputHelpers.BasicMove(p);
    }

    public override void TabulateResults(){
        Debug.Log("results are compiled and ready");
        SetMinigameToResultsReady();
    }


}

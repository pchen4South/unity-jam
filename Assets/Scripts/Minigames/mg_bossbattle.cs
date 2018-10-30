using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Parent methods
// public virtual void HandleMove(PlayerState p) {}
// public virtual void HandleDash(PlayerState p) {}
// public virtual void HandleRotate(PlayerState p) {}
// public virtual void HandleFire(PlayerState p) {}
// public virtual void HandlePlayerDamage(PlayerState attacker, PlayerState victim) {}

public class mg_bossbattle : Minigame
{

    [SerializeField] public GameObject Boss;
    [SerializeField] public GameObject[] StageElementsToModify;

    public override void PrepareMinigame(){
        var boss_instance = Instantiate(Boss);
        StageElementsToModify = GameObject.FindGameObjectsWithTag("Disable_BossBattle");
        foreach(var ele in StageElementsToModify){
            ele.SetActive(false);
        }
        Destroy(boss_instance, MinigameDuration);
    }

    public override void HandleMinigameCompleted(){
        Debug.Log("boss battle ended");
        foreach(var ele in StageElementsToModify){
            ele.SetActive(true);
        }
    }
    public override void HandleMove(PlayerState p) {
        InputHelpers.BasicMove(p);
    }

    public override void TabulateResults(){
        SetMinigameToResultsReady();
    }


}

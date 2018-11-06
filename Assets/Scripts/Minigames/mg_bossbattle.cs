using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Parent methods
// public virtual void HandleMove(PlayerState p) {}
// public virtual void HandleDash(PlayerState p) {}
// public virtual void HandleRotate(PlayerState p) {}
// public virtual void HandleFire(PlayerState p) {}
// public virtual void HandlePlayerDamage(PlayerState attacker, PlayerState victim) {}

public class mg_bossbattle : Minigame
{

    [SerializeField] public BossMonster Boss;
    [SerializeField] public GameObject[] StageElementsToModify;

    public float IntroTimer = 10f;
    GameObject IntroscreenObject;



    // introscreen and prepareminigame need to be refactored, i think move into the abstract class based on state machine
    public override void PrepareMinigame(){
        MinigameIntroScreen.gameObject.SetActive(true);
        StartCoroutine(Introscreen());
    }

    IEnumerator Introscreen(){
        yield return new WaitForSeconds(IntroTimer);
        MinigameIntroScreen.gameObject.SetActive(false);
        BossMonster boss_instance = Instantiate(Boss);
        boss_instance.Initialize();
        StageElementsToModify = GameObject.FindGameObjectsWithTag("Disable_BossBattle");
        foreach(var ele in StageElementsToModify){
            ele.SetActive(false);
        }
        NPCS.Add(boss_instance.GetComponent<BossMonster>());
        Destroy(boss_instance.gameObject, MinigameDuration - IntroTimer);
    }

    public override void HandleMinigameCompleted(){
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

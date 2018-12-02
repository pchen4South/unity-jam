using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mg_bossbattle : AbstractMinigame
{
    [SerializeField] public BossMonster Boss;

    void Awake(){
        MinigameName = "BossBattle";
    }

    public override void PrepareMinigameObjects(){
        StageElementsToModify = GameObject.FindGameObjectsWithTag("Disable_BossBattle");
        foreach(var ele in StageElementsToModify)
        {
            ele.SetActive(false);
        }
        BossMonster boss_instance = Instantiate(Boss);
        boss_instance.Initialize();
        NPCS.Add(boss_instance.GetComponent<BossMonster>());
    }
    public override void HandleMinigameCompleted(){
        foreach(var ele in StageElementsToModify)
        {
            ele.SetActive(true);
        }
    }

    public override void TabulateResults(){
        foreach(var npc in NPCS){
            var hitList = npc.HitCounter;
            foreach(var mgPlayer in Results.MinigamePlayersArray)
            {
                var HitsForPlayer = hitList.FindAll(i => i.attackerIndex == mgPlayer.PlayerNumber);
                var totalDamageForPlayer = 0;

                foreach(var hit in HitsForPlayer)
                {
                    totalDamageForPlayer += hit.damageAmount;
                }
                mgPlayer.TotalScoreEarned = totalDamageForPlayer;
            }
            Results.CalculatePlayerPlacement();
            Destroy(npc.gameObject);
        }
        SetMinigameToResultsReady();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mg_bossbattle : AbstractMinigame
{
    public BossMonster Boss;
    public BossMonster BossPrefab;

    public override void PrepareMinigameObjects()
    {
        Boss = Instantiate(BossPrefab);
        Boss.OnValidHitOccurred = gameMode.HitsToBeProcessed.Add;
    }

    public override void Update()
    {
        base.Update();

        if (MinigameState == MG_State.Running && Boss.status == CharacterStatus.Dead)
        {
            MinigameState = MG_State.ResultsReady;
            TabulateResults();
            HandleMinigameCompleted();
            Destroy(Boss.gameObject);
        }
    }

    public override void TabulateResults()
    {
        foreach (var h in Boss.HitCounter)
        {
            Results.MinigamePlayersArray[h.attackerIndex].TotalScoreEarned += h.damageAmount;
        }
        Results.CalculatePlayerPlacement();
    }
}
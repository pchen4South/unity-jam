using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_duel : AbstractMinigame
{
    public GameMode gameMode;
    public PlayerState p1;
    public PlayerState p2;

    public override void HandleFire(PlayerState p)
    {
        if (p != p1 && p != p2)
            return;

        InputHelpers.BasicPullTrigger(p);
        InputHelpers.BasicReleaseTrigger(p);
    }

    public override void RunMinigame()
    {
        base.RunMinigame();
        
        // select players for the duel
        p1 = gameMode.playerStates[Random.Range(0, gameMode.playerStates.Length)];
        while (p1 == p2)
        {
            p2 = gameMode.playerStates[Random.Range(0, gameMode.playerStates.Length)];
        }
    }

    void ProcessHits()
    {
		foreach(var Newhit in gameMode.HitsToBeProcessed)
		{
            var isPvPAttack = Newhit.OriginatingEntityType == "PLAYERCHARACTER" && Newhit.VictimEntityType == "PLAYERCHARACTER";

            if (!isPvPAttack)
                continue;

            var attackerIndex = Newhit.OriginatingEntityIdentifier;
            var victim = (Player)Newhit.VictimEntity;
            var attacker = (Player)gameMode.playerStates[attackerIndex].player;
            var validAttack = (attacker == p1.player || attacker == p2.player) && (victim == p1.player || victim == p2.player);

            if (!validAttack)
                continue;

			gameMode.ProcessedHits.Add(Newhit);
		}
		gameMode.HitsToBeProcessed.Clear();
    }
}
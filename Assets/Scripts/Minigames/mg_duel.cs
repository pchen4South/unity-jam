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
    }
}
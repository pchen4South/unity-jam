using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_arenafire : AbstractMinigame
{
    public override void RunMinigame(){
        SetMinigameToRunning();
        MinigameAliveTimer = 0f;
    }
}

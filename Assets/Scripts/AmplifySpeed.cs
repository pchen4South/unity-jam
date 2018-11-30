using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifySpeed : AbstractPowerup
{
    public override void ApplyEffect(Player player){
        player.SetSpeedMultiplier(EffectMultiplier, BuffTimer);
    }
}

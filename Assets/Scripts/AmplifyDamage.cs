using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifyDamage : AbstractPowerup
{
    public override void ApplyEffect(Player player){
        player.SetDamageMultiplier(EffectMultiplier, BuffTimer);
    }
}

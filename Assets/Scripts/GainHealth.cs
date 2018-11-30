using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainHealth : AbstractPowerup
{
    public int healAmount = 3;
    public override void ApplyEffect(Player player){
        player.HealForAmount(healAmount);
    }
}

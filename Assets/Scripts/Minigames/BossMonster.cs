using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : AbstractCharacter
{
    public int Health = 0;
    public RectTransform HPBar;

        //Event Broadcasting
	public delegate void ValidHitOccurredEvent(ValidHit NewHit);
	public event ValidHitOccurredEvent OnValidHitOccurred;

    public void Initialize()
    {
        Health = MaxHealth;    
        ENTITY_TYPE = "NPC";
    }

    public void DamageMonster(int attackerIndex, int damageAmount)
    {
        if(IsAlive())
        {
            Health = Mathf.Max(0, Health - damageAmount);
            HPBar.sizeDelta = new Vector2 (300 * Health / MaxHealth, HPBar.rect.height);
            HitCounter.Add(new HitCounter(attackerIndex, damageAmount));
        }
    }
}

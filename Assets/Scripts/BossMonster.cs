using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : AbstractCharacter
{
    public int Health = 0;
    public RectTransform HPBar;
    BossMonster(){
        ENTITY_TYPE = "NPC";
    }

    public void Initialize(){
        Health = MaxHealth;    
    }

    public void DamageMonster(int attackerIndex, int damageAmount){
        Debug.Log("Health " + Health);

        if(IsAlive()){
            Health = Mathf.Max(0, Health - damageAmount);
            HPBar.sizeDelta = new Vector2 (300 * Health / MaxHealth, HPBar.rect.height);
        }
    }

    private void OnDestroy() {
        OnDamage = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : AbstractCharacter
{
    public RectTransform HPBar;

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);
        float hp = (float)Health / (float)MaxHealth;

        HPBar.sizeDelta = new Vector2(300 * hp, HPBar.rect.height);
    }
}

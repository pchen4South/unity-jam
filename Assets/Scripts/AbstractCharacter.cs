using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitCounter
{
    public int attackerIndex;
    public int damageAmount;
    public HitCounter(int index, int dmg)
    {
        attackerIndex = index;
        damageAmount = dmg;
    }
}

public enum CharacterStatus { Alive, Dead, Spawning }

public abstract class AbstractCharacter : MonoBehaviour
{
    public Vector3 GROUNDED_DOWNWARD_VELOCITY = new Vector3(0, -10f, 0);
    public int MaxHealth = 3;

    public int ID = 0;
    public CharacterStatus status = CharacterStatus.Alive;
    public List<HitCounter> HitCounter = new List<HitCounter>();
    public int Health = 1;
    public float damageMultiplier = 1f;
    public System.Action<ValidHit> OnValidHitOccurred;

    public bool IsDead()
    {
        return status == CharacterStatus.Dead;
    }

    public bool IsAlive()
    {
        return status == CharacterStatus.Alive;
    }

    public bool IsSpawning()
    {
        return status == CharacterStatus.Spawning;
    }

    public virtual void Damage(int damageAmount)
    {
        if (Health <= damageAmount)
        {
            status = CharacterStatus.Dead;
            Health = 0;
        }
        else
        {
            Health -= damageAmount;
        }
    }

	public void RegisterNewValidHit(AbstractCharacter attacker, AbstractCharacter victim, int damageAmount)
    {
        ValidHit h = new ValidHit
        {
            attacker = attacker,
            victim = victim,
        };

        if (attacker is Player p)
        {
            h.weapon = p.Weapon; 
            h.damageAmount = Mathf.RoundToInt(damageAmount * p.damageMultiplier);
        }

        OnValidHitOccurred(h);
	}
}
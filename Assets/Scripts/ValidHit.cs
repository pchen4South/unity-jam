using UnityEngine;

public struct ValidHit
{
    public AbstractCharacter victim;
    public AbstractCharacter attacker;
    public AbstractWeapon weapon;
    public GameObject projectile;
    public int damageAmount;
}
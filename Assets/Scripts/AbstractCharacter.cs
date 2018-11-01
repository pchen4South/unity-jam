using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
    public int ID = 0;
    public string ENTITY_TYPE;
    public int MaxHealth = 3;
    public Vector3 GROUNDED_DOWNWARD_VELOCITY = new Vector3(0, -10f, 0);
    
    //attackerIndex victimIndex amountOfDamage
    public System.Action<int, int, int> OnDamage;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidHit
{
    //player, npc, level hazard
    public string OriginatingEntityType { get; set; }
    //player number, etc.
    public int OriginatingEntityIdentifier { get; set; }
    public string VictimEntityType{get;set;}
    public AbstractCharacter VictimEntity {get; set;}
    //name of weapon if applicable
    public string WeaponName{ get; set; }
    //amount of dmg
    public int DamageAmount{ get; set; }
    

    
    // if want damage types, burn, pierce, fall, drown, etc
    // public string DamageType{get;set;}
       
}

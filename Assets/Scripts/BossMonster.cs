using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : AbstractCharacter
{
    BossMonster(){
        ENTITY_TYPE = "NPC";
    }
    public void GotHit(){
        Debug.Log("OW");
    }
}

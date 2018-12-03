using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Projectile_Damage : MonoBehaviour
{
    [SerializeField] AbstractCharacter NPC = null;
    [SerializeField] int DamageAmount = 1;
    


    private void OnParticleCollision(GameObject other) {
        if(other.tag != "Player") return;
        var target = other.GetComponent<Player>();
        NPC.RegisterNewValidHit(NPC, target, DamageAmount);
    }
}

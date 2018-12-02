using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunHitDetect : MonoBehaviour
{
    public ParticleSystem shotgunPellets;
    List<ParticleCollisionEvent> collisionEvents;
    int firingPlayerNumber;
    [SerializeField] ParticleSystem HitPlayerParticlePrefab;
    [SerializeField] Shotgun shotgun;

    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent> ();
        firingPlayerNumber = this.GetComponentInParent<Player>().ID;
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(shotgunPellets, other, collisionEvents);
        
        int i = 0;
        while (i < numCollisionEvents) 
        {
            if (other.tag == "Player")
            {
                var target = other.gameObject.GetComponent<Player>();
                var targetPlayerNumber = target.ID;
                if(targetPlayerNumber != firingPlayerNumber)
                {
                    shotgun.player.RegisterNewValidHit(shotgun.player, target, shotgun.DamageAmount);
                    shotgun.CreateBloodSpray(target.transform.position, transform.rotation);
                }
            } else if (other.tag == "NPCHitbox"){
                var target = other.gameObject.GetComponent<AbstractCharacter>();
                shotgun.player.RegisterNewValidHit(shotgun.player, target, shotgun.DamageAmount);
            }
            i++;
        }
    }
}

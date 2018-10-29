using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunHitDetect : MonoBehaviour
{
    public ParticleSystem blastRadius;
    List<ParticleCollisionEvent> collisionEvents;
    // Start is called before the first frame update
    int firingPlayerNumber;
    [SerializeField]
    ParticleSystem HitPlayerParticlePrefab;


    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent> ();
        firingPlayerNumber = this.GetComponentInParent<Player>().PlayerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents (blastRadius, other, collisionEvents);
        
        int i = 0;
        while (i < numCollisionEvents) 
        {
            
            if (other.tag == "Player")
            {
                var target = other.gameObject.GetComponent<Player>();
                var targetPlayerNumber = target.PlayerNumber;

                if(targetPlayerNumber != firingPlayerNumber)
                {
                    target.OnDamage(firingPlayerNumber, target.PlayerNumber, 1);
                }
            }
            i++;
        }

    }
}

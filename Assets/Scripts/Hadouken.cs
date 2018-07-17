using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hadouken : MonoBehaviour {

    [Header("Cached components")]
    public Rigidbody body;

    // [Header("Configuration")]
    // public float fuseTime = 3f;
    // private float timeAlive = 0f;

    // [Header("Prefabs")]
    // public GrenadeExplosion Explosion;

    [Header("State")]
    public int PlayerNumber;
	
	void OnCollisionEnter(Collision other)
    {
        //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
        if (other.gameObject.tag == "Player" && PlayerNumber != other.gameObject.GetComponent<Player>().PlayerNumber)
        {
			var target = other.gameObject.GetComponent<Player>();
			target.Health -= 1;
        }
		if(other.gameObject.tag != "Ground" || 
		(other.gameObject.tag == "Player" && PlayerNumber != other.gameObject.GetComponent<Player>().PlayerNumber)){
			Destroy(gameObject);
		}
    }
}

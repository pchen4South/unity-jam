using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullet : MonoBehaviour {

	[Header("Cached components")]
    public Rigidbody body;

    [Header("Configuration")]
    private float timeAlive = 0f;

    
    [Header("State")]
    public int PlayerNumber;
	
	void Update() 
    {
        //grow larger over time test
        //transform.localScale += new Vector3(0.5f,0.5f,0.5f) * Time.deltaTime;
        timeAlive += Time.deltaTime;

	}

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

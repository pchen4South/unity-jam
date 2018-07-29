using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hadouken : MonoBehaviour 
{
    [Header("Cached components")]
    public Rigidbody body;

    [Header("State")]
    public int PlayerNumber;
	
	void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerNumber != other.gameObject.GetComponent<Player>().PlayerNumber)
        {
			var target = other.gameObject.GetComponent<Player>();

			target.Damage(1, PlayerNumber);
        }
        else
        {
			Destroy(gameObject);
		}
    }
}
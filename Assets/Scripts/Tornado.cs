using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

	    [Header("Cached components")]
    public Rigidbody body;

    [Header("Configuration")]
    public float aliveTime = 1.5f;
    private float timeAlive = 0f;

    
    [Header("State")]
    public int PlayerNumber;

    void Start(){
        Debug.Log("Tornado");
    }


	void Update() 
    {
        //grow larger over time test
        //transform.localScale += new Vector3(0.5f,0.5f,0.5f) * Time.deltaTime;
        timeAlive += Time.deltaTime;

        if (timeAlive >= aliveTime) 
        {
            //var explosion = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);

            //explosion.PlayerNumber = PlayerNumber;
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().PlayerNumber != PlayerNumber)
        {
            var player = other.gameObject.GetComponent<Player>();
            player.Health -= 1;
            Destroy(gameObject);
        } 
    }
}

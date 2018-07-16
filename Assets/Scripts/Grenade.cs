using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float fuseTime = 3f;
    private float timeAlive = 0f;
    public GameObject Explosion;

    [SerializeField]
    private int PlayerNumber;

    public void SetPlayerNumber(Player player){
        PlayerNumber = player.PlayerNumber;
    }
	// Use this for initialization
	void Start ()
    {
        //Rigidbody rigidbody = GetComponent<Rigidbody>();

        //rigidbody.velocity = transform.forward * 6;
    }
	
	// Update is called once per frame
	void Update () {

        AudioSource explosion = GetComponent<AudioSource>();

        timeAlive += Time.deltaTime;

        if (timeAlive >= fuseTime) {
            explosion.Play();
            var theExplosion = (GameObject)Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
            theExplosion.GetComponent<GrenadeExplosion>().PlayerNumber = PlayerNumber;

            Destroy(gameObject);
        }

	}

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag != "Player"){
            var rb = gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }             
    }

  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float fuseTime = 3f;
    private float timeAlive = 0f;
    public GameObject Explosion;


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

        if (timeAlive >= 3) {
            explosion.Play();
            var theExplosion = (GameObject)Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.velocity = transform.forward * 6;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

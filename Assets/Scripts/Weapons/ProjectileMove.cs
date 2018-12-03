using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

	public float travelSpeed = 5f;
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * travelSpeed * Time.deltaTime;
	}
}

// script to render explosion
using UnityEngine;
using System.Collections;

public class GrenadeExplosion : MonoBehaviour {
	public float loopduration;
	private float ramptime=0;
	private float alphatime=1;	

	public int PlayerNumber;

	void Start()
	{
		CheckExplosionRadius();
	}
	void Update () {
		Destroy(gameObject, 7);
		ramptime+=Time.deltaTime*2;
		alphatime-=Time.deltaTime;		
		float r = Mathf.Sin((Time.time / loopduration) * (2 * Mathf.PI)) * 0.5f + 0.25f;
		float g = Mathf.Sin((Time.time / loopduration + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
		float b = Mathf.Sin((Time.time / loopduration + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
		float correction = 1 / (r + g + b);
		r *= correction;
		g *= correction;
		b *= correction;
		GetComponent<Renderer>().material.SetVector("_ChannelFactor", new Vector4(r,g,b,0));
		GetComponent<Renderer>().material.SetVector("_Range", new Vector4(ramptime,0,0,0));
		GetComponent<Renderer>().material.SetFloat("_ClipRange", alphatime);


	}

	private void CheckExplosionRadius(){

		//var explosionRadius = transform.gameObject.GetComponent<CapsuleCollider>().radius;

		//Debug.Log("Explosion radius: " + explosionRadius);
		Debug.Log("Explosion position: " + transform.position);

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);

		Debug.Log("hit count : " + hitColliders.Length);

		foreach (Collider col in hitColliders){
			if(col.gameObject.tag == "Player")
				if(col.gameObject.GetComponent<Player>().PlayerNumber != PlayerNumber)
					Destroy(col.gameObject);
		}


        

	}


}

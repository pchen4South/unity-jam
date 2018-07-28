// script to render explosion
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GrenadeExplosion : MonoBehaviour 
{
	[Header("Cached references")]
	public AudioSource Sound;
	public Renderer Renderer;

	[Header("Configuration")]
	private float ramptime = 0;
	private float alphatime = 1;	
	public float loopduration = 1;
	public float despawnTime = 7f;
    public List<int> PlayersHit = new List<int>();

	[Header("State")]
	public int PlayerNumber;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 2f);
    }

    void Start()
	{
		CheckExplosionRadius();
	}

	void Update () 
	{
		float r = Mathf.Sin((Time.time / loopduration) * (2 * Mathf.PI)) * 0.5f + 0.25f;
		float g = Mathf.Sin((Time.time / loopduration + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
		float b = Mathf.Sin((Time.time / loopduration + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
		float correction = 1 / (r + g + b);

		r *= correction;
		g *= correction;
		b *= correction;
		ramptime += Time.deltaTime * 2;
		alphatime -= Time.deltaTime;		
		Renderer.material.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
		Renderer.material.SetVector("_Range", new Vector4(ramptime, 0, 0, 0));
		Renderer.material.SetFloat("_ClipRange", alphatime);
		Destroy(gameObject, despawnTime);
	}

	void CheckExplosionRadius()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);

    

		Sound.Play();
		foreach (Collider col in hitColliders)
		{
			if(col.gameObject.tag == "Player")
			{
				var player = col.gameObject.GetComponent<Player>();
                var pNum = player.PlayerNumber;

                if (pNum != PlayerNumber && !PlayersHit.Contains(pNum))
				{
                    PlayersHit.Add(pNum);
					player.Damage(1, PlayerNumber);
				}
			}
		}
	}
}
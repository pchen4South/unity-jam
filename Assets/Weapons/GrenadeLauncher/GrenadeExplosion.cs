using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GrenadeExplosion : MonoBehaviour 
{
	[Header("Cached references")]
	public AudioSource Sound;

	[Header("Configuration")]
	public float despawnTime = 7f;
    public List<int> PlayersHit = new List<int>();

	[Header("State")]
	public int PlayerNumber;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1.5f);
    }

    void Start()
	{
		CheckExplosionRadius();
	}

	void Update () 
	{
		Destroy(gameObject, despawnTime);
	}

	void CheckExplosionRadius()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);

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
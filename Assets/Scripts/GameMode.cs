using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour 
{
	[Header("Prefabs")]
	[SerializeField]
	AbstractWeapon[] WeaponPrefabs;

	[SerializeField]
	Player PlayerPrefab;

	[Header("Configuration")]
	[SerializeField]
	DebugConfig debugConfig;

	[Header("Map Items")]
	[SerializeField]
	GameObject[] playerSpawnLocations;

	[Header("State")]
	List<Player> players = new List<Player>();

	void OnDrawGizmos()
	{
		Gizmos.color = debugConfig.PlayerSpawnColor;
		foreach(var s in playerSpawnLocations)
		{
			var position = s.transform.position;
			var rotation = s.transform.rotation;
			var scale = new Vector3(.5f, .5f, .5f);

			Gizmos.DrawWireMesh(debugConfig.PlayerSpawnMesh, 0, position, rotation, scale);
		}
	}

	void Start()
	{
		Spawn(0);
		Spawn(1);
	}

	void Update()
	{
		for(var i = 0; i < players.Count; i++)
		{
			var player = players[i];

			if (player.Health <= 0)
			{

				players[i] = Spawn(player.PlayerNumber);
				Destroy(player.gameObject);
			}
		}
	}

	Player Spawn(int PlayerNumber)
	{
		var spawnIndex = Random.Range(0, playerSpawnLocations.Length);
		var spawn = playerSpawnLocations[spawnIndex];
		var player = Instantiate(PlayerPrefab, spawn.transform.position, spawn.transform.rotation, transform);

		player.name = "Player " + PlayerNumber;
		player.PlayerNumber = PlayerNumber;
		players.Add(player);
		return player;
	}
}
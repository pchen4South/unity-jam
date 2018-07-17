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

	[Header("Game")]
	[SerializeField]
	GameSettings GameSettings;

	[Header("Sounds")]
	[SerializeField]
	AudioSource BackgroundMusic;

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
		
		players.Add(Spawn(0));
		/* players.Add(Spawn(1));*/
	}

	void OnEnable()
	{
		if (GameSettings.PlayBackgroundMusic)
		{
			BackgroundMusic.Play();
			BackgroundMusic.volume = GameSettings.BackgroundMusicVolume;
		}
	}

	void OnDisable()
	{
		BackgroundMusic.Pause();
	}

	void Update()
	{
		if (GameSettings.PlayBackgroundMusic)
		{
			if (!BackgroundMusic.isPlaying)
			{
				BackgroundMusic.Play();
			}
		}
		else {
			if (BackgroundMusic.isPlaying)
			{
				BackgroundMusic.Pause();
			}
		}
		BackgroundMusic.volume = GameSettings.BackgroundMusicVolume;

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
		var weapon = Instantiate(WeaponPrefabs[0], player.transform);

		weapon.player = player;
		player.Weapon = weapon;
		player.name = "Player " + PlayerNumber;
		player.PlayerNumber = PlayerNumber;

		return player;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour 
{
	[Header("Prefabs")]
	[SerializeField]
	Player PlayerPrefab;

	[Header("Game")]
	[SerializeField]
	GameSettings GameSettings;

	[Header("Sounds")]
	[SerializeField]
	AudioSource BackgroundMusic;

	[Header("Configuration")]
	public DebugConfig debugConfig;
	public AbstractWeapon[] WeaponPrefabs;
	public GameObject[] playerSpawnLocations;

	[Header("State")]
	public List<Player> players = new List<Player>();
	List<int> spawnIndices = new List<int>();

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
		
		for (int i = 0; i < playerSpawnLocations.Length; i++){
			spawnIndices.Add(i);
		}

		// player 0 is the dev player
		// players.Add(Spawn(0));
		players.Add(Spawn(1, true));
		players.Add(Spawn(2, true));
		players.Add(Spawn(3, true));
		players.Add(Spawn(4, true));

	}

	void OnEnable()
	{
		if (GameSettings.PlayBackgroundMusic)
		{
			BackgroundMusic.volume = GameSettings.BackgroundMusicVolume;
			BackgroundMusic.Play();
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
				players[i] = Spawn(player.PlayerNumber, false);
				Destroy(player.gameObject);
			}
		}
	}

	Player Spawn(int PlayerNumber, bool initialSpawn)
	{
		GameObject spawn;
		if(initialSpawn)
		{
		//edited spawn so each player spawns at a unique location
			var spawnIndex = Random.Range(0, spawnIndices.Count);
			spawn = playerSpawnLocations[spawnIndices[spawnIndex]];
			spawnIndices.RemoveAt(spawnIndex);
		} 
		else 
		{
			var spawnIndex = Random.Range(0, playerSpawnLocations.Length);
			spawn = playerSpawnLocations[spawnIndex];
		}

		var player = Instantiate(PlayerPrefab, spawn.transform.position, spawn.transform.rotation, transform);
		var weapon = Instantiate(WeaponPrefabs[0], player.transform);

		weapon.player = player;
		player.Weapon = weapon;
		player.name = "Player " + PlayerNumber;
		player.PlayerNumber = PlayerNumber;
		return player;
	}
}
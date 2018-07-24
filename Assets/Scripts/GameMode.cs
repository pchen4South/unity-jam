﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour 
{
	[System.Serializable]
	class PlayerState
	{
		public int weaponIndex = 0;
		public int killCount = 0;
		public int deathCount = 0;
		public Player player;
		public PlayerState(Player Player)
		{
			player = Player;
		}
	}

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

	[Header("State")]
	List<PlayerState> playerStates = new List<PlayerState>();
	GameObject[] spawnPoints;

	void Start()
	{
		var debugPlayer = GameObject.FindObjectOfType<Player>();

		playerStates.Add(new PlayerState(debugPlayer));

		// crawl the map collecting references to all spawn points
		spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

		for (var i = 1; i <= 1; i++)
		{
			Spawn(i);
		}

		StartCoroutine(UpdateSettings());
	}

	void Update()
	{
		var gunCount = WeaponPrefabs.Length;

		for(var i = 0; i < playerStates.Count; i++)
		{
			var playerState = playerStates[i];

			if (playerState.player.Health <= 0)
			{
				var attackerIndex = playerState.player.lastAttackerIndex;
				var attackerState = playerStates[attackerIndex];

				attackerState.killCount += 1;
				playerState.deathCount += 1;

				if (attackerState.weaponIndex + 1 >= gunCount)
				{
					Debug.Log("Game over. Player " + attackerState.player.PlayerNumber + " wins!");
					foreach(var ps in playerStates)
					{
						ps.deathCount = 0;
						ps.killCount = 0;
						ps.weaponIndex = 0;
						Respawn(ps.player);
						ps.player.SetWeapon(WeaponPrefabs[ps.weaponIndex]);
					}
				}
				else
				{
					attackerState.weaponIndex += 1;
					attackerState.player.SetWeapon(WeaponPrefabs[attackerState.weaponIndex]);
					Respawn(playerState.player);
				}
			}
		}
	}

	Player Spawn(int PlayerNumber)
	{
		var player = Instantiate(PlayerPrefab);

		playerStates.Add(new PlayerState(player));
		player.PlayerNumber = PlayerNumber;
		player.name = "Player " + PlayerNumber;
        player.HorizontalInput = "Horizontal_" + PlayerNumber;
        player.VerticalInput = "Vertical_" + PlayerNumber;
        player.FireInput = "Fire_" + PlayerNumber;
        player.JumpInput = "Jump_" + PlayerNumber;
		return Respawn(player);
	}

	Player Respawn(Player player)
	{
		var spawnIndex = Random.Range(0, spawnPoints.Length);
		var spawn = spawnPoints[spawnIndex];
		
		player.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
		player.Health = 3;
		player.canMove = true;
		player.canRotate = true;
		player.VerticalVelocity = 0f;
		return player;
	}

	IEnumerator UpdateSettings()
	{
		var wait = new WaitForSeconds(1f);

		while (true)
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

			yield return wait;
		}
	}
}
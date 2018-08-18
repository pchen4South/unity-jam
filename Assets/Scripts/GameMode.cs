using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerState
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

public class GameMode : MonoBehaviour 
{
	[Header("Prefabs")]
	[SerializeField]
	Player PlayerPrefab;
	[SerializeField]
	Graph GraphPrefab;

	[Header("Game")]
	[SerializeField]
	GameSettings GameSettings;
	Graph graph;

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

        if(debugPlayer != null) { 
		    playerStates.Add(new PlayerState(debugPlayer));
		    debugPlayer.Respawn(debugPlayer.transform.position, debugPlayer.transform.rotation);
        }
        // crawl the map collecting references to all spawn points
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

		graph = Instantiate(GraphPrefab);

		for (var i = playerStates.Count; i < 2; i++)
		{
			Spawn(i);
		}

		for (var i = 0; i < playerStates.Count; i++)
		{
			playerStates[i].player.SetWeapon(WeaponPrefabs[playerStates[i].weaponIndex]);
			playerStates[i].player.color = GameSettings.playerColors[i];
		}
	}

	void Update()
	{
		var gunCount = WeaponPrefabs.Length;

		for(var i = 0; i < playerStates.Count; i++)
		{
			var playerState = playerStates[i];

			if (playerState.player.Health <= 0 && playerState.player.IsDead)
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
						var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

						ps.deathCount = 0;
						ps.killCount = 0;
						ps.weaponIndex = 0;
						ps.player.Respawn(sp.transform.position, sp.transform.rotation);
						ps.player.SetWeapon(WeaponPrefabs[ps.weaponIndex]);
					}
				}
				else
				{
					var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

					attackerState.weaponIndex += 1;
					attackerState.player.SetWeapon(WeaponPrefabs[attackerState.weaponIndex]);
					playerState.player.Respawn(sp.transform.position, sp.transform.rotation);
				}
			}
		}

		// Update the graphs for gun status
		for (var i = 0; i < playerStates.Count; i++)
		{
			var ps = playerStates[i];
			var normalizedScale = (float)ps.weaponIndex / (float)gunCount;

			graph.UpdateBar(i, ps.player.color, normalizedScale);
		}

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
	}

	void Spawn(int PlayerNumber)
	{
		var player = Instantiate(PlayerPrefab);
		var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

		playerStates.Add(new PlayerState(player));
		player.PlayerNumber = PlayerNumber;
		player.name = "Player " + PlayerNumber;
        player.HorizontalInput = "Horizontal_" + PlayerNumber;
        player.VerticalInput = "Vertical_" + PlayerNumber;
        player.FireInput = "Fire_" + PlayerNumber;
        player.JumpInput = "Jump_" + PlayerNumber;
		player.Respawn(sp.transform.position, sp.transform.rotation);
	}
}
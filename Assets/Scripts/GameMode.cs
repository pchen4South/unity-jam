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

[System.Serializable]
public class PlayerHUDManager : Object
{
	PlayerHUD[] playerHUDPool;

	public PlayerHUDManager(PlayerHUD PlayerHUDPrefab, int maximumSize)
	{
		playerHUDPool = new PlayerHUD[maximumSize];

		for (var i = 0; i < playerHUDPool.Length; i++)
		{
			playerHUDPool[i] = Instantiate(PlayerHUDPrefab);
		}
	}

	void OnDestroy()
	{
		for (var i = 0; i < playerHUDPool.Length; i++)
		{
			Destroy(playerHUDPool[i]);
		}
	}

	public void UpdatePlayerHUDs(PlayerState[] playerStates, Camera camera, RectTransform parent)
	{
		var i = 0;

		while (i < playerStates.Length)
		{
			playerHUDPool[i].gameObject.SetActive(true);
			playerHUDPool[i].transform.SetParent(parent, false);
			playerHUDPool[i].UpdateHealth(playerStates[i].player.Health, playerStates[i].player.MaxHealth);
			playerHUDPool[i].UpdatePosition(camera, parent, playerStates[i].player.transform.position);
			i++;
		}
		while (i < playerHUDPool.Length)
		{
			playerHUDPool[i].gameObject.SetActive(false);
			i++;
		}
	}
}

public class GameMode : MonoBehaviour 
{
	[SerializeField] Player PlayerPrefab;
	[SerializeField] PlayerHUD PlayerHUDPrefab;

	[SerializeField] GameSettings GameSettings;
	[SerializeField] Shakeable shakeable;
	[SerializeField] Graph graph;
	[SerializeField] Canvas screenSpaceUICanvas;

	[SerializeField] AudioSource BackgroundMusic;

	[SerializeField] AbstractWeapon[] WeaponPrefabs;

	PlayerState[] playerStates;
	GameObject[] spawnPoints;
	PlayerHUDManager playerHUDManager;

	void Start()
	{
		var playerCount = 2;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		playerStates = new PlayerState[playerCount];

		// for each connected controller, spawn a player
		for (var i = 0; i < playerCount; i++)
		{
			Spawn(i);
		}

		for (var i = 0; i < playerStates.Length; i++)
		{
			playerStates[i].player.SetWeapon(WeaponPrefabs[playerStates[i].weaponIndex]);
			playerStates[i].player.color = GameSettings.playerColors[i];
		}

		// Instantiate UI Objects
		playerHUDManager = new PlayerHUDManager(PlayerHUDPrefab, 8);
	}

	void Update()
	{
		var gunCount = WeaponPrefabs.Length;

		for(var i = 0; i < playerStates.Length; i++)
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

		// always push timescale back towards full-speed
		Time.timeScale += (1 - Time.timeScale) * .1f * Time.timeScale;

		// Update the graphs for gun status
		for (var i = 0; i < playerStates.Length; i++)
		{
			var ps = playerStates[i];
			var normalizedScale = (float)ps.weaponIndex / (float)gunCount;

			graph.UpdateBar(i, ps.player.color, normalizedScale);
		}

		// Update the player HUDs
		playerHUDManager.UpdatePlayerHUDs(playerStates, shakeable.shakyCamera, screenSpaceUICanvas.transform as RectTransform);

		if (GameSettings.PlayBackgroundMusic)
		{
			if (!BackgroundMusic.isPlaying)
			{
				BackgroundMusic.Play();
			}
		}
		else 
		{
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

		playerStates[PlayerNumber] = new PlayerState(player);
		player.PlayerNumber = PlayerNumber;
		player.shakeable = shakeable;
		player.name = "Player " + PlayerNumber;
		player.Respawn(sp.transform.position, sp.transform.rotation);
	}
}
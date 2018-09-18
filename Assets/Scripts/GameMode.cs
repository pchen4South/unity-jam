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
	public enum GameState { PreGame, Countdown, Live, Victory, PostGame };

	[SerializeField] Player PlayerPrefab;
	[SerializeField] PlayerHUD PlayerHUDPrefab;

	[SerializeField] ColorScheme colorScheme;
	[SerializeField] Shakeable shakeable;
	[SerializeField] Graph graph;
	[SerializeField] Canvas screenSpaceUICanvas;
	[SerializeField] AudioSource BackgroundMusic;
	[SerializeField] AbstractWeapon[] WeaponPrefabs;

	public float RespawnTimer = 3f;

	GameObject[] spawnPoints;
	PlayerState[] playerStates;
	int winningPlayerIndex;
	PlayerHUDManager playerHUDManager;
	GameState state = GameState.PreGame;

	void Start()
	{
		var playerCount = 2;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		playerStates = new PlayerState[playerCount];
		for (var i = 0; i < playerCount; i++)
		{
			var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
			var player = Instantiate(PlayerPrefab);
			var ps = new PlayerState(player);
			var WeaponPrefab = WeaponPrefabs[ps.weaponIndex];

			ps.player.PlayerNumber = i;
			ps.player.name = "Player " + i;
			ps.player.SetWeapon(WeaponPrefab);
			ps.player.OnDeath = HandlePlayerDeath;
			ps.player.color = colorScheme.playerColors[i];
			ps.player.Spawn(sp.transform);
			playerStates[i] = ps;
		}

		playerHUDManager = new PlayerHUDManager(PlayerHUDPrefab, 8);
	}

	void Update()
	{
		switch (state)
		{
			case GameState.PreGame:
				Debug.Log("PreGame: LOOK AROUND!");
			break;
			case GameState.Countdown:
            	Debug.Log("Countdown: YOU CAN MOVE BUT CAN'T SHOOT!");
			break;
			case GameState.Live:
            	Debug.Log("Live: GAME ON!");
			break;
			case GameState.Victory:
            	Debug.Log("Victory: PLAYER " + winningPlayerIndex + " HAS TAKEN IT!");
			break;
			case GameState.PostGame:
            	Debug.Log("PostGame: WHY NOT GET A SNACK?");
			break;
		}

		// always push timescale back towards full-speed
		Time.timeScale += (1 - Time.timeScale) * .1f * Time.timeScale;

		// Update the graphs for gun status
		var gunCount = WeaponPrefabs.Length;
		for (var i = 0; i < playerStates.Length; i++)
		{
			var ps = playerStates[i];
			var normalizedScale = (float)ps.weaponIndex / (float)gunCount;

			graph.UpdateBar(i, ps.player.color, normalizedScale);
		}

		// Update the player HUDs
		playerHUDManager.UpdatePlayerHUDs(playerStates, shakeable.shakyCamera, screenSpaceUICanvas.transform as RectTransform);
	}

	void HandlePlayerDeath(int killedIndex, int killerIndex)
	{
		var killedPlayerState = playerStates[killedIndex];
		var killerPlayerState = playerStates[killerIndex];
		var gunCount = WeaponPrefabs.Length;

		shakeable.AddIntensity(1f);
		Time.timeScale = .1f;
		killedPlayerState.deathCount++;
		killerPlayerState.killCount++;

		if (killerPlayerState.weaponIndex >= gunCount - 1)
		{
			winningPlayerIndex = killerIndex;
			state = GameState.Victory;
		}
		else
		{
			killerPlayerState.weaponIndex++;
			killerPlayerState.player.SetWeapon(WeaponPrefabs[killerPlayerState.weaponIndex]);
			StartCoroutine(RespawnAfter(killedPlayerState, RespawnTimer));
		}
	}

	IEnumerator RespawnAfter(PlayerState ps, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		ps.player.Spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
	}
}
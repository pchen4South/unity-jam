using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[System.Serializable]
public class PlayerState
{
	public int weaponIndex = 0;
	public int killCount = 0;
	public int deathCount = 0;
	public Player player;
	public Rewired.Player playerController;
	public PlayerState(Player Player, Rewired.Player PlayerController)
	{
		player = Player;
		playerController = PlayerController;
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
	public enum GameState { PreGame, Live, Victory, PostGame };

	[SerializeField] Player PlayerPrefab;
	[SerializeField] PlayerHUD PlayerHUDPrefab;

	[SerializeField] ColorScheme colorScheme;
	[SerializeField] Shakeable shakeable;
	[SerializeField] Graph graph;
	[SerializeField] Canvas screenSpaceUICanvas;
	[SerializeField] AudioSource BackgroundMusic;
	[SerializeField] AbstractWeapon[] WeaponPrefabs;

	public float RespawnTimer = 3f;
	public float CountdownDuration = 3f;

	GameObject[] spawnPoints;
	PlayerState[] playerStates;
	PlayerHUDManager playerHUDManager;
	GameState state = GameState.PreGame;
	int winningPlayerIndex;
	float remainingCountdownDuration;

	// TODO: I like the idea of not using Start for this but making an explicit method?
	void Start()
	{
		var playerCount = 2;

		remainingCountdownDuration = CountdownDuration;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		playerStates = new PlayerState[playerCount];
		for (var i = 0; i < playerCount; i++)
		{
			var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
			var player = Instantiate(PlayerPrefab);
			var ps = new PlayerState(player, ReInput.players.GetPlayer(i));
			var WeaponPrefab = WeaponPrefabs[ps.weaponIndex];

			ps.player.PlayerNumber = i;
			ps.player.name = "Player " + i;
			ps.player.SetWeapon(WeaponPrefab);
			ps.player.OnDeath = HandlePlayerDeath;
			ps.player.SetColor(colorScheme.playerColors[i]);
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
				if (remainingCountdownDuration > 0f)
				{
					remainingCountdownDuration -= Time.deltaTime;
				}
				else 
				{
					state = GameState.Live;
				}
			break;
			case GameState.Live:
				for (var i = 0; i < playerStates.Length; i++)
				{
					var c = playerStates[i].playerController;
					var p = playerStates[i].player;
					var triggerDown = c.GetButton("Fire") || c.GetButtonRepeating("Fire");
					var triggerUp = c.GetButtonUp("Fire");
					var xAxis = c.GetAxis(0);
					var yAxis = c.GetAxis(1);
					var moving = xAxis != 0f || yAxis != 0f;

					if (triggerDown && p.Weapon)
					{
						p.Weapon.PullTrigger(p);
					}
					if (triggerUp && p.Weapon)
					{
						p.Weapon.ReleaseTrigger(p);
					}
					if (moving && p.canMove)
					{
						p.Move(xAxis, yAxis);
					}
					if (p.canRotate)
					{
						if (c.controllers.hasMouse)
						{
							Vector2 pvp = shakeable.shakyCamera.WorldToScreenPoint(p.transform.position);
							Vector2 mouse = c.controllers.Mouse.screenPosition;
							Vector3 direction = new Vector3(mouse.x - pvp.x, 0, mouse.y - pvp.y);

							if (direction.magnitude > 0f)
							{
								p.transform.LookAt(direction, Vector3.up);
							}
						}
						else
						{
							var lookXAxis = c.GetAxis(5);
							var lookYAxis = c.GetAxis(6);
							var direction = new Vector3(lookXAxis, 0f, lookYAxis);

							if (direction.magnitude > 0f)
							{
								p.transform.LookAt(direction, Vector3.up);
							}
						}
					}
				}
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

			graph.UpdateBar(i, colorScheme.playerColors[i], normalizedScale);
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
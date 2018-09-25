using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	public enum GameState { Countdown, Live, Victory };

	[SerializeField] Player PlayerPrefab;
	[SerializeField] PlayerHUD PlayerHUDPrefab;

	[SerializeField] ColorScheme colorScheme;
	[SerializeField] Shakeable shakeable;
	[SerializeField] Graph graph;
	[SerializeField] UI ui;
	[SerializeField] Canvas screenSpaceUICanvas;
	[SerializeField] AudioSource BackgroundMusic;
	[SerializeField] AbstractWeapon[] WeaponPrefabs;

	public float RespawnTimer = 3f;
	public float CountdownDuration = 3f;

	GameObject[] spawnPoints;
	PlayerState[] playerStates;
	PlayerHUDManager playerHUDManager;
	GameState state = GameState.Countdown;
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
		var canMove = false;
		var canRotate = false;
		var canShoot = false;

		if (state == GameState.Countdown)
		{
			canMove = true;
			canRotate = true;
			canShoot = false;
			remainingCountdownDuration -= Time.deltaTime;
			ui.countdownNumber.text = Mathf.CeilToInt(remainingCountdownDuration).ToString();
			if (remainingCountdownDuration <= 0f)
			{
				state = GameState.Live;
				ui.animator.SetTrigger("Close");
			}
		}
		else if (state == GameState.Live)
		{
			canMove = true;
			canRotate = true;
			canShoot = true;
		}
		else if (state == GameState.Victory)
		{
			canMove = false;
			canRotate = false;
			canShoot = false;

			// check for Fire button presses to load next map
			for (var i = 0; i < playerStates.Length; i++)
			{
				var c = playerStates[i].playerController;

				if (c.GetButtonDown("Fire"))
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}

		for (var i = 0; i < playerStates.Length; i++)
		{
			var c = playerStates[i].playerController;
			var p = playerStates[i].player;
			var triggerDown = c.GetButton("Fire") || c.GetButtonRepeating("Fire");
			var triggerUp = c.GetButtonUp("Fire");
			var xAxis = c.GetAxis(0);
			var yAxis = c.GetAxis(1);
			var moving = xAxis != 0f || yAxis != 0f;

			//added for roll / dash
			var rollTrue = Input.GetKey("r");

			if (canShoot && triggerDown && p.Weapon)
			{
				p.Weapon.PullTrigger(p);
			}
			if (canShoot && triggerUp && p.Weapon)
			{
				p.Weapon.ReleaseTrigger(p);
			}
			if (canMove && moving && p.canMove)
			{
				p.Move(xAxis, yAxis);
			}
			if (canRotate && p.canRotate)
			{

				Vector3 direction = new Vector3();
				if (c.controllers.hasMouse)
				{
					Vector2 pvp = shakeable.shakyCamera.WorldToScreenPoint(p.transform.position);
					Vector2 mouse = c.controllers.Mouse.screenPosition;

					direction = new Vector3(mouse.x - pvp.x, 0, mouse.y - pvp.y);
					if (direction.magnitude > 0f)
					{
						p.transform.LookAt(direction, Vector3.up);
					}
				}
				else
				{
					var lookXAxis = c.GetAxis(5);
					var lookYAxis = c.GetAxis(6);

					direction = new Vector3(lookXAxis, 0f, lookYAxis);
					if (direction.magnitude > 0f)
					{
						p.transform.LookAt(direction, Vector3.up);
					}
				}

				//added for roll/dash
				if(rollTrue && !p.isRolling){
					//p.RollInDirection(xAxis, yAxis);
				}

				if(rollTrue){
					p.Dash();
				}
			}
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
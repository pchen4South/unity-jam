using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

public class PlayerUI: Object{
	public PlayerHUD HUD {get;set;}
	public PlayerStatusUI PSUI {get;set;}
}

[System.Serializable]
public class PlayerHUDManager : Object
{
	PlayerUI[] playerUIPool;

	public PlayerHUDManager(PlayerHUD PlayerHUDPrefab, PlayerStatusUI PlayerStatusUIPrefab, int maximumSize)
	{
		playerUIPool = new PlayerUI[maximumSize];

		for (var i = 0; i < playerUIPool.Length; i++)
		{
			playerUIPool[i] = new PlayerUI();
			var HUD = Instantiate(PlayerHUDPrefab);
			var PSUI = Instantiate(PlayerStatusUIPrefab);
			playerUIPool[i].HUD = HUD;
			playerUIPool[i].PSUI = PSUI;
		}
	}

	void OnDestroy()
	{
		for (var i = 0; i < playerUIPool.Length; i++)
		{
			Destroy(playerUIPool[i]);
		}
	}

	public void UpdatePlayerHUDs(PlayerState[] playerStates, AbstractWeapon[] WeaponPrefabs, Camera camera, RectTransform parent, RectTransform bottomUIContainer )
	{
		var i = 0;

		while (i < playerStates.Length)
		{

			playerUIPool[i].HUD.gameObject.SetActive(true);
			playerUIPool[i].HUD.transform.SetParent(parent, false);
			playerUIPool[i].HUD.UpdateHealth(playerStates[i].player.Health, playerStates[i].player.MaxHealth);
			playerUIPool[i].HUD.UpdatePosition(camera, parent, playerStates[i].player.transform.position);
			playerUIPool[i].HUD.UpdateWeaponText(playerStates[i].player.Weapon.WeaponName);
			playerUIPool[i].HUD.UpdateAmmoCount(playerStates[i].player.Weapon.AmmoCount);
			
			playerUIPool[i].PSUI.gameObject.SetActive(true);
			playerUIPool[i].PSUI.transform.SetParent(bottomUIContainer.transform, false);
			playerUIPool[i].PSUI.UpdateWeaponType(playerStates[i].player.Weapon.WeaponName, (playerStates[i].player.Weapon.MagazineSize));
			playerUIPool[i].PSUI.UpdateAmmoCount(playerStates[i].player.Weapon.AmmoCount);
			playerUIPool[i].PSUI.UpdateHealth(playerStates[i].player.Health, playerStates[i].player.MaxHealth);
			playerUIPool[i].PSUI.UpdatePlayerIdentity(playerStates[i].player.PlayerNumber, playerStates[i].player.meshRenderer.material.color);
			playerUIPool[i].PSUI.UpdateWeaponProgress(playerStates[i].weaponIndex, WeaponPrefabs);
			playerUIPool[i].PSUI.UpdateDashCooldown(playerStates[i].player.MoveSkillCooldown, playerStates[i].player.MoveSkillTimer);

			i++;
		}
		while (i < playerUIPool.Length)
		{
			playerUIPool[i].HUD.gameObject.SetActive(false);
			playerUIPool[i].PSUI.gameObject.SetActive(false);
			i++;
		}
	}
}

public class GameMode : MonoBehaviour 
{
	public enum GameState { Countdown, Live, Victory };

	[SerializeField] Player PlayerPrefab;
	[SerializeField] PlayerHUD PlayerHUDPrefab;
	[SerializeField] PlayerStatusUI PlayerStatusUIPrefab;
	[SerializeField] RectTransform PlayerUIArea;

	[SerializeField] ColorScheme colorScheme;
	[SerializeField] Shakeable shakeable;
	[SerializeField] Graph graph;
	[SerializeField] UI ui;
	[SerializeField] Canvas screenSpaceUICanvas;
	[SerializeField] AudioSource BackgroundMusic;
	[SerializeField] AudioSource CountdownAudio;
	[SerializeField] AbstractWeapon[] WeaponPrefabs;
	[SerializeField] GameObject WinCamSpawn;
	[SerializeField] WinningPlayer WinningPlayerModel;
	[SerializeField] Text LeaderboardLabel;
	[SerializeField] Text PlayerNumbers;
	[SerializeField] Text GuncountText;


	public float RespawnTimer = 3f;
	public float CountdownDuration = 3f;

	GameObject[] spawnPoints;
	
	PlayerState[] playerStates;
	PlayerHUDManager playerHUDManager;
	GameState state = GameState.Countdown;
	int winningPlayerIndex;
	float remainingCountdownDuration;
	bool CountdownStarted = false;

	List<string> Leaders = new List<string>();
	int leadingLevel = 0;
	int maxLevels = 0;

	// TODO: I like the idea of not using Start for this but making an explicit method?
	void Start()
	{
		var playerCount = 4;

		remainingCountdownDuration = CountdownDuration;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		
		//make a copy of spawnPoints in a List so that we can assign unique starting spawns
		List<GameObject> spawnPointsCopy = new List<GameObject>();
		foreach(var sp in spawnPoints){
			spawnPointsCopy.Add(sp);
		}


		playerStates = new PlayerState[playerCount];
		for (var i = 0; i < playerCount; i++)
		{
			// choose a spawn point from the copied list, then remove it
			var spawnpoint = Random.Range(0, spawnPointsCopy.Count);
			var sp = spawnPointsCopy[spawnpoint];
			spawnPointsCopy.RemoveAt(spawnpoint);
			
			//spawn a player and PlayerState, initialize Player's values
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

			//initialize leaderboard
			Leaders.Add("P" + (i + 1).ToString() + " ");
			leadingLevel = 1;
		}

		LeaderboardLabel.text = "Current Leaders";
		string leaders = "";
		maxLevels = WeaponPrefabs.Length;
		Leaders.ForEach(s => leaders += s);
		PlayerNumbers.text = leaders.Trim();
		GuncountText.text = "1 / " + maxLevels.ToString();

		playerHUDManager = new PlayerHUDManager(PlayerHUDPrefab, PlayerStatusUIPrefab, 8);
	}

	void Update()
	{
		var canMove = false;
		var canRotate = false;
		var canShoot = false;

		if (state == GameState.Countdown)
		{
			if(!CountdownStarted){
				CountdownStarted = true;
				CountdownAudio.Play();
			}

			canMove = true;
			canRotate = true;
			canShoot = false;
			remainingCountdownDuration -= Time.deltaTime;
			ui.countdownNumber.text = Mathf.CeilToInt(remainingCountdownDuration).ToString();
			if (remainingCountdownDuration <= 0f)
			{
				CountdownStarted = false;
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
			var dashTrue = c.GetButtonDown("Dash");
			
			if (canShoot && triggerDown && p.Weapon)
			{
				p.Weapon.PullTrigger(p);
			}
			if (canShoot && triggerUp && p.Weapon)
			{
				p.Weapon.ReleaseTrigger(p);
			}
			if (canMove && moving && p.canMove && (Mathf.Abs(xAxis) >= .05f || Mathf.Abs(yAxis) >= .05f))
			{
				p.Move(xAxis, yAxis);
			}
			if (canRotate && p.canRotate)
			{

				Vector3 direction = new Vector3();
				//if (c.controllers.hasMouse)
				if (false)
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
					var angle = Mathf.Atan2(lookXAxis,lookYAxis) * Mathf.Rad2Deg;
                	var lookrot = Quaternion.Euler(0, angle, 0);

					if (lookXAxis != 0.0f || lookYAxis != 0.0f)
					{
						p.transform.rotation = lookrot;
					}
					else if (lookXAxis < .01f && lookYAxis < .01f){
						var input = new Vector3(xAxis, 0, yAxis);

						if(input != Vector3.zero) p.transform.forward = input.normalized;
            		}
				}

				if(dashTrue){
					var dashDir = new Vector3(xAxis, 0, yAxis);
					p.Dash(dashDir);
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
		playerHUDManager.UpdatePlayerHUDs(playerStates, WeaponPrefabs, shakeable.shakyCamera, screenSpaceUICanvas.transform as RectTransform, PlayerUIArea.transform as RectTransform);
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
			StartCoroutine(HandleVictory(killerPlayerState.player));
		}
		else
		{
			killerPlayerState.weaponIndex++;
			killerPlayerState.player.SetWeapon(WeaponPrefabs[killerPlayerState.weaponIndex]);

			if(killerPlayerState.weaponIndex + 1 == leadingLevel){
				LeaderboardLabel.text = "Current Leaders";
			} else if (killerPlayerState.weaponIndex + 1 > leadingLevel){
				LeaderboardLabel.text = "Current Leader";
				Leaders = new List<string>();
			}
			GuncountText.text = (killerPlayerState.weaponIndex+1).ToString() + " / " + maxLevels.ToString();
			string leaders = "";
			Leaders.Add("P" + (killerIndex+1).ToString());
			Leaders.ForEach(s => leaders += s + " ");
			PlayerNumbers.text = leaders.Trim();
			StartCoroutine(RespawnAfter(killedPlayerState, RespawnTimer));
		}
	}

	IEnumerator HandleVictory(Player winningPlayer){
			yield return new WaitForSeconds(2f);
			BackgroundMusic.Stop();
			winningPlayerIndex = winningPlayer.PlayerNumber;
			state = GameState.Victory;

			winningPlayer.SetAsVictor();
			WinningPlayerModel.StartWinSequence(winningPlayer);

			ui.countdownNumber.fontSize = 50;
			ui.countdownNumber.text = "\n\n\nPlayer " + (winningPlayerIndex + 1).ToString() + " Wins!";
			ui.animator.SetTrigger("Open");

			Color color = new Color();
			color.a = 0;
			ui.PanelImage.color = color;

			shakeable.transform.position = WinCamSpawn.transform.position;
			shakeable.transform.rotation = WinCamSpawn.transform.rotation;
	}


	IEnumerator RespawnAfter(PlayerState ps, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		ps.player.Spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
	}
}

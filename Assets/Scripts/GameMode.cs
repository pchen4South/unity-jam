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

public class PlayerUI: Object
{
	public PlayerHUD HUD { get; set; }
	public PlayerStatusUI PSUI { get; set; }
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
			playerUIPool[i].HUD = Instantiate(PlayerHUDPrefab);
			playerUIPool[i].PSUI = Instantiate(PlayerStatusUIPrefab);
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
			playerUIPool[i].PSUI.UpdatePlayerIdentity(playerStates[i].player.ID, playerStates[i].player.meshRenderer.material.color);
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
	[SerializeField] UI ui;
	[SerializeField] Canvas screenSpaceUICanvas;
	[SerializeField] AudioSource BackgroundMusic;
	[SerializeField] AudioSource CountdownAudio;
	[SerializeField] AbstractWeapon[] WeaponPrefabs;
	[SerializeField] Minigame[] MinigamePrefabs;
	[SerializeField] GameObject WinCamSpawn;
	[SerializeField] WinningPlayer WinningPlayerModel;
	[SerializeField] Text LeaderboardLabel;
	[SerializeField] Text PlayerNumbers;
	[SerializeField] Text GuncountText;

	public float RespawnTimer = 3f;
	public float CountdownDuration = 3f;
	public float killHeight = -1000f;

	GameObject[] spawnPoints;
	PlayerState[] playerStates;
	PlayerHUDManager playerHUDManager;
	GameState state = GameState.Countdown;
	Minigame Minigame;
	int winningPlayerIndex;
	float remainingCountdownDuration;
	bool CountdownStarted = false;
	float GameTimer = 0;

	//testing var
	bool didSpawnMinigame = false;

	//hit processing
	List<ValidHit> NewHits = new List<ValidHit>();
	List<ValidHit> HitsToBeProcessed = new List<ValidHit>();
	List<ValidHit> ProcessedHits = new List<ValidHit>();
	public void AddValidHit(ValidHit NewHit){
		HitsToBeProcessed.Add(NewHit);
	}
	
	void ProcessNewValidHits(){
		//make a copy of the Hits array or it will complain
		var HitArrayCopy = new List<ValidHit>(HitsToBeProcessed);

		if( HitsToBeProcessed.Count == 0) return;
		foreach(var Newhit in HitArrayCopy){
			//player originated damage
			if(Newhit.OriginatingEntityType == "PLAYERCHARACTER"){
				int shooterPlayerNumber = Newhit.OriginatingEntityIdentifier;
				//if player caused the hit on a player
				if(Newhit.VictimEntityType == "PLAYERCHARACTER"){
					var target = Newhit.VictimEntity;
					// Debug.Log("target: " + target.ID);
					Newhit.VictimEntity.OnDamage.Invoke(shooterPlayerNumber, target.ID, Newhit.DamageAmount);				
				} 
				//if player caused the hit on a NPC
				else if(Newhit.VictimEntityType == "NPC"){

				}
			//npc originated damage
			} else if(Newhit.OriginatingEntityType == "NPC"){

			}
			// all other sources of damage
			else {

			}

			ProcessedHits.Add(Newhit);
			HitsToBeProcessed.Remove(Newhit);
		}
	}

	void Start()
	{
		var playerCount = 4;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		remainingCountdownDuration = CountdownDuration;
		playerStates = new PlayerState[playerCount];
		playerHUDManager = new PlayerHUDManager(PlayerHUDPrefab, PlayerStatusUIPrefab, 8);

		for (var i = 0; i < playerCount; i++)
		{
			//spawn a player and PlayerState, initialize Player's values
			var spawnpoint = spawnPoints[i % spawnPoints.Length];
			var player = Instantiate(PlayerPrefab);
			var ps = new PlayerState(player, ReInput.players.GetPlayer(i));
			var WeaponPrefab = WeaponPrefabs[ps.weaponIndex];

			//ps.player.PlayerNumber = i;
			ps.player.ID = i;
			ps.player.name = "Player " + i;
			ps.player.SetWeapon(WeaponPrefab);
			ps.player.OnDamage = HandlePVPDamage;
			ps.player.SetColor(colorScheme.playerColors[i]);
			ps.player.Spawn(spawnpoint.transform);
			playerStates[i] = ps;

			//added valid hit event registration here
			ps.player.Weapon.OnValidHitOccurred += AddValidHit;
		}
	}

	void Update()
	{
		if (state == GameState.Countdown)
		{
			if(!CountdownStarted)
			{
				CountdownStarted = true;
				CountdownAudio.Play();
			}

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
			GameTimer += Time.deltaTime;

			ProcessNewValidHits();

			//Debug.Log("processed: " + ProcessedHits.Count);

			if (Minigame)
			{
				if (Minigame.MinigameIsRunning()){
					for (var i = 0; i < playerStates.Length; i++)
					{
						Minigame.HandleMove(playerStates[i]);
						Minigame.HandleDash(playerStates[i]);
						Minigame.HandleRotate(playerStates[i]);
						Minigame.HandleFire(playerStates[i]);
					}
				} else if (Minigame.MinigameResultsReady()){
					var MinigameResults = Minigame.Results;
					//DoSomeShitWithTheResults();
					Debug.Log("did some shit with the minigame results");
					Minigame.SetMinigameToReady();
					Destroy(Minigame);
				}
			}
			else
			{
				// Temp code for testing
				// if(GameTimer >= 5f && didSpawnMinigame == false){ 
				// 	Minigame = Instantiate(MinigamePrefabs[0]);
				// 	Minigame.BeginMinigame();
				// 	didSpawnMinigame = true;
				// }
				// These are the "default" behaviors when no minigames are present
				for (var i = 0; i < playerStates.Length; i++)
				{
					InputHelpers.BasicMove(playerStates[i]);
					InputHelpers.BasicDash(playerStates[i]);
					InputHelpers.BasicRotate(playerStates[i]);
					InputHelpers.BasicPullTrigger(playerStates[i]);
					InputHelpers.BasicReleaseTrigger(playerStates[i]);
				}
			}

			// kill players that have fallen off the map
			for (var i = 0; i < playerStates.Length; i++)
			{
				var notDead = !playerStates[i].player.IsDead();
				var belowKillHeight = playerStates[i].player.transform.position.y < killHeight;

				if (notDead && belowKillHeight)
				{
					playerStates[i].player.KillByFalling();
					StartCoroutine(RespawnAfter(playerStates[i], RespawnTimer));
				}
			}

			// calculate current top level
			var topLevel = 1;
			for (var i = 0; i < playerStates.Length; i++)
			{
				topLevel = Mathf.Max(topLevel, playerStates[i].weaponIndex + 1);
			}

			// find all current leaders
			var leaders = "";
			for (var i = 0; i < playerStates.Length; i++)
			{
				leaders += playerStates[i].weaponIndex == topLevel 
					? "P" + i + ", "
					: "";
			}

			LeaderboardLabel.text = "Current Leaders";
			GuncountText.text = topLevel + " / " + WeaponPrefabs.Length;
			PlayerNumbers.text = leaders;
			playerHUDManager.UpdatePlayerHUDs(playerStates, WeaponPrefabs, shakeable.shakyCamera, screenSpaceUICanvas.transform as RectTransform, PlayerUIArea.transform as RectTransform);
		}
		else if (state == GameState.Victory)
		{
			for (var i = 0; i < playerStates.Length; i++)
			{
				if (playerStates[i].playerController.GetButtonDown("Fire"))
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}

		// always push timescale back towards full-speed
		Time.timeScale += (1 - Time.timeScale) * .1f * Time.timeScale;
	}


	void HandlePVPDamage(int attackerIndex, int victimIndex, int damageAmount)
	{
		// TODO: This is an event that probably should be handled by a minigame.. consider how to do this
		var validVictim = victimIndex >= 0 && victimIndex < playerStates.Length;
		var validAttacker = victimIndex >= 0 && victimIndex < playerStates.Length;

		if (!validVictim)
			return;

		var victim = playerStates[victimIndex];
		var victimShouldDie = damageAmount >= victim.player.Health && !victim.player.IsDead();

		if (victimShouldDie)
		{
			Time.timeScale = .1f;
			victim.player.Kill();
			shakeable.AddIntensity(1f);
			StartCoroutine(RespawnAfter(victim, RespawnTimer));

			if (validAttacker)
			{
				var attacker = playerStates[attackerIndex];
				var attackerShouldWin = attacker.weaponIndex >= WeaponPrefabs.Length - 1;
				
				if (attackerShouldWin)
				{
					StartCoroutine(HandleVictory(attacker.player));
				}
				else
				{
					// Debug.Log("weaponindex increased from" + attacker.weaponIndex);
					attacker.weaponIndex++;
					// Debug.Log("weaponindex increased to" + attacker.weaponIndex);
					attacker.player.SetWeapon(WeaponPrefabs[attacker.weaponIndex]);
					//added valid hit event registration here
					attacker.player.Weapon.OnValidHitOccurred += AddValidHit;
				}
			}
		}
		else
		{
			victim.player.Damage(damageAmount);
			shakeable.AddIntensity(.3f);
		}
	}

	IEnumerator HandleVictory(Player winningPlayer)
	{
		yield return new WaitForSeconds(2f);
		BackgroundMusic.Stop();
		winningPlayerIndex = winningPlayer.ID;
		state = GameState.Victory;

		winningPlayer.SetAsVictor();
		WinningPlayerModel.StartWinSequence(winningPlayer);

		ui.countdownNumber.fontSize = 50;
		ui.countdownNumber.text = "\n\n\nPlayer " + (winningPlayerIndex + 1).ToString() + " Wins!";
		ui.animator.SetTrigger("Open");
		ui.PanelImage.color = new Color(0, 0, 0, 0);

		shakeable.transform.position = WinCamSpawn.transform.position;
		shakeable.transform.rotation = WinCamSpawn.transform.rotation;
	}

	IEnumerator RespawnAfter(PlayerState ps, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		ps.player.Spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
	}
}
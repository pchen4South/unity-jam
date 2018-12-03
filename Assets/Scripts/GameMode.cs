using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

[System.Serializable]
public class PlayerState
{
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
	public PlayerHUD HUD;
	public PlayerStatusUI PSUI;
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

	public void UpdatePlayerHUDs(PlayerState[] playerStates, AbstractWeapon[] WeaponPrefabs, Camera camera, RectTransform parent, RectTransform bottomUIContainer)
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
			playerUIPool[i].HUD.UpdatePlayerReloadIndicator(playerStates[i].player.Weapon.ReloadProgressPercent());
			
			playerUIPool[i].PSUI.gameObject.SetActive(true);
			playerUIPool[i].PSUI.transform.SetParent(bottomUIContainer.transform, false);
			playerUIPool[i].PSUI.UpdateWeaponType(playerStates[i].player.Weapon.WeaponName, (playerStates[i].player.Weapon.MagazineSize));
			playerUIPool[i].PSUI.UpdateAmmoCount(playerStates[i].player.Weapon.AmmoCount);
			playerUIPool[i].PSUI.UpdateHealth(playerStates[i].player.Health, playerStates[i].player.MaxHealth);
			playerUIPool[i].PSUI.UpdatePlayerIdentity(playerStates[i].player.ID, playerStates[i].player.meshRenderer.material.color);
			playerUIPool[i].PSUI.UpdateWeaponProgress(playerStates[i].player.weaponIndex, WeaponPrefabs);
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

	public void DisableUI(PlayerState[] playerStates)
	{
		var i = 0;

		while (i < playerStates.Length)
		{
			playerUIPool[i].HUD.gameObject.SetActive(false);
			playerUIPool[i].PSUI.gameObject.SetActive(false);
			i++;
		}
	}
}

public class GameMode : MonoBehaviour 
{
	[Header("Runtime-instantiated Prefabs")]
	public Player PlayerPrefab;
	public PlayerHUD PlayerHUDPrefab;
	public PlayerStatusUI PlayerStatusUIPrefab;
	public RectTransform PlayerUIArea;
	public FloatingText PopupTextPrefab;


	[Header("Art Configuration")]
	public ColorScheme colorScheme;
	public AudioSource BackgroundMusic;
	public AudioSource CountdownAudio;
	public WinningPlayer WinningPlayerModel;


	[Header("Game Configuration")]
	[Range(1, 4)]
	public int playerCount = 4;
	public float MatchDuration = 600f;
	public float CountdownDuration = 3f;
	public float RespawnDuration = 3f;
	public float killHeight = -1000f;
	public AbstractWeapon[] WeaponPrefabs;
	public AbstractMinigame[] MinigamePrefabs;


	[Header("Game Objects")]
	public GameObject WinCamSpawn;
	public Shakeable shakeable;
	public UI ui;
	public Canvas screenSpaceUICanvas;
	public GameObject LeaderboardPanel;
	public Text LeaderboardLabel;
	public Text PlayerNumbers;
	public Text GuncountText;
	public Text ClockText;
	public Text TimelineIndicator;
	public RectTransform textParent;
	public GameObject[] spawnPoints;


	[Header("State")]
	public MatchState matchState;
	// TODO: This should be created statically with the prefab heirarchy... no need for the pooling thing anymore
	// TODO: should be moved up among the prefabs 
	public PlayerHUDManager playerHUDManager;
	public List<ValidHit> HitsToBeProcessed = new List<ValidHit>(512);

	void Start()
	{
		playerHUDManager = new PlayerHUDManager(PlayerHUDPrefab, PlayerStatusUIPrefab, 8);
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		matchState = new MatchState();
		matchState.playerStates = new PlayerState[playerCount];
		matchState.matchStatus = MatchStatus.Active;
		matchState.countdownTimeRemaining = CountdownDuration;
		matchState.matchTimeRemaining = MatchDuration;
		for (var i = 0; i < playerCount; i++)
		{
			var spawnpoint = spawnPoints[i % spawnPoints.Length];
			var player = Instantiate(PlayerPrefab);
			var ps = new PlayerState(player, ReInput.players.GetPlayer(i));
			var WeaponPrefab = WeaponPrefabs[ps.player.weaponIndex];

			ps.player.ID = i;
			ps.player.name = "Player " + i;
			ps.player.Spawn(spawnpoint.transform);
			ps.player.SetWeapon(WeaponPrefab);
			ps.player.SetColor(colorScheme.playerColors[i]);
			ps.player.OnValidHitOccurred = HitsToBeProcessed.Add;
			matchState.playerStates[i] = ps;
		}

		matchState.minigame = MinigamePrefabs.Length > 0 ? Instantiate(MinigamePrefabs[0]) : null;
		if (matchState.minigame)
		{
			matchState.minigame.gameMode = this;
			matchState.minigame.BeginMinigame(matchState.playerStates);
		}
	}

	void Update()
	{
		var dt = Time.deltaTime;

		UpdateGameTime(dt);
		ProcessNewValidHits();

		// -------- Process Input ---------
		for (var i = 0; i < matchState.playerStates.Length; i++)
		{
			var ps = matchState.playerStates[i];

			InputHelpers.BasicMove(ps);
			InputHelpers.BasicRotate(ps);
			InputHelpers.BasicDash(ps);
			InputHelpers.BasicPullTrigger(ps);
			InputHelpers.BasicReleaseTrigger(ps);
		}

		// TODO: this is written naively and won't fairly handle ties...
		// check if any player has won
		for (var i = 0; i < matchState.playerStates.Length; i++)
		{
			var ps = matchState.playerStates[i];

			// GAME OVER!!!!!!!!!!!!!!!
			if (ps.player.weaponIndex >= WeaponPrefabs.Length)
			{
				BackgroundMusic.Stop();
				matchState.matchStatus = MatchStatus.Victory;

				ps.player.SetAsVictor();
				WinningPlayerModel.StartWinSequence(ps.player);

				ui.countdownNumber.fontSize = 50;
				ui.countdownNumber.text = "\n\n\nPlayer " + (ps.player.ID + 1).ToString() + " Wins!";
				ui.animator.SetTrigger("Open");
				ui.PanelImage.color = new Color(0, 0, 0, 0);

				shakeable.transform.position = WinCamSpawn.transform.position;
				shakeable.transform.rotation = WinCamSpawn.transform.rotation;
				break;
			}
		}

		// kill players that have fallen off the map
		for (var i = 0; i < matchState.playerStates.Length; i++)
		{
			var ps = matchState.playerStates[i];
			var notDead = ps.player.status != CharacterStatus.Dead;
			var belowKillHeight = ps.player.transform.position.y < killHeight;

			if (notDead && belowKillHeight)
			{
				ps.player.Damage(1000000);
				ps.player.fallDeathSound.Play();
			}
		}

		// respawn eligible dead players
		for (var i = 0; i < matchState.playerStates.Length; i++)
		{
			var ps = matchState.playerStates[i];
			var isDead = ps.player.status == CharacterStatus.Dead;

			// not a dead player
			if (!isDead)
				continue;

			ps.player.respawnTimeRemaining -= dt;

			var eligibleForRespawn = ps.player.respawnTimeRemaining <= 0f;

			// still not eligible to respawn
			if (!eligibleForRespawn)
				continue;
			
			// respawn this player if appropriate
			ps.player.Spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
		}
			
		// ------- UI stuff ----------
		UpdateGameClock(matchState.matchTimeRemaining);

		// calculate current top level
		LeaderboardPanel.SetActive(true);
		var topLevel = 1;
		for (var i = 0; i < matchState.playerStates.Length; i++)
		{
			topLevel = Mathf.Max(topLevel, matchState.playerStates[i].player.weaponIndex + 1);
		}

		// find all current leaders
		var leaders = "";
		for (var i = 0; i < matchState.playerStates.Length; i++)
		{
			leaders += (matchState.playerStates[i].player.weaponIndex + 1) == topLevel 
				? "P" + (i + 1) + ", "
				: "";
		}
		LeaderboardLabel.text = "Current Leaders";
		GuncountText.text = topLevel + " / " + WeaponPrefabs.Length;

		leaders = leaders.TrimEnd(' ');
		leaders = leaders.TrimEnd(',');
		PlayerNumbers.text = leaders;

		playerHUDManager.UpdatePlayerHUDs(
			matchState.playerStates, 
			WeaponPrefabs, 
			shakeable.shakyCamera, 
			screenSpaceUICanvas.transform as RectTransform, 
			PlayerUIArea.transform as RectTransform);
	}

	void UpdateGameTime(float dt)
	{
		if (matchState.matchStatus == MatchStatus.Active)
		{
			matchState.matchTimeRemaining -= dt;
			Time.timeScale += (1 - Time.timeScale) * .1f * Time.timeScale;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	void UpdateGameClock(float remainingTime)
	{
		int gameElapsedSeconds = Mathf.RoundToInt(remainingTime);
		int minutesLeft = gameElapsedSeconds / 60;
		int secondsLeft = gameElapsedSeconds % 60;

		ClockText.gameObject.SetActive(true);
		ClockText.text = minutesLeft.ToString("00") + ":" + secondsLeft.ToString("00");
		TimelineIndicator.rectTransform.anchoredPosition = new Vector2(1600 * gameElapsedSeconds / MatchDuration, 0);
	}

	void ProcessNewValidHits()
	{
		foreach(var h in HitsToBeProcessed)
		{
			h.victim.Damage(h.damageAmount);
			h.victim.HitCounter.Add(new HitCounter(h.attacker.ID, h.damageAmount));

			if (h.victim.status == CharacterStatus.Dead)
			{
				// Player attacks Player
				if (h.attacker is Player attacker)
				{
					if (h.victim is Player victim)
					{
						attacker.weaponIndex++;
						if (attacker.weaponIndex < WeaponPrefabs.Length)
						{
							attacker.SetWeapon(WeaponPrefabs[attacker.weaponIndex]);
						}
						if (victim.status == CharacterStatus.Dead)
						{
							victim.respawnTimeRemaining = RespawnDuration;	
						}
					}
				}
				shakeable.AddIntensity(1f);
				Time.timeScale = .1f;
			}
			else 
			{
				shakeable.AddIntensity(.3f);
			}
		}
		HitsToBeProcessed.Clear();
	}
}
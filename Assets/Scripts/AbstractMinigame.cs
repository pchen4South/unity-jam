using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePlayer
{
    public int PlayerNumber = 0;
    public int MinigamePlacing = 0;
    public int TotalScoreEarned = 0;
    public MinigamePlayer(int playerNumber)
    {
        PlayerNumber = playerNumber;
    }   
}

public class MinigameResults
{
    static int SortByScore(MinigamePlayer p1, MinigamePlayer p2)
    {
        return p2.TotalScoreEarned.CompareTo(p1.TotalScoreEarned);
    }

    public List<MinigamePlayer> MinigamePlayersArray = new List<MinigamePlayer>();
    public void CalculatePlayerPlacement()
    {
        var placeCounter = 1;

        MinigamePlayersArray.Sort(SortByScore);
        for(var i = 0; i < MinigamePlayersArray.Count; i++)
        {
            //Account for ties
            if(i > 0)
            {
                if(MinigamePlayersArray[i].TotalScoreEarned == MinigamePlayersArray[i-1].TotalScoreEarned)
                    placeCounter--;
            }
            MinigamePlayersArray[i].MinigamePlacing = placeCounter;
            placeCounter++;
        }
    }
}

public enum MG_State { Ready, Intro, Running, ResultsReady, Destroy}

public abstract class AbstractMinigame : MonoBehaviour
{
    public bool hasIntro = false;
    public bool hasSummary = false;
    public float introDuration = 10f;
    public float runningDuration = 60f;
    public float summaryDuration = 3f;    

    public Canvas MinigameCanvas;
    public RectTransform MinigameIntro;
    public RectTransform MinigameResultsScreen;
    public int WinnersPrize = 2;
    public int LosersPenalty = 2;    
    public RectTransform PlayerResultsLine;
    public MG_State MinigameState = MG_State.Ready;
    public MinigameResults Results = new MinigameResults();
    public GameMode gameMode;

    public virtual void Update()
    {
        switch (MinigameState)
        {
            case MG_State.Intro:
                introDuration -= Time.deltaTime;
                MinigameIntro.gameObject.SetActive(true);

                if (introDuration <= 0)
                {
                    MinigameIntro.gameObject.SetActive(false);
                    RunMinigame();
                }
            break;

            case MG_State.Running:
                runningDuration -= Time.deltaTime;

                if (runningDuration <= 0)
                {
                    HandleMinigameCompleted();
                    TabulateResults();
                    MinigameState = MG_State.ResultsReady;
                }
            break;

            case MG_State.ResultsReady:
                if (hasSummary)
                {
                    summaryDuration -= Time.deltaTime;
                    ShowResultsScreen();

                    if (summaryDuration <= 0)
                    {
                        MinigameState = MG_State.Destroy;
                        MinigameResultsScreen.gameObject.SetActive(false);
                    }
                } 
                else 
                {
                    MinigameState = MG_State.Destroy;
                    gameMode = null;
                    Destroy(gameObject);
                }
            break;
        }
    }

    public virtual void RunMinigame()
    {
        PrepareMinigameObjects();
        MinigameState = MG_State.Running;
    }

    public virtual void BeginMinigame(PlayerState[] playerstates)
    {
        for (var i = 0; i < playerstates.Length; i++)
        {
            var mg_player = new MinigamePlayer(playerstates[i].player.ID);

            Results.MinigamePlayersArray.Add(mg_player);
        }
        introDuration = hasIntro ? introDuration : 0;
        MinigameState = MG_State.Intro;
    }

    public virtual void HandleMove(PlayerState p) {
        InputHelpers.BasicMove(p);
    }

    public virtual void HandleDash(PlayerState p) {
        InputHelpers.BasicDash(p);
    }

    public virtual void HandleRotate(PlayerState p) {
        InputHelpers.BasicRotate(p);
    }

    public virtual void HandleFire(PlayerState p) {
        InputHelpers.BasicPullTrigger(p);
        InputHelpers.BasicReleaseTrigger(p);
    }

    public virtual void HandleMinigameCompleted() {}

    public virtual void TabulateResults() {}

    public virtual void PrepareMinigameObjects() {}

    public virtual void ShowResultsScreen() 
    {
        if(!hasSummary) 
            return;

        MinigameResultsScreen.gameObject.SetActive(true);
        var mgPlayers = Results.MinigamePlayersArray;
        var posYIncrement = 50;    

        for (int i = 0; i < mgPlayers.Count; i++)
        {
            var line_transform = Instantiate(PlayerResultsLine);
            var result_line_script = line_transform.GetComponent<PlayerMinigameResult>();
            
            line_transform.SetParent(MinigameResultsScreen.transform, false);
            line_transform.anchoredPosition = new Vector2(PlayerResultsLine.rect.x, - posYIncrement * i);

            result_line_script.Playername.text = "P" + (1 + mgPlayers[i].PlayerNumber).ToString();
            result_line_script.Playerscore.text = mgPlayers[i].TotalScoreEarned.ToString();

            if (mgPlayers[i].MinigamePlacing == 1)
            {
                result_line_script.Playerprize.text = "+" + WinnersPrize.ToString();
            } 
            else 
            {
                result_line_script.Playerprize.text = "-" + LosersPenalty.ToString();
            }   
        }
    }
}
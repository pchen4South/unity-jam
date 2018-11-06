using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePlayer{
    public int PlayerNumber = 0;
    public int MinigamePlacing = 0;
    public int TotalScoreEarned = 0;

    public MinigamePlayer(int playerNumber){
        PlayerNumber = playerNumber;
    }
    
}

public class MinigameResults{
    public List<MinigamePlayer> MinigamePlayersArray = new List<MinigamePlayer>();

    static int SortByScore(MinigamePlayer p1, MinigamePlayer p2)
     {
         return p2.TotalScoreEarned.CompareTo(p1.TotalScoreEarned);
     }
    public void CalculatePlayerPlacement(){
        MinigamePlayersArray.Sort(SortByScore);
        var placeCounter = 1;
        for(var i = 0; i < MinigamePlayersArray.Count; i++ ){
            //Account for ties
            if(i > 0){
                if(MinigamePlayersArray[i].TotalScoreEarned == MinigamePlayersArray[i-1].TotalScoreEarned)
                    placeCounter--;
            }
            MinigamePlayersArray[i].MinigamePlacing = placeCounter;
            placeCounter++;
        }
    }
}


public abstract class Minigame : MonoBehaviour
{
    // Ready = ready to run minigame, running = game is running, ended = game is done but needs to calculate results / effects, 
    // ResultsReady = Ready for main loop to consume results and set the minigame to MG_State.Ready
    enum MG_State { Ready, Intro, Running, Ended, ResultsReady}
    [SerializeField] public float MinigameDuration = 5f;
    [SerializeField] public float MinigameAliveTimer = 0f;
    
    [SerializeField] public Canvas MinigameIntroScreen;
    [SerializeField] public List<AbstractCharacter> NPCS = new List<AbstractCharacter>();

    MG_State MinigameState = MG_State.Ready;

    public MinigameResults Results = new MinigameResults();

    public void BeginMinigame(PlayerState[] playerstates){
        if(MinigameState == MG_State.Ready){
            for (var i = 0; i < playerstates.Length; i++){
                var mg_player = new MinigamePlayer(playerstates[i].player.ID);
                Results.MinigamePlayersArray.Add(mg_player);
            }
            PrepareMinigame();
            MinigameState = MG_State.Running;
            MinigameAliveTimer = 0f;
        }
    }

    void Update(){
        switch(MinigameState){
            case MG_State.Running:
                MinigameAliveTimer += Time.deltaTime;
                    if(MinigameAliveTimer >= MinigameDuration){
                        MinigameState = MG_State.Ended;
                        HandleMinigameCompleted();
                    }
                break;
            case MG_State.Ended:
                // I think Minigame will stay in Ended state until Gamemode consumes the results and sets Minigame back to Ready externally
                TabulateResults();
                break;
            default:
                break;
        }
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
    public virtual void HandlePlayerDamage(PlayerState attacker, PlayerState victim) {}
    public virtual void HandleMinigameCompleted(){}
    public virtual void TabulateResults(){}
    public virtual void PrepareMinigame() {}

    public void SetMinigameToReady(){ MinigameState = MG_State.Ready; }
    public void SetMinigameToResultsReady(){ MinigameState = MG_State.ResultsReady; }
    public bool MinigameIsRunning(){ return MinigameState == MG_State.Running; }
    public bool MinigameResultsReady(){ return MinigameState == MG_State.ResultsReady; }


}
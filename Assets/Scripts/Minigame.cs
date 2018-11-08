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
    enum MG_State { Ready, Intro, Running, Ended, ResultsReady, Destroy}
    [SerializeField] public float MinigameDuration = 5f;
    float MinigameAliveTimer = 0f;
    [SerializeField] public float MinigameSummaryLengthSeconds = 5f;    
    float MinigameSummaryTimer = 0f;
    [SerializeField] public Canvas MinigameCanvas;
    [SerializeField] public RectTransform MinigameIntro;
    [SerializeField] RectTransform MinigameResultsScreen;

    [SerializeField] public List<AbstractCharacter> NPCS = new List<AbstractCharacter>();
    [SerializeField] public int WinnersPrize = 2;
    [SerializeField] public int LosersPenalty = 2;    
    [SerializeField] RectTransform PlayerResultsLine;
    [SerializeField] public string MinigameName = "";
    [SerializeField] Text MinigameNameText;
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
            case MG_State.ResultsReady:
                MinigameSummaryTimer += Time.deltaTime;
                ShowResultsScreen();
                if(MinigameSummaryTimer >= MinigameSummaryLengthSeconds){
                    Debug.Log("should destroy");
                    MinigameState = MG_State.Destroy;
                }
                break;
            case MG_State.Destroy:
                Debug.Log("Destroy me");
                break;
            default:
                break;
        }
    }

    //By Default the minigame uses the default controller handling, can override these to set custom behavior
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
    public virtual void ShowResultsScreen() {
        MinigameResultsScreen.gameObject.SetActive(true);
        ResetLines();
        var mgPlayers = Results.MinigamePlayersArray;
        int posYIncrement = 50;    

        for(int i = 0; i < mgPlayers.Count; i++){
            var line_transform = Instantiate(PlayerResultsLine);
            var result_line_script = line_transform.GetComponent<PlayerMinigameResult>();
            
            line_transform.SetParent(MinigameResultsScreen.transform, false);
            line_transform.anchoredPosition = new Vector2(PlayerResultsLine.rect.x, - posYIncrement * i);
            result_line_script.Playername.text = "P" + (1 + mgPlayers[i].PlayerNumber).ToString();
            result_line_script.Playerscore.text = mgPlayers[i].TotalScoreEarned.ToString();
            if(mgPlayers[i].MinigamePlacing == 1){
                result_line_script.Playerprize.text = "+" + WinnersPrize.ToString();
            } else {
                result_line_script.Playerprize.text = "-" + LosersPenalty.ToString();
            }   
        }
    }

    void ResetLines(){
        var lines = GameObject.FindGameObjectsWithTag("UI_Resettable");
        foreach (var item in lines)
        {   
            Destroy(item);
        }
    }

    public void SetMinigameToReady(){ MinigameState = MG_State.Ready; }
    public void SetMinigameToResultsReady(){ MinigameState = MG_State.ResultsReady; }
    public bool MinigameIsRunning(){ return MinigameState == MG_State.Running; }
    public bool MinigameResultsReady(){ return MinigameState == MG_State.ResultsReady; }
    public bool MinigameShouldDestroy(){return MinigameState == MG_State.Destroy;}


}
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

public abstract class AbstractMinigame : MonoBehaviour
{
    // Ready = ready to run minigame, running = game is running, ended = game is done but needs to calculate results / effects, 
    // ResultsReady = Ready for main loop to consume results and set the minigame to MG_State.Ready
    enum MG_State { Ready, Intro, Running, Ended, ResultsReady, Destroy}
    [SerializeField] public bool hasIntro = false;
    [SerializeField] public bool hasSummary = false;
    [SerializeField] public float MinigameDuration = 5f;
    public float MinigameAliveTimer = 0f;
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
    [SerializeField] public GameObject[] StageElementsToModify;
    MG_State MinigameState = MG_State.Ready;
    public float IntroTimer = 10f;
    public MinigameResults Results = new MinigameResults();

    public virtual void ShowIntro(){
        MinigameIntro.gameObject.SetActive(true);
        StartCoroutine(Introscreen());
    }
        
    IEnumerator Introscreen(){
        yield return new WaitForSeconds(IntroTimer);
        MinigameIntro.gameObject.SetActive(false);
        PrepareMinigameObjects();
    }


    public void BeginMinigame(PlayerState[] playerstates){
        if(MinigameState == MG_State.Ready){
            for (var i = 0; i < playerstates.Length; i++){
                var mg_player = new MinigamePlayer(playerstates[i].player.ID);
                Results.MinigamePlayersArray.Add(mg_player);
            }
            if(hasIntro)
                ShowIntro();
            RunMinigame();
        }
    }


    public virtual void Update(){
        switch(MinigameState){
            case MG_State.Running:
                MinigameAliveTimer += Time.deltaTime;
                    if(MinigameAliveTimer >= MinigameDuration){
                        MinigameState = MG_State.Ended;
                        HandleMinigameCompleted();
                    }
                break;
            case MG_State.Ended:
                TabulateResults();
                break;
            case MG_State.ResultsReady:
                if(hasSummary){
                    MinigameSummaryTimer += Time.deltaTime;
                    ShowResultsScreen();
                    if(MinigameSummaryTimer >= MinigameSummaryLengthSeconds){
                        MinigameState = MG_State.Destroy;
                    }
                } else {
                    MinigameState = MG_State.Destroy;
                }
                break;
            case MG_State.Destroy:
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
    
    public virtual void RunMinigame(){
        MinigameState = MG_State.Running;
        MinigameAliveTimer = 0f;
    }
    public virtual void HandleMinigameCompleted(){}
    public virtual void TabulateResults(){
        SetMinigameToResultsReady();
    }
    public virtual void PrepareMinigameObjects(){ }
    public virtual void ShowResultsScreen() {
        if(!hasSummary) return;

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

    public void SetMinigameToReady(){ 
        if(MinigameState != MG_State.Ready) MinigameState = MG_State.Ready; 
    }
    public void SetMinigameToResultsReady(){ 
        if(MinigameState != MG_State.ResultsReady) MinigameState = MG_State.ResultsReady; 
    }
    public void SetMinigameToRunning(){ 
        if(MinigameState != MG_State.Running) MinigameState = MG_State.Running; 
    }
    public bool MinigameIsRunning(){ return MinigameState == MG_State.Running; }
    public bool MinigameResultsReady(){ return MinigameState == MG_State.ResultsReady; }
    public bool MinigameShouldDestroy(){return MinigameState == MG_State.Destroy;}


}
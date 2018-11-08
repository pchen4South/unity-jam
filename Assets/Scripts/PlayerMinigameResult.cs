using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMinigameResult : MonoBehaviour
{
    public Text Playername; 
    public Text Playerscore;
    public Text Playerprize;

    //could use these if we want to keep this object around for later
    [Header("State")]
    public int player_number = 0;
    public int player_score = 0;
    public int player_prize = 0;

    public PlayerMinigameResult(int playerNum, int score, int prize){
        var playername = "P" + (playerNum + 1).ToString();
        Playername.text = playername;
        player_number = playerNum;
        Playerscore.text = score.ToString();
        player_score = score;
        Playerprize.text = prize.ToString();
        player_prize = prize;
    }
}

using UnityEngine;

public enum MatchStatus { Intro, Countdown, Active, Victory, Outro }

[System.Serializable]
public class MatchState
{
    public MatchStatus matchStatus;
    public AbstractMinigame minigame;
    public float matchTimeRemaining;
    public float countdownTimeRemaining;
    public PlayerState[] playerStates;
}
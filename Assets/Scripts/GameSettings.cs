using UnityEngine;

public class GameSettings : MonoBehaviour 
{
	[Header("Audio")]
	public bool PlayBackgroundMusic = true;
	[Range(0f, 1f)]
	public float BackgroundMusicVolume = .6f;
	public Color[] playerColors = new Color[8];
}
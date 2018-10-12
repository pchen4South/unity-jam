using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour 
{
	[SerializeField] Text startText;
	[SerializeField] SpriteRenderer TitleText;
	public Rewired.Player playerController;
	public Gradient TitleColor;
	private bool IsFlashing = false;
	public float textFlashTime = .25f;
	public float buildingFadeInTime = 2f;

	private Image Buildings;
	private float titleRunTime = 0f;

    public float strobeDuration = 2f;
	
	void Start() 
	{
		Buildings = gameObject.transform.Find("Buildings").GetComponent<Image>();
		Buildings.color = new Color(1,1,1,0);
	}
	
	// Update is called once per frame
	void Update() 
	{
		titleRunTime += Time.deltaTime;

		if (titleRunTime <= buildingFadeInTime)
		{
			Buildings.color = new Color(1,1,1, titleRunTime / buildingFadeInTime);
		}

		if (!IsFlashing)
		{
            StartCoroutine(FlashTextOff());       
        }

		float t = Mathf.PingPong(Time.time / strobeDuration, 1f);
        TitleText.color = TitleColor.Evaluate(t);


		//poll players controllers for start button
		var players = ReInput.players;
		for(int i = 0; i <= players.playerCount - 1 ; i++){
			var player = players.GetPlayer(i);
			if(player.controllers.joystickCount > 0){
				var startPressed = player.GetButtonDown("Start");
				if(startPressed){
					SceneManager.LoadScene("StageSelect");
				}
			}
		}
		
		//for mouse/keyboard
		var AnyKeyOrMouseClick = Input.anyKeyDown;
		if(AnyKeyOrMouseClick){
			SceneManager.LoadScene("StageSelect");
		}
	}

    IEnumerator FlashTextOff()
	{
		IsFlashing = true;
        startText.enabled = true;
        yield return new WaitForSeconds(textFlashTime);
        StartCoroutine(FlashTextOn());

    }
     IEnumerator FlashTextOn()
	 {
        startText.enabled = false;
        yield return new WaitForSeconds(textFlashTime);
        IsFlashing = false;
     }
}
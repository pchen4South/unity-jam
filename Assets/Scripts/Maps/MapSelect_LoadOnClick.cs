using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;

public class MapSelect_LoadOnClick : MonoBehaviour
{
    public GameObject LoadingScreen;
	[SerializeField] AbstractMap[] Maps;
    [SerializeField] Button UpArrow;
    [SerializeField] Button DownArrow;
    [SerializeField] Button OverlayButton;
    public Image CurrentMapImage;
    public Text CurrentMapLabel;
    AbstractMap currentMap;
    int mapIndex = 0;
    // need a timer to delay inputs or else it will scroll thru all the choices instantly
    [SerializeField] float inputDelayTimer = .25f;
    float inputDelayCounter = 0;
    
    public void LoadScene(string MapName)
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene(MapName);
    }

    void Start()
    {
        ChangeMap();
    }

    void Update()
    {
        if(inputDelayCounter > 0){
            inputDelayCounter -= Time.deltaTime;
        } else {
            inputDelayCounter = 0;
        }


        if(mapIndex == 0)
        {
            UpArrow.interactable = false;
            DownArrow.interactable = true;
        } 
        else if (mapIndex == Maps.Length - 1)
        {
            UpArrow.interactable = true;
            DownArrow.interactable = false;
        } 
        else 
        {
            UpArrow.interactable = true;
            DownArrow.interactable = true;
        }

        //poll players controllers 
		var players = ReInput.players;
		for(int i = 0; i <= players.playerCount - 1 ; i++){
			var player = players.GetPlayer(i);

        
			if(player.controllers.joystickCount > 0 && inputDelayCounter == 0){   
                 Debug.Log(player.GetAxis(0) + " " + player.GetAxis(1));
                if(InputHelpers.MenuDownOrRight(player)){
                    DownArrowClicked();
                    inputDelayCounter = inputDelayTimer;
                }
                if(InputHelpers.MenuUpOrLeft(player)){
                    UpArrowClicked();
                    inputDelayCounter = inputDelayTimer;
                }
                if(InputHelpers.MenuAccept(player)){
                    MapClick();
                }
			}
		}
    }

    public void UpArrowClicked()
    {
        if (mapIndex == 0) return;
        mapIndex --;
        ChangeMap();
    }

    public void DownArrowClicked()
    {
        if (mapIndex == Maps.Length -1) return;
        mapIndex++;
        ChangeMap();
    }

    void ChangeMap()
    {
        currentMap = Maps[mapIndex];
        CurrentMapImage.sprite  = currentMap.MapScreenshot.sprite;
        CurrentMapLabel.text = currentMap.MapName.text;
    }

    public void MapClick()
    {
        LoadScene(CurrentMapLabel.text);
    }
}
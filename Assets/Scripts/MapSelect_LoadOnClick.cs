using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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



    public void LoadScene(string MapName){
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene(MapName);
    }
    void Start(){
        ChangeMap();
    }
    void Update(){
        if(mapIndex == 0){
            UpArrow.interactable = false;
            DownArrow.interactable = true;
        } else if (mapIndex == Maps.Length - 1){
            UpArrow.interactable = true;
            DownArrow.interactable = false;
        } else {
            UpArrow.interactable = true;
            DownArrow.interactable = true;
        }
    }

    public void UpArrowClicked(){
        if (mapIndex == 0) return;
        mapIndex --;
        ChangeMap();
    }

    public void DownArrowClicked(){
        if (mapIndex == Maps.Length -1) return;
        mapIndex++;
        ChangeMap();
    }

    void ChangeMap(){
        currentMap = Maps[mapIndex];
        CurrentMapImage.sprite  = currentMap.MapScreenshot.sprite;
        CurrentMapLabel.text = currentMap.MapName.text;
    }

    public void MapClick(){
        LoadScene(CurrentMapLabel.text);
    }


}

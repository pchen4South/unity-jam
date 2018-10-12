using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect_LoadOnClick : MonoBehaviour
{
    public GameObject LoadingScreen;
    public void LoadScene(string MapName){
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene(MapName);
    }
}

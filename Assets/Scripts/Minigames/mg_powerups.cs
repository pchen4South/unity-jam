using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_powerups : AbstractMinigame
{
    //temp for testing
    [SerializeField] GameObject powerup;
    [SerializeField] GameObject[] ItemSpawnLocations;
    public int NumSpawns = 4;

    public override void RunMinigame(){
        SetMinigameToRunning();
        MinigameAliveTimer = 0f;

        ItemSpawnLocations = GameObject.FindGameObjectsWithTag("ItemSpawn");
        
        for(int i = 0; i < NumSpawns; i++){
            Instantiate(powerup, ItemSpawnLocations[Random.Range(0, ItemSpawnLocations.Length)].transform.position, Quaternion.identity);
        }
    }
}

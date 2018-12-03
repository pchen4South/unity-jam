using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_powerups : AbstractMinigame
{
    //temp for testing
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject[] ItemSpawnLocations;
    public int NumSpawns = 4;

    public override void RunMinigame()
    {
        base.RunMinigame();

        ItemSpawnLocations = GameObject.FindGameObjectsWithTag("ItemSpawn");
        List<GameObject> spawns = new List<GameObject>(ItemSpawnLocations);

        for(int i = 0; i < NumSpawns; i++){
            var spawnindex = Random.Range(0, spawns.Count);

            Instantiate(powerups[Random.Range(0, powerups.Length)], 
                            spawns[spawnindex].transform.position, 
                            Quaternion.identity);
            spawns.RemoveAt(spawnindex);
        }
    }
}

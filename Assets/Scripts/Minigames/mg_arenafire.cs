using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_arenafire : AbstractMinigame
{
    GameObject LeftWall;
    GameObject RightWall;
    GameObject BackWall;
    GameObject FrontWall;

    [SerializeField] int FirePerWave = 5;
    [SerializeField] int FireSpawnTimer = 5;
    [SerializeField] GameObject Fire  = null;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> verticesCopy;
    private bool waveSpawned = false;

    public override void Update()
    {
        base.Update();
        if(Mathf.RoundToInt(MinigameAliveTimer) % FireSpawnTimer == 0 && MinigameAliveTimer > 0 && !waveSpawned){
            waveSpawned = true;
            for(int i = 0; i < FirePerWave; i++){
                if(verticesCopy.Count >= 0){
                    int index = Random.Range(0, verticesCopy.Count);
                    Instantiate(Fire, verticesCopy[index], Quaternion.identity);
                    verticesCopy.RemoveAt(index);
                }
            }
        }
        if(Mathf.RoundToInt(MinigameAliveTimer) % FireSpawnTimer != 0)
            waveSpawned = false;
    }

	private void OnDrawGizmos () {
        if (vertices == null) {
			return;
		}

		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Count; i++) {
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}
	}
    public override void RunMinigame(){
        LeftWall = GameObject.Find("LeftWall");
        RightWall = GameObject.Find("RightWall");
        BackWall = GameObject.Find("BackWall");
        FrontWall = GameObject.Find("FrontWall");

        var xSize = RightWall.transform.position.x - LeftWall.transform.position.x;
        var zSize = BackWall.transform.position.z - FrontWall.transform.position.z;
        		
        int xs = Mathf.RoundToInt(xSize);
        int zs = Mathf.RoundToInt(zSize);

        Generate(xs, zs);
        verticesCopy = new List<Vector3>(vertices);
        SetMinigameToRunning();
        MinigameAliveTimer = 0f;
    }

	private void Generate (int xs, int zs) {
        int xStart = (int)LeftWall.transform.position.x + (int)LeftWall.GetComponent<MeshRenderer>().bounds.size.x + 4;
        int xEnd =(int)RightWall.transform.position.x - (int)RightWall.GetComponent<MeshRenderer>().bounds.size.x;
        int zStart = (int)FrontWall.transform.position.z + (int)FrontWall.GetComponent<MeshRenderer>().bounds.size.z;
        int zEnd = (int)BackWall.transform.position.z - (int)BackWall.GetComponent<MeshRenderer>().bounds.size.z;

		for (int i = 0, z = zStart; z <= zEnd; z = z + 5) {
			for (int x = xStart; x <= xEnd; x = x + 5, i++) {
				vertices.Add(new Vector3(x, 1, z));
			}
		}
	}
}

using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public float counter = 0f;


	void Update()
	{
		counter += Time.deltaTime;
	}
}
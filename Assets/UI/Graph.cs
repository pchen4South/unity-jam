using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour 
{
	[SerializeField]
	GraphBar[] graphBars;

	public void UpdateBar(int barIndex, Color color, float normalizedScale)
	{
		graphBars[barIndex].backgroundImage.color = color;
		graphBars[barIndex].transform.localScale = new Vector3(normalizedScale, 1, 1);
	}
}
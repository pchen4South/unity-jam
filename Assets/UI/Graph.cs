using UnityEngine;

public class Graph : MonoBehaviour 
{
	[SerializeField]
	GraphBar[] graphBars;

	public void UpdateBar(int barIndex, Color color, float normalizedLength)
	{
		graphBars[barIndex].UpdateBar(color, normalizedLength);
	}
}
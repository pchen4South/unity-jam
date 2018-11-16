using UnityEngine;
using UnityEngine.UI;

public class GraphBar : MonoBehaviour 
{
	public RawImage backgroundImage;
	public RectTransform rectTransform;

	public void UpdateBar(Color color, float normalizedLength)
	{
		backgroundImage.color = color;	
		backgroundImage.transform.localScale = new Vector3(normalizedLength, 1, 1);
	}
}
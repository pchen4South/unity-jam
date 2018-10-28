using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractMap : MonoBehaviour
{
	public Image MapScreenshot;
	public Text MapName;

	public string GetMapName()
	{
		if(MapName != null)
			return MapName.text;
		else	
			return "placeholder";
	}
}
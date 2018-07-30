using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour 
{
	public Text startText;
	private bool IsFlashing = false;
	public float textFlashTime = .25f;
	public float buildingFadeInTime = 2f;

	private Image Buildings;
	private float titleRunTime = 0f;

	void Start() 
	{
		Buildings = gameObject.transform.Find("Buildings").GetComponent<Image>();
		Buildings.color = new Color(1,1,1,0);
	}
	
	// Update is called once per frame
	void Update() 
	{
		titleRunTime += Time.deltaTime;

		if (titleRunTime <= buildingFadeInTime)
		{
			Buildings.color = new Color(1,1,1, titleRunTime / buildingFadeInTime);
		}

		if (!IsFlashing)
		{
            StartCoroutine(FlashTextOff());       
        }
	}

    IEnumerator FlashTextOff()
	{
		IsFlashing = true;
        startText.enabled = true;
        yield return new WaitForSeconds(textFlashTime);
        StartCoroutine(FlashTextOn());

    }
     IEnumerator FlashTextOn()
	 {
        startText.enabled = false;
        yield return new WaitForSeconds(textFlashTime);
        IsFlashing = false;
     }
}
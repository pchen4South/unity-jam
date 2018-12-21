using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour 
{
	[SerializeField]	RectTransform rectTransform  = null;
	[SerializeField]    RectTransform GreenHPBar = null;
	[SerializeField]    Slider ReloadIndicator = null;
	[SerializeField]    GameObject ReloadBar = null;

    public Image DashOuter;
    public Image DashInner;
	public Text Dashtext;
	public Text WeaponName;
	
	public Vector2 screenSpaceOffset = Vector2.zero;
	public Color healthyColor = Color.green;
	public Color deadColor = Color.red;

	public void UpdatePlayerIdentity(){}
	public void UpdatePlayerReloadIndicator(int reloadProgress){
		//not reloading
		if (reloadProgress == -1){
			ReloadBar.SetActive(false);
		//is reloading
		} else {
			ReloadBar.SetActive(true);
			ReloadIndicator.value = reloadProgress;
		}
	}

	public void UpdateHealth(int currentHealth, int maxHealth)
	{
		var normalizedHealth = (float) currentHealth / (float) maxHealth;
		var barColor = Color.Lerp(deadColor, healthyColor, normalizedHealth);

		GreenHPBar.sizeDelta = new Vector2(normalizedHealth * 100, GreenHPBar.rect.height);
	}

	//playerHUDPool[i].UpdatePosition(camera, parent, playerStates[i].player.transform.position);
	public void UpdatePosition(Camera cam, RectTransform parent, Vector3 worldPosition)
	{
		Vector2 position = cam.WorldToViewportPoint(worldPosition);
		position -= new Vector2(.5f, .5f);
		position *= parent.sizeDelta;
		position += screenSpaceOffset;

		rectTransform.anchoredPosition = position;
	}

    public void UpdateDashCooldown(float MaxCDTime, float CDTimer){
        float currentCDRatio = CDTimer != 0 ? CDTimer / MaxCDTime : 1;
        var height = DashOuter.rectTransform.rect.height;
        int InnerWidth = 0;
        InnerWidth = Mathf.RoundToInt(DashOuter.rectTransform.rect.width * currentCDRatio);        
        DashInner.rectTransform.sizeDelta = new Vector2(InnerWidth, height);
		if(CDTimer != 0) Dashtext.color = Color.red;
		else Dashtext.color = Color.white;
        //DashInner.rectTransform.anchoredPosition = new Vector2(InnerWidth / 2, 0);
    }
	public void UpdateWeaponText(string weaponName){
		WeaponName.text = weaponName;
		return;
	}
	public void UpdateAmmoCount(int currentAmmo){

		// bool showAmmo = currentAmmo >= 0 ? true : false;

		// if(showAmmo){
		// 	AmmoLabel.enabled = true;
		// 	AmmoCount.enabled = true;
		// 	AmmoCount.text = currentAmmo > 0 ? currentAmmo.ToString() : "Reloading";
		// }
		// else{
		// 	AmmoLabel.enabled = false;
		// 	AmmoCount.enabled = false;
		// }
		return;
	}
}
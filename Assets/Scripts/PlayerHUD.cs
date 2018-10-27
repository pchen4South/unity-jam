using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour 
{
	[SerializeField]	RectTransform rectTransform;
	//[SerializeField]	RectTransform reticle;
	[SerializeField]	GraphBar lifeBar;
	[SerializeField] 	Text WeaponName;
	[SerializeField] 	Text AmmoLabel;
	[SerializeField] 	Text AmmoCount;
	[SerializeField] 	Text PlayerNumber;

	

	public Vector2 screenSpaceOffset = Vector2.zero;
	public Color healthyColor = Color.green;
	public Color deadColor = Color.red;

	public void UpdatePlayerIdentity(){}

	public void UpdateHealth(int currentHealth, int maxHealth)
	{
		var normalizedHealth = (float) currentHealth / (float) maxHealth;
		var barColor = Color.Lerp(deadColor, healthyColor, normalizedHealth);

		lifeBar.UpdateBar(barColor, normalizedHealth);
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

	public void UpdateWeaponText(string weaponName){
		WeaponName.text = weaponName;
	}
	public void UpdateAmmoCount(int currentAmmo){

		bool showAmmo = currentAmmo >= 0 ? true : false;

		if(showAmmo){
			AmmoLabel.enabled = true;
			AmmoCount.enabled = true;
			AmmoCount.text = currentAmmo > 0 ? currentAmmo.ToString() : "Reloading";
		}
		else{
			AmmoLabel.enabled = false;
			AmmoCount.enabled = false;
		}
		
	}
}
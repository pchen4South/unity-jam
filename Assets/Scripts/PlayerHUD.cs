using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour 
{
	[SerializeField]	RectTransform rectTransform;
	[SerializeField]	RectTransform reticle;
	[SerializeField]	GraphBar lifeBar;
	[SerializeField] 	Text WeaponName;
	[SerializeField] 	Text AmmoLabel;
	[SerializeField] 	Text AmmoCount;

	

	public Vector2 screenSpaceOffset = Vector2.zero;
	public Color healthyColor = Color.green;
	public Color deadColor = Color.red;

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

		Vector2 position2 = worldPosition + new Vector3(0, .1f, 0);
		// position2 -= new Vector2(.5f, .5f);
		position2 *= parent.sizeDelta;
		position2 += screenSpaceOffset;

		reticle.anchoredPosition = position2;

		rectTransform.anchoredPosition = position;
	}

	public void UpdateReticlePosition(){

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
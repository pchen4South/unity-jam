using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{

    public Text WeaponName;
    public Text AmmoCount;
    public Text PlayerNumberText;
    public Image BoxBackground;

    public int P1_X_Position = 200;
    public int P2_X_Position = 500;
    public int P3_X_Position = 1100;
    public int P4_X_Position = 1400;


    int playerNumber = -1;


    void Update(){



        if (playerNumber == -1) return;
        switch(playerNumber){
            case 1:
                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(P1_X_Position, 0, 0);
                break;
            case 2:
                 gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(P2_X_Position, 0, 0);
                break;
            case 3:
                 gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(P3_X_Position, 0, 0);
                break;
            case 4:
                 gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(P4_X_Position, 0, 0);
                break;
            default:
                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(P4_X_Position, 0, 0);
                break;
        }
    }

    //public methods
	public void UpdateAmmoCount(int currentAmmo){

		bool showAmmo = currentAmmo >= 0 ? true : false;

		// if(showAmmo){
		// 	AmmoCount.enabled = true;
		// 	AmmoCount.text = currentAmmo > 0 ? currentAmmo.ToString() : "Reloading";
		// }
		// else{
		// 	AmmoCount.enabled = false;
		// }
        return;
	}

    public void UpdateWeaponType(string weaponName){
        UpdateWeaponText(weaponName);
        UpdateWeaponAmmoUI(weaponName);   
    }
	public void UpdateHealth(int currentHealth, int maxHealth)
	{
		var normalizedHealth = (float) currentHealth / (float) maxHealth;
		var barColor = Color.Lerp(Color.red, Color.green, normalizedHealth);

		//lifeBar.UpdateBar(barColor, normalizedHealth);
	}

    public void UpdatePlayerIdentity(int pNum, Color pColor){
        playerNumber = pNum + 1;
        PlayerNumberText.text = "P" + playerNumber.ToString();
        PlayerNumberText.color = pColor;
        
        var pColorA = pColor;
        pColorA.a = .4f;
        BoxBackground.color = pColorA;
        

    }

    //private methods
	void UpdateWeaponText(string weaponName){
		//WeaponName.text = weaponName;
        return;
	}
    void UpdateWeaponAmmoUI(string weaponName){
        //Updates the ammo icon and maximum count
        return;
    }

}

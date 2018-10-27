﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{

    public Text WeaponName;
    public Text AmmoCount;
    public Text PlayerNumberText;
    public Image BoxBackground;

    public Image ShotgunAmmo;
    public Image OtherAmmo;
    public Image Infinity;

    public Image DashOuter;
    public Image DashInner;

    public Image HPOuter;
    public Image HPInner;

    public RectTransform AmmoLine1;
    public RectTransform AmmoLine2;
    public RectTransform AmmoLine3;

    public int P1_X_Position = 200;
    public int P2_X_Position = 500;
    public int P3_X_Position = 1100;
    public int P4_X_Position = 1400;

    string CurrentWeapon = "";
    int CurrentMaxAmmo = 0;
    int playerNumber = -1;

    List<Image> AmmoArray = new List<Image>();

    bool WeaponDidChange = false;

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
        if(currentAmmo == -1){
            Infinity.gameObject.SetActive(true);
        } else {
            Infinity.gameObject.SetActive(false);
           
           AmmoArray.ForEach(p=>p.gameObject.SetActive(false));
           for(int i = 0; i < currentAmmo; i++){
               AmmoArray[i].gameObject.SetActive(true);
           }
        }
        return;
	}

    public void UpdateWeaponType(string weaponName, int magSize){
        if(CurrentWeapon != weaponName){
            CleanupAmmoIcons();
            CurrentMaxAmmo = magSize;
            UpdateWeaponText(weaponName);
            UpdateWeaponAmmoUI(weaponName); 
        }  
    }
	public void UpdateHealth(int currentHealth, int maxHealth)
	{
		var normalizedHealth = (float) currentHealth / (float) maxHealth;		        
        var height = HPOuter.rectTransform.rect.height;
        int InnerWidth = 0;
        InnerWidth = Mathf.RoundToInt(HPOuter.rectTransform.rect.width * normalizedHealth);        
        HPInner.rectTransform.sizeDelta = new Vector2(InnerWidth, height);
        HPInner.rectTransform.anchoredPosition = new Vector2(InnerWidth / 2, 0);
	}

    public void UpdatePlayerIdentity(int pNum, Color pColor){
        playerNumber = pNum + 1;
        PlayerNumberText.text = "P" + playerNumber.ToString();
        PlayerNumberText.color = pColor;
        
        var pColorA = pColor;
        pColorA.a = .4f;
        BoxBackground.color = pColorA;
    }

    public void UpdateDashCooldown(float MaxCDTime, float CDTimer){
        float currentCDRatio = CDTimer != 0 ? CDTimer / MaxCDTime : 1;
        var height = DashOuter.rectTransform.rect.height;
        int InnerWidth = 0;
        InnerWidth = Mathf.RoundToInt(DashOuter.rectTransform.rect.width * currentCDRatio);        
        DashInner.rectTransform.sizeDelta = new Vector2(InnerWidth, height);
        DashInner.rectTransform.anchoredPosition = new Vector2(InnerWidth / 2, 0);
    }

    public void UpdateWeaponProgress(int WeaponIndex, AbstractWeapon[] WeaponPrefabs){

    }

    //private methods
	void UpdateWeaponText(string weaponName){
		WeaponName.text = weaponName;
        CurrentWeapon = weaponName;
        return;
	}

    void CleanupAmmoIcons(){
        foreach ( var a in AmmoArray){
            Destroy(a);
        }
        AmmoArray = new List<Image>();
    }

    void UpdateWeaponAmmoUI(string weaponName){
        Image AmmoType;
        if(weaponName == "Shotgun"){
            AmmoType = ShotgunAmmo;
        } else {
            AmmoType = OtherAmmo;
        }

        var x_counter = 16;
        var initialX = 8;

        for(int i = 0; i < CurrentMaxAmmo; i++){
            var ammo = Instantiate(AmmoType);
            if(i < 10 ){
                ammo.transform.SetParent(AmmoLine1);
                ammo.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialX + x_counter * (i), -10);
            } else if(i >= 10 && i < 20){
                ammo.transform.SetParent(AmmoLine2);
                ammo.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialX + x_counter * (i-10), -10);
            } else{
                ammo.transform.SetParent(AmmoLine3);
                ammo.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialX + x_counter * (i-20), -10);
            }
            AmmoArray.Add(ammo);
        }
        
        return;
    }

}

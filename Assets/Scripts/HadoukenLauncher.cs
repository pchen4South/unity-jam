using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HadoukenLauncher : AbstractWeapon {

    enum WeaponState { Ready, Charging, Firing };
    [Header("Cached references")]
    [SerializeField]
    AudioSource[] fireSound;

    [Header("Prefabs")]
    public Hadouken[] Ammo;
    public WeaponBar ChargeBar;

    [Header("Config")]
    public float TravelSpeed = 25f;
    public Color BarColor1 = Color.white;
    public Color BarColor2 = Color.yellow;
    public Color BarColor3 = Color.red;

    [Header("State")]
    private float nextFire = 0f;
    public float fireRate = 0.5f;
    WeaponState state = WeaponState.Ready;

    private float chargeTime = 0f;

    private WeaponBar BarInstance;

    void Start(){
        var player = this.player;
        if(ChargeBar != null){
            var bar = Instantiate(ChargeBar, player.transform.position + player.transform.up * 1.02f , player.transform.rotation, player.transform);
            bar.player = player;
            BarInstance = bar;
        }
    }

    public override void PullTrigger(Player player)
    {
        if (Time.time < nextFire)
            return;
        
        
        /*
        chargeTime += Time.deltaTime;
        var slider = BarInstance.GetComponentInChildren<Slider>();
        slider.value = 0.5f;
        */
        
        chargeTime += Time.deltaTime;
		

    }

    public override void ReleaseTrigger(Player player){
        var ChargeLevel = 0;
        var wep = player.Weapon;
        
        if (chargeTime >= 1f && chargeTime <2f)
            ChargeLevel = 1;
        else if (chargeTime >= 2f){
            ChargeLevel = 2;
        }
        fireSound[ChargeLevel].Play();

        if(ChargeLevel == 2)
            StartCoroutine(DelayFire(0.6f, ChargeLevel, wep, player));
        else
        {
        ShootFireball(ChargeLevel, wep, player);
        }
        
        nextFire = Time.time + fireRate;
        chargeTime = 0f;
        var slider = BarInstance.GetComponentInChildren<Slider>();
        slider.value = 0;
     
    
    }

    void Update()
    {
        var slider = BarInstance.GetComponentInChildren<Slider>();
        var img = slider.GetComponentInChildren<Image>();

        var percentCharged = chargeTime / 2.0f;
        slider.value = percentCharged;

        if(percentCharged < 0.5){
            img.color = Color.Lerp(BarColor1, BarColor2, (float)percentCharged / 0.5f);
        }
        else if (percentCharged > 0.5 && percentCharged < 1){
            img.color = Color.Lerp(BarColor2, BarColor3, (float)((percentCharged - 0.5f)/ 0.5f));
        } else if (percentCharged >= 1){
            img.color = BarColor3;
        }

        if(slider.value > 1)
            slider.value = 1;
    }

    IEnumerator DelayFire(float delayTime, int ChargeLevel, AbstractWeapon wep, Player player){
            yield return new WaitForSeconds(delayTime);
            ShootFireball(ChargeLevel, wep, player);
    }
    void ShootFireball(int ChargeLevel, AbstractWeapon wep, Player player){
            float forceMultiplier = 0;

            switch (ChargeLevel){
                case 0:
                    forceMultiplier = 1;
                    break;
                case 1:
                    forceMultiplier = 1.5f;
                    break;
                case 2:
                    forceMultiplier = 2;
                    break;
                default:
                    forceMultiplier = 1;
                    break;
            }


            var fireball = Instantiate(Ammo[ChargeLevel], wep.transform.position + wep.transform.forward * 1.02f , wep.transform.rotation);
            fireball.body.AddForce(fireball.transform.forward * TravelSpeed * forceMultiplier, ForceMode.Impulse);
            fireball.PlayerNumber = player.PlayerNumber;
            var slider = BarInstance.GetComponentInChildren<Slider>();
            slider.value = 0;
    }

}

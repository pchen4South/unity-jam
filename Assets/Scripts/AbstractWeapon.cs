using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour 
{
	public Transform Muzzle;
	public Player player;

	public string WeaponName = "";
	public float SpeedModifier = 1f;
	public int AmmoCount = 0;
	public int MagazineSize = 0;

	public Transform LeftHandIKTarget;
	public Transform RightHandIKTarget;

	public virtual void PullTrigger(Player player){}
	public virtual void ReleaseTrigger(Player player){}
}
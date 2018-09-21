using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour 
{
	public Transform Muzzle;
	public Player player;

	public float SpeedModifier = 1f;

	public virtual void PullTrigger(Player player){}
	public virtual void ReleaseTrigger(Player player){}
}
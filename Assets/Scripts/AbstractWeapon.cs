using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour 
{
	public Player player;
	//public virtual void Fire(Player player){}
	public virtual void PullTrigger(Player player){}
	public virtual void ReleaseTrigger(Player player){}
}
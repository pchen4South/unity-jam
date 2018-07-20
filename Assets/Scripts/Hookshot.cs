using UnityEngine;

public class Hookshot : AbstractWeapon
{
	[SerializeField]
	Hook hook;

	public override void PullTrigger(Player player)
	{
		player.canMove = true;
		player.canRotate = false;
	}

	public override void ReleaseTrigger(Player player)
	{
		player.canMove = false;
		player.canRotate = true;
	}

	public void Update()
	{

	}
}
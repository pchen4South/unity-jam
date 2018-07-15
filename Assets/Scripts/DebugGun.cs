using UnityEngine;

public class DebugGun : AbstractWeapon
{
	public override void Fire(PlayerScript player)
	{
		Debug.Log("BANG BANG");
	}
}
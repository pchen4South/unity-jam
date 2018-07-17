using UnityEngine;

public class WeaponHotkeys : MonoBehaviour 
{
	[SerializeField]
	GameMode game;

	void Update()
	{
		var weaponCount = game.WeaponPrefabs.Length;

		if (Input.GetKeyDown(KeyCode.Alpha1) && weaponCount > 0)
			game.players.ForEach(p => p.SetWeapon(game.WeaponPrefabs[0]));
		if (Input.GetKeyDown(KeyCode.Alpha2) && weaponCount > 1)
			game.players.ForEach(p => p.SetWeapon(game.WeaponPrefabs[1]));
		if (Input.GetKeyDown(KeyCode.Alpha3) && weaponCount > 2)
			game.players.ForEach(p => p.SetWeapon(game.WeaponPrefabs[2]));
		if (Input.GetKeyDown(KeyCode.Alpha4) && weaponCount > 3)
			game.players.ForEach(p => p.SetWeapon(game.WeaponPrefabs[3]));
		if (Input.GetKeyDown(KeyCode.Alpha5) && weaponCount > 4)
			game.players.ForEach(p => p.SetWeapon(game.WeaponPrefabs[4]));
	}
}
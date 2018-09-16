using UnityEngine;

public class WeaponHotkeys : MonoBehaviour 
{
	[SerializeField]
	Player player;
	[SerializeField]
	AbstractWeapon[] WeaponPrefabs;

	void Start()
	{
		player.SetWeapon(WeaponPrefabs[0]);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			player.SetWeapon(WeaponPrefabs[0]);
		if (Input.GetKeyDown(KeyCode.Alpha2))
			player.SetWeapon(WeaponPrefabs[1]);
		if (Input.GetKeyDown(KeyCode.Alpha3))
			player.SetWeapon(WeaponPrefabs[2]);
		if (Input.GetKeyDown(KeyCode.Alpha4))
			player.SetWeapon(WeaponPrefabs[3]);
		if (Input.GetKeyDown(KeyCode.Alpha5))
			player.SetWeapon(WeaponPrefabs[4]);
		if (Input.GetKeyDown(KeyCode.Alpha6))
			player.SetWeapon(WeaponPrefabs[5]);
		if (Input.GetKeyDown(KeyCode.Alpha7))
			player.SetWeapon(WeaponPrefabs[6]);
		if (Input.GetKeyDown(KeyCode.Alpha8))
			player.SetWeapon(WeaponPrefabs[7]);
		if (Input.GetKeyDown(KeyCode.Alpha9))
			player.SetWeapon(WeaponPrefabs[8]);
	}
}
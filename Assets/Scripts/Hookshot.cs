using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : AbstractWeapon
{
	enum WeaponState { Ready, Firing, Pulling };

	[SerializeField]
	Hook HookPrefab;

	public float FiringDuration = 2f;
	public float MoveSpeed = .04f;

	WeaponState state = WeaponState.Ready;
	WaitForFixedUpdate wait = new WaitForFixedUpdate();

	bool triggerDown = false;

	public override void PullTrigger(Player player)
	{
		triggerDown = true;
		if (state != WeaponState.Ready)
			return;

		RaycastHit rayHit = new RaycastHit();
		Ray ray = new Ray(transform.position + transform.forward * .5f, player.transform.forward);
		bool didHit = Physics.Raycast(ray, out rayHit, 100f);

		if (!didHit)
			return;

		var hook = Instantiate(HookPrefab, ray.origin, player.transform.rotation);
		
		hook.weapon = this;
		StartCoroutine(FireWeapon(hook, destination: rayHit.point));
	}

	public override void ReleaseTrigger(Player player)
	{
		triggerDown = false;
	}

	public IEnumerator FireWeapon(Hook hook, Vector3 destination)
	{
		Vector3 delta = hook.transform.position - destination;
		Vector3 origin = player.transform.position;
		float totalFlightTime = delta.magnitude / MoveSpeed;
		float remainingFlightTime = totalFlightTime;

		player.canMove = false;
		player.canRotate = false;
		state = WeaponState.Firing;
		while (remainingFlightTime > 0f)
		{
			hook.transform.position = Vector3.Lerp(destination, player.transform.position, remainingFlightTime / totalFlightTime);
			yield return wait;
			remainingFlightTime -= Time.fixedDeltaTime;
		}
		StartCoroutine(PullToTarget(hook, player.transform.position));
	}

	public IEnumerator PullToTarget(Hook hook, Vector3 origin)
	{
		state = WeaponState.Pulling;
		
		Vector3 delta = hook.transform.position - origin;
		float totalFlightTime = delta.magnitude / MoveSpeed;
		float remainingFlightTime = totalFlightTime;

		player.canMove = false;
		player.canRotate = false;
		state = WeaponState.Firing;
		while (remainingFlightTime > 0f)
		{
			player.transform.position = Vector3.Lerp(hook.transform.position, origin, remainingFlightTime / totalFlightTime);
			yield return wait;
			remainingFlightTime -= Time.fixedDeltaTime;
		}
		player.canMove = true;
		player.canRotate = true;
		state = WeaponState.Ready;
		Destroy(hook.gameObject);
	}
}
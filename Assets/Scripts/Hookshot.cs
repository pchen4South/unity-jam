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

	public override void PullTrigger(Player player)
	{
		if (state != WeaponState.Ready)
			return;

		var hook = Instantiate(HookPrefab, transform.position + transform.forward * .4f, player.transform.rotation);
		
		hook.weapon = this;
		StartCoroutine(FireWeapon(hook));
	}

	public IEnumerator FireWeapon(Hook hook)
	{
		float duration = FiringDuration;

		player.canMove = false;
		player.canRotate = false;
		state = WeaponState.Firing;
		while (duration > 0f && hook.isFree)
		{
			hook.transform.position += hook.transform.forward * MoveSpeed;
			yield return wait;
			duration -= Time.fixedDeltaTime;
		}
		StartCoroutine(PullToTarget(hook));
	}

	public IEnumerator PullToTarget(Hook hook)
	{
		float duration = 2f;

		state = WeaponState.Pulling;
		while (duration > 0f)
		{
			var delta = hook.transform.position - player.transform.position;

			if (delta.sqrMagnitude < 2f)
				break;

			player.controller.Move(delta.normalized * MoveSpeed);
			yield return wait;
			duration -= Time.fixedDeltaTime;
		}
		player.canMove = true;
		player.canRotate = true;
		state = WeaponState.Ready;
		Destroy(hook.gameObject);
	}
}
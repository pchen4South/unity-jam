using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : AbstractWeapon
{
	enum WeaponState { Ready, Firing, Pulling };

	[SerializeField]
	Hook HookPrefab;
	[SerializeField]
	AudioSource audioSource;

	public float MoveSpeed = 100f;

	WeaponState state = WeaponState.Ready;
	WaitForFixedUpdate wait = new WaitForFixedUpdate();

	public override void PullTrigger(Player player)
	{
		if (state != WeaponState.Ready)
			return;

		RaycastHit rayHit = new RaycastHit();
		Ray ray = new Ray(transform.position + transform.forward * .5f, player.transform.forward);
		bool didHit = Physics.Raycast(ray, out rayHit, 100f);

		if (!didHit)
			return;

		var hook = Instantiate(HookPrefab, ray.origin, player.transform.rotation);
		
		hook.weapon = this;
		StartCoroutine(FireWeapon(hook, rayHit.point, rayHit.collider));
	}

	public IEnumerator FireWeapon(Hook hook, Vector3 destination, Collider targetCollider)
	{
		Vector3 delta = hook.transform.position - destination;
		Vector3 origin = player.transform.position;
		float totalFlightTime = delta.magnitude / MoveSpeed;
		float remainingFlightTime = totalFlightTime;

		// immobilize the target if it's another player.
		if (targetCollider.CompareTag("Player"))
		{
			var targetPlayer = targetCollider.GetComponent<Player>();

			targetPlayer.canMove = false;
			targetPlayer.canRotate = false;
		}

		audioSource.Play();
		player.canMove = false;
		player.canRotate = false;
		state = WeaponState.Firing;
		while (remainingFlightTime > 0f)
		{
			hook.transform.position = Vector3.Lerp(destination, player.transform.position, remainingFlightTime / totalFlightTime);
			yield return wait;
			remainingFlightTime -= Time.fixedDeltaTime;
		}
		StartCoroutine(PullToTarget(hook, player.transform.position, targetCollider));
	}

	public IEnumerator PullToTarget(Hook hook, Vector3 origin, Collider targetCollider)
	{
		state = WeaponState.Pulling;
		
		Vector3 delta = hook.transform.position - origin;
		float totalFlightTime = delta.magnitude / MoveSpeed;
		float remainingFlightTime = totalFlightTime;

		hook.audioSource.Play();
		player.canMove = false;
		player.canRotate = false;
		state = WeaponState.Firing;
		while (remainingFlightTime > 0f)
		{
			player.transform.position = Vector3.Lerp(hook.transform.position, origin, remainingFlightTime / totalFlightTime);
			yield return wait;
			remainingFlightTime -= Time.fixedDeltaTime;
		}

		// kill the target if it's a player other than ourself... which it should be...
		if (targetCollider.CompareTag("Player"))
		{
			var targetPlayer = targetCollider.GetComponent<Player>();

			if (targetPlayer.PlayerNumber != player.PlayerNumber)
			{
				targetPlayer.Health = 0;
			}
		}

		audioSource.Stop();
		player.canMove = true;
		player.canRotate = true;
		state = WeaponState.Ready;
		Destroy(hook.gameObject);
	}
}
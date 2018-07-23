using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : AbstractWeapon
{
	enum WeaponState { Ready, Firing, Pulling };

	[SerializeField]
	Hook HookPrefab;
	[SerializeField]
	AudioSource chainAudio;
	[SerializeField]
	AudioSource hookAudio;

	public float MoveSpeed = 100f;
	public float chainVolume = .6f;
	public float hookVolume = .7f;

	WeaponState state = WeaponState.Ready;
	IEnumerator chainFadeOut;

	bool triggerUp = false;

	public override void PullTrigger(Player player)
	{
		if (!triggerUp || state != WeaponState.Ready)
			return;

		RaycastHit rayHit = new RaycastHit();
		Ray ray = new Ray(transform.position + transform.forward * .5f, player.transform.forward);
		bool didHit = Physics.Raycast(ray, out rayHit, 100f);

		if (!didHit)
			return;

		var hook = Instantiate(HookPrefab, ray.origin, player.transform.rotation);
		
		hook.weapon = this;
		triggerUp = false;
		StartCoroutine(FireWeapon(hook, rayHit.point, rayHit.collider));
	}

	public override void ReleaseTrigger(Player player)
	{
		// player must release the trigger for the hookshot to fire again
		triggerUp = true;
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

		// stop the fadeout on our chain sound if it's still active
		if (chainFadeOut != null)
		{
			StopCoroutine(chainFadeOut);
		}
		chainAudio.volume = chainVolume;
		chainAudio.Play();
		player.canMove = false;
		player.canRotate = false;
		state = WeaponState.Firing;

		while (remainingFlightTime > 0f)
		{
			hook.transform.position = Vector3.Lerp(destination, player.transform.position, remainingFlightTime / totalFlightTime);
			yield return null;
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

		hookAudio.volume = hookVolume;
		hookAudio.Play();
		player.canMove = false;
		player.canRotate = false;
		state = WeaponState.Firing;

		while (remainingFlightTime > 0f)
		{
			player.transform.position = Vector3.Lerp(hook.transform.position, origin, remainingFlightTime / totalFlightTime);
			yield return null;
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

		player.canMove = true;
		player.canRotate = true;
		state = WeaponState.Ready;
		chainFadeOut = chainAudio.FadeOutOver(.4f);
		StartCoroutine(chainFadeOut);
		Destroy(hook.gameObject);
	}
}
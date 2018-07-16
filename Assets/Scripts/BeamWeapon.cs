using UnityEngine;

public class BeamWeapon : AbstractWeapon 
{
	enum WeaponState { Ready, Charging, Firing };

	[SerializeField]
	LineRenderer lineRenderer;

	[Header("Properties")]
	public float MaxRange = 100f;
	public float KnockBackStrength = .8f;
	public float FullChargeTime = 1f;
	public float FiringDuration = 1f;

	[Header("Cached Variables")]
	Ray ray = new Ray();
	RaycastHit rayHit = new RaycastHit();

	[Header("State")]
	WeaponState state = WeaponState.Ready;
	float remainingChargeTime = 0f;
	float remainingFiringTime = 0f;
	float firingPower = 0f;

	public override void PullTrigger(Player p)
	{
		if (state == WeaponState.Ready)
		{
			state = WeaponState.Charging;
			remainingChargeTime = FullChargeTime;
		}
	}

	public override void ReleaseTrigger(Player p)
	{
		if (state == WeaponState.Charging)
		{
			state = WeaponState.Firing;
			remainingFiringTime = FiringDuration;
			firingPower = (FullChargeTime - Mathf.Max(remainingChargeTime, 0)) / FullChargeTime;
		}
	}

	void Update()
	{
		switch (state)
		{
			// if ready, hide the beam
			case WeaponState.Ready:
				lineRenderer.enabled = false;
				break;

			// if charging, increase the charge amount
			case WeaponState.Charging:
				lineRenderer.enabled = false;
				remainingChargeTime -= Time.deltaTime;
				break;

			// if firing, update/render the beam and check for meaningful collisions
			case WeaponState.Firing:
				if (remainingFiringTime > 0f)
				{
					var from = transform.position + transform.forward * .2f;
					var direction = transform.forward;

					ray.origin = from;
					ray.direction = direction;

					var didHit = Physics.Raycast(ray, out rayHit);
					var to = didHit ? rayHit.point : transform.forward * MaxRange;

					// draw the beam
					lineRenderer.SetPosition(0, from);
					lineRenderer.SetPosition(1, to);
					lineRenderer.endWidth = firingPower / 2f;
					lineRenderer.enabled = true;

					// track firing time
					remainingFiringTime -= Time.deltaTime;

					// knock back player that is firing... this is kinda jank...
					var knockBack = transform.forward;

					knockBack *= -KnockBackStrength;
					player.controller.Move(knockBack);

					// deal damage to players
					if (didHit && rayHit.collider.gameObject.tag == "Player")
					{
						var target = rayHit.collider.gameObject.GetComponent<Player>();	

						target.Health -= 1;
					}
				}
				else 
				{
					state = WeaponState.Ready;
				}
				break;
		}
	}
}
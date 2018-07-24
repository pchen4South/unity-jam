using UnityEngine;

public class JumpWeapon : AbstractWeapon 
{
	enum WeaponState { Ready, Charging, Firing };

	[SerializeField]
	LineRenderer lineRenderer;
	[SerializeField]
	AnimationCurve beamDirectionCurve;

	[Header("Properties")]
	public float MaxRange = 100f;
	public float FiringDuration = 1f;

	[Header("Cached Variables")]
	Ray ray = new Ray();
	RaycastHit rayHit = new RaycastHit();

	[Header("State")]
	WeaponState state = WeaponState.Ready;
	float remainingFiringTime = 0f;
	float firingPower = 0f;

	public override void PullTrigger(Player p)
	{
		if (state == WeaponState.Ready && !p.isGrounded)
		{
			state = WeaponState.Charging;
		}
	}

	public override void ReleaseTrigger(Player p)
	{
		if (state == WeaponState.Charging)
		{
			state = WeaponState.Firing;
			remainingFiringTime = FiringDuration;
			firingPower = p.aerialHeight;
		}
	}

	void Update()
	{
		switch (state)
		{
			// if ready, hide the beam
			case WeaponState.Ready:
				player.canMove = true;
				lineRenderer.enabled = false;
				break;

			// if charging, increase the charge amount
			case WeaponState.Charging:
				player.canMove = false;
				lineRenderer.enabled = false;

				break;

			// if firing, update/render the beam and check for meaningful collisions
			case WeaponState.Firing:
				if (remainingFiringTime > 0f)
				{
					var from = transform.position + transform.forward * .2f;
					var direction = Vector3.Slerp(transform.forward, Vector3.down, beamDirectionCurve.Evaluate(remainingFiringTime / FiringDuration));

					ray.origin = from;
					ray.direction = direction;

					var didHit = Physics.Raycast(ray, out rayHit);
					var to = didHit ? rayHit.point : transform.forward * MaxRange;

					// root the firing player
					player.canMove = false;

					// draw the beam
					lineRenderer.SetPosition(0, from);
					lineRenderer.SetPosition(1, to);
					lineRenderer.endWidth = Mathf.Lerp(firingPower / 4f, .1f, remainingFiringTime / FiringDuration);
					lineRenderer.enabled = true;

					// track firing time
					remainingFiringTime -= Time.deltaTime;

					// deal damage to players
					if (didHit && rayHit.collider.gameObject.CompareTag("Player"))
					{
						var target = rayHit.collider.gameObject.GetComponent<Player>();	

						// don't damage ourselves!
						if (target != player)
						{
							target.Health -= 1;
						}
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
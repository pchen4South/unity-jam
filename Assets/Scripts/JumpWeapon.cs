using UnityEngine;

public class JumpWeapon : AbstractWeapon 
{
	enum WeaponState { Ready, Charging, Firing };

	[SerializeField]
	LineRenderer lineRenderer;
	[SerializeField]
	AnimationCurve beamDirectionCurve;

	[Header("Prefabs")]
	public WeaponBar ChargeBar;
	[Header("Properties")]
	public float MaxRange = 100f;
	public float FiringDuration = 1f;

	//weapon bar stuff
	public Color BarColor1 = Color.white;
    public Color BarColor2 = Color.yellow;
    public Color BarColor3 = Color.red;
	private WeaponBar BarInstance;
	// Added maxchargetime for max time you can float in the air
	public float FullChargeTime = 1f;

	[Header("Cached Variables")]
	Ray ray = new Ray();
	RaycastHit rayHit = new RaycastHit();

	[Header("State")]
	WeaponState state = WeaponState.Ready;
	float remainingFiringTime = 0f;
	float firingPower = 0f;
	float chargeTime = 0f;


	private void Start(){
		var bar = Instantiate(ChargeBar, player.transform.position + player.transform.up * 1.02f , player.transform.rotation, player.transform);
		bar.player = player;
		bar.maxBarColor = BarColor3;
		BarInstance = bar;
	}
	public override void PullTrigger(Player p)
	{
		if (state == WeaponState.Ready && !p.isGrounded)
		{
			state = WeaponState.Charging;
			chargeTime = 0;
		}
	}

	public override void ReleaseTrigger(Player p)
	{
		if (state == WeaponState.Charging)
		{
			state = WeaponState.Firing;
			remainingFiringTime = FiringDuration;
			firingPower = p.aerialHeight;
			BarInstance.slider.value = 0;
		}
	}

	void Update()
	{
		switch (state)
		{
			// if ready, hide the beam
			case WeaponState.Ready:
				player.rooted = false;
				lineRenderer.enabled = false;
				break;

			// if charging, increase the charge amount
			case WeaponState.Charging:
				player.rooted = true;
				lineRenderer.enabled = false;

			   // Weapon bar stuff
				chargeTime += Time.deltaTime;
				var img = BarInstance.img;
        		var percentCharged = (chargeTime <= FullChargeTime ? chargeTime : FullChargeTime) / FullChargeTime;
				 BarInstance.slider.value = percentCharged;

				if (percentCharged < 0.5)
				{
					img.color = Color.Lerp(BarColor1, BarColor2, (float)percentCharged / 0.5f);
				}
				else if (percentCharged > 0.5 && percentCharged < 1)
				{
					img.color = Color.Lerp(BarColor2, BarColor3, (float)((percentCharged - 0.5f)/ 0.5f));
				} 
				else if (percentCharged == 1 && chargeTime == FullChargeTime)
				{
					img.color = BarColor3;
				}

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
					player.rooted = true;

					// draw the beam
					lineRenderer.SetPosition(0, from);
					lineRenderer.SetPosition(1, to);
					lineRenderer.endWidth = Mathf.Lerp(firingPower / 4f, .1f, remainingFiringTime / FiringDuration);
					lineRenderer.enabled = true;

					// track firing time
					remainingFiringTime -= Time.deltaTime;

					// deal damage to players
					if (didHit && rayHit.collider.gameObject.tag == "Player")
					{
						var target = rayHit.collider.gameObject.GetComponent<Player>();	

						// don't damage ourselves!
						if (target != player)
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
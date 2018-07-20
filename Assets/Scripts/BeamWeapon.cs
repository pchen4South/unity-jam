using UnityEngine;

public class BeamWeapon : AbstractWeapon 
{
	enum WeaponState { Ready, Charging, Firing };

	[SerializeField]
	LineRenderer lineRenderer;

	[Header("Prefabs")]
	public WeaponBar ChargeBar;

	[Header("Properties")]
	public float MaxRange = 100f;
	public float KnockBackStrength = .8f;
	public float FullChargeTime = 1f;
	public float FiringDuration = 1f;
	public Color BarColor1 = Color.white;
    public Color BarColor2 = Color.yellow;
    public Color BarColor3 = Color.red;

	[Header("Cached Variables")]
	Ray ray = new Ray();
	RaycastHit rayHit = new RaycastHit();

	[Header("State")]
	WeaponState state = WeaponState.Ready;
	float chargeTime = 0f;
	float remainingFiringTime = 0f;
	float firingPower = 0f;
	private WeaponBar BarInstance;
	private void Start(){
		var bar = Instantiate(ChargeBar, player.transform);
		bar.player = player;
		bar.maxBarColor = BarColor3;
		BarInstance = bar;
	}

	public override void PullTrigger(Player p)
	{
		if (state == WeaponState.Ready)
		{
			state = WeaponState.Charging;
			chargeTime = 0;
		}
	}

	public override void ReleaseTrigger(Player p)
	{
		if (state == WeaponState.Charging)
		{
			var time = Mathf.Min(FullChargeTime, chargeTime);
			var power = time / FullChargeTime;

			state = WeaponState.Firing;
			remainingFiringTime = time;
			firingPower = power; 
			BarInstance.slider.value = 0;
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
				chargeTime += Time.deltaTime;
				
				// Weapon bar stuff
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

					knockBack *= -KnockBackStrength * firingPower;
					player.controller.Move(knockBack);

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
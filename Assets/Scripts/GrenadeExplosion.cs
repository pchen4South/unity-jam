// script to render explosion
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour 
{
	[Header("Cached references")]
	public AudioSource sound;
	public Renderer renderer;

	[Header("Configuration")]
	private float ramptime = 0;
	private float alphatime = 1;	
	public float loopduration = 1;
	public float despawnTime = 7f;

	[Header("State")]
	public int PlayerNumber;

	void Start()
	{
		// TODO: probably is best to create copy of the base material on start?
		// Stupid unity...
		CheckExplosionRadius();
	}

	void Update () 
	{
		float r = Mathf.Sin((Time.time / loopduration) * (2 * Mathf.PI)) * 0.5f + 0.25f;
		float g = Mathf.Sin((Time.time / loopduration + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
		float b = Mathf.Sin((Time.time / loopduration + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
		float correction = 1 / (r + g + b);

		r *= correction;
		g *= correction;
		b *= correction;
		ramptime += Time.deltaTime * 2;
		alphatime -= Time.deltaTime;		
		renderer.material.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
		renderer.material.SetVector("_Range", new Vector4(ramptime, 0, 0, 0));
		renderer.material.SetFloat("_ClipRange", alphatime);
		Destroy(gameObject, despawnTime);
	}

	void CheckExplosionRadius()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);

		sound.Play();
		foreach (Collider col in hitColliders)
		{
			if(col.gameObject.tag == "Player")
			{
				var player = col.gameObject.GetComponent<Player>();

				if (player.PlayerNumber != PlayerNumber)
				{
					player.Health = 0;
				}
			}
		}
	}
}
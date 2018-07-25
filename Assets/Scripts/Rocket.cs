using UnityEngine;

public class Rocket : MonoBehaviour 
{
	[SerializeField]
	GrenadeExplosion ExplosionPrefab;

	public Rigidbody Body;
	public int PlayerNumber;

	void Start()
	{
		Destroy(gameObject, 4f);
	}

	void OnCollisionEnter(Collision other)
    {
		Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
    }
}
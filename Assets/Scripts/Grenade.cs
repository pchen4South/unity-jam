using UnityEngine;

public class Grenade : MonoBehaviour 
{
    [Header("Cached components")]
    public Rigidbody body;

    [Header("Configuration")]
    public float fuseTime = 3f;
    private float timeAlive = 0f;

    [Header("Prefabs")]
    public GrenadeExplosion Explosion;
    
    [Header("State")]
    public int PlayerNumber;

	void Update() 
    {
        timeAlive += Time.deltaTime;

        if (timeAlive >= fuseTime) 
        {
            var explosion = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);

            explosion.PlayerNumber = PlayerNumber;
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().PlayerNumber != PlayerNumber)
        {
            body.velocity = Vector3.zero;
            body.isKinematic = true;
        } 
    }
}
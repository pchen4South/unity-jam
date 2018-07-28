using UnityEngine;

public class SprayNade : MonoBehaviour 
{
    [Header("Cached components")]
    public Rigidbody body;

    [Header("Configuration")]
    public float fuseTime = 1f;
    private float timeAlive = 0f;

    [Header("Prefabs")]
    public GrenadeExplosion Explosion;
    public ParticleSystem DirectHit;

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
        var otherGObj = other.gameObject;

        //if collision is with the originating player
        if (otherGObj.tag == "Player") {
            var otherPlayer = otherGObj.GetComponent<Player>();
            var otherPNum = otherPlayer.PlayerNumber;

            if (otherPNum != PlayerNumber)
            {
                //direct hit - instantly removes the rest of the other player's health
                otherPlayer.Damage(otherPlayer.Health, PlayerNumber);

                Instantiate(DirectHit, otherPlayer.transform);

                Destroy(gameObject);
            }
            else
            {
                return;
            }
        // else collision is with a wall or terrain, explode and check explosion radius
        } else {
            var explosion = Instantiate(Explosion, gameObject.transform.position,
                                gameObject.transform.rotation);
            explosion.PlayerNumber = PlayerNumber;
            Destroy(gameObject);
        }
    }
}
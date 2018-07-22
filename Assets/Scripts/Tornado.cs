using UnityEngine;

public class Tornado : MonoBehaviour 
{
    [Header("Cached components")]
    public Rigidbody body;

    [Header("Configuration")]
    public float lifeTime = 1.5f;
    public AnimationCurve scaleCurve;

    [Header("State")]
    private float timeAlive = 0f;
    public int PlayerNumber;

	void Update() 
    {
        var normalizedLifetime = timeAlive / lifeTime;
        var currentScale = scaleCurve.Evaluate(normalizedLifetime);

        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        timeAlive += Time.deltaTime;

        if (timeAlive >= lifeTime) 
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
            return;

        var player = other.gameObject.GetComponent<Player>();

        if (player.PlayerNumber == PlayerNumber)
            return;

        player.Health -= 1;
    }
}

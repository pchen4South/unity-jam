using UnityEngine;

public class MachineGunBullet : MonoBehaviour 
{
	[Header("Cached components")]
    public Rigidbody body;

    [Header("Configuration")]
    public float LifeTime = 2f;

    [Header("State")]
    private float timeAlive = 0f;
    
    [Header("State")]
    public int PlayerNumber;

	void Update() 
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= LifeTime)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerNumber != other.gameObject.GetComponent<Player>().PlayerNumber)
        {
			var target = other.gameObject.GetComponent<Player>();

			target.Damage(1, PlayerNumber);
        }
        else
        {
			Destroy(gameObject);
		}          
    }
}

using UnityEngine;

public class Player : MonoBehaviour 
{
	[SerializeField]
	Rigidbody body;

	public float RotationSpeed = .1f;
	public float MovementSpeed = .2f;

	void Awake()
	{

	}

	void Start()
	{

	}

	void OnDestroy()
	{

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, transform.forward * 10f);
	}

	void Update()
	{
		Vector3 p = transform.position;
		bool goingForward = Input.GetKey("w");
		bool goingBackward = Input.GetKey("s");
		bool turningLeft = Input.GetKey("a");
		bool turningRight = Input.GetKey("d");
		bool jumping = Input.GetKey("space");

		if (goingForward || goingBackward)
			Debug.Log("Moving");

		if (goingForward)
			p += transform.forward * MovementSpeed;

		if (goingBackward)
			p -= transform.forward * MovementSpeed;

		if (turningLeft)
			transform.Rotate(Vector3.up, -RotationSpeed);

		if (turningRight)
			transform.Rotate(Vector3.up, RotationSpeed);

		if (jumping)
			body.AddForce(Vector3.up, ForceMode.Impulse);

		transform.position = p;
	}
}
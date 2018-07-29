using UnityEngine;

public class Balloon : MonoBehaviour 
{
	[SerializeField]
	Rigidbody balloonBody;
	[SerializeField]
	LineRenderer lineRenderer;

	public MeshRenderer meshRenderer;
	public SpringJoint springJoint;
	public float bouyancy = 10f;
	public float despawnTime = 4f;
	public bool isCut = false;

	public void Cut()
	{
		isCut = true;
		Destroy(springJoint);
		Destroy(lineRenderer);
		Destroy(gameObject, despawnTime);
	}

	void Update()
	{
		if (isCut)
			return;

		lineRenderer.enabled = true;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, springJoint.connectedBody.transform.position);
	}

	void FixedUpdate()
	{
		balloonBody.AddForce(Vector3.up * bouyancy, ForceMode.Acceleration);
	}
}
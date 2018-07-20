using UnityEngine;

public class Hook : MonoBehaviour 
{
	[SerializeField]
	LineRenderer lineRenderer;

	public Hookshot weapon;
	public bool isFree = true;

	public void Update()
	{
		lineRenderer.widthMultiplier = .1f;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, weapon.transform.position);
	}

	public void OnTriggerEnter(Collider other)
	{
		isFree = false;
	}
}
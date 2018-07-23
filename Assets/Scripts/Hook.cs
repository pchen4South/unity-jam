using UnityEngine;

public class Hook : MonoBehaviour 
{
	[SerializeField]
	LineRenderer lineRenderer;

	public AudioSource audioSource;

	public Hookshot weapon;

	public void Update()
	{
		lineRenderer.widthMultiplier = .1f;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, weapon.transform.position);
	}
}
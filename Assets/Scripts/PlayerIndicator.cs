using UnityEngine;

public class PlayerIndicator : MonoBehaviour 
{
	public float rotationSpeed = 100f;
	public Color color;

	[SerializeField] MeshRenderer MeshRenderer;

	void Update()
	{
		transform.rotation = Quaternion.AngleAxis(rotationSpeed * Time.time, Vector3.up);
		MeshRenderer.material.color = color;
	}
}
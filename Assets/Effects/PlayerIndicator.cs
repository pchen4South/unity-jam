using UnityEngine;

public class PlayerIndicator : MonoBehaviour 
{
	public MeshRenderer meshRenderer;
	public float rotationSpeed = 100f;

	void Update()
	{
		var radians = rotationSpeed * Time.time;

		meshRenderer.material.SetFloat("_SinAngle", Mathf.Sin(radians));
		meshRenderer.material.SetFloat("_CosAngle", Mathf.Cos(radians));
	}
}
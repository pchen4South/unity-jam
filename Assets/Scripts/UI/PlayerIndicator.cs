using UnityEngine;

public class PlayerIndicator : MonoBehaviour 
{
	public float rotationSpeed = 100f;
	public Color color;

	[SerializeField] MeshRenderer MeshRenderer  = null;
	[SerializeField] float IndicatorAlpha = .8f;

	void Update()
	{
		transform.rotation = Quaternion.AngleAxis(rotationSpeed * Time.time, Vector3.up);
		Color meshcolor = new Color();
		meshcolor = color;
		meshcolor.a = IndicatorAlpha;
		MeshRenderer.material.color = meshcolor;
	}
}
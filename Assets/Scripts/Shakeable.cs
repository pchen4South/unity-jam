using UnityEngine;

public class Shakeable : MonoBehaviour 
{
	public Camera shakyCamera;
	[Range(0f, 90f)]
	public float MaxAngle = 8f;
	[Range(0f, 10f)]
	public float MaxOffset = 1f;
	[Range(0f, 10f)]
	public float sampleVelocity = 1f;
	[Range(0f, 1f)]

	float seed = 0f;
	float intensity = 0f;

	public void AddIntensity(float di)
	{
		intensity = Mathf.Clamp01(intensity + di);
	}

	void Update()
	{
		var possibleAngle = intensity * MaxAngle;
		var possibleOffset = intensity * MaxOffset;
		var angle = Mathf.Lerp(-possibleAngle, possibleAngle, Mathf.PerlinNoise(seed++ * sampleVelocity, Time.time));
		var xOffset = Mathf.Lerp(-possibleOffset, possibleOffset, Mathf.PerlinNoise(seed++ * sampleVelocity, Time.time));
		var yOffset = Mathf.Lerp(-possibleOffset, possibleOffset, Mathf.PerlinNoise(seed++ * sampleVelocity, Time.time));
		var shakyPosition = transform.position + transform.up * yOffset + transform.right * xOffset;
		var shakyRotation = transform.rotation * Quaternion.Euler(transform.forward * angle);

		shakyCamera.transform.position = shakyPosition;
		shakyCamera.transform.rotation = shakyRotation;
		intensity += (0f - intensity) * .1f * Time.timeScale;
	}
}
using UnityEngine;

public class Rotator : MonoBehaviour 
{
	public float Speed = 1f;
	public Vector3 Axis = Vector3.up;
	void Update()
	{
		transform.Rotate(Axis, Speed * Time.deltaTime);
	}
}
using UnityEngine;

public class Wanderer : MonoBehaviour 
{
	public float WanderSpeed = 100f;
	public Vector3 Point = Vector3.zero;
	public Vector3 Axis = Vector3.forward;

	void Update () 
	{
		transform.RotateAround(Point, Axis, WanderSpeed * Time.deltaTime);
	}
}
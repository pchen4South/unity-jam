using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    private CharacterController character;
    private Rigidbody body;
    public float MoveSpeed;
    public float RotateSpeed;

    private Vector3 _velocity;

	// Use this for initialization
	void Start () {
		
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
    }

    // Update is called once per frame
    void Update () {

        // move forward
        if (Input.GetKey(KeyCode.W))
            character.Move(transform.forward * Time.deltaTime * MoveSpeed);
      
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -RotateSpeed);

        if (Input.GetKey(KeyCode.D))
          transform.Rotate(Vector3.up, RotateSpeed);


        if (character.isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        var Gravity = -981f;

        _velocity.y = Gravity * Time.deltaTime;
 
        if (Input.GetKey(KeyCode.Space) && character.isGrounded)
            _velocity.y += Mathf.Sqrt(500f * -2f * Gravity);

        character.Move(_velocity * Time.deltaTime);
    }
}

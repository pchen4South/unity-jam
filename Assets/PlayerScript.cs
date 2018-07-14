using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    private CharacterController character;
    private Rigidbody body;
    public float MoveSpeed = 2f;
    public float RotateSpeed = 5f;
    public float JumpStrength = 2f;

    public Vector3 Gravity = new Vector3(0, -100, 0);

    private Vector3 _velocity;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
    }

    void Update () 
    {
        if (Input.GetKey(KeyCode.W))
            character.Move(transform.forward * Time.deltaTime * MoveSpeed);
      
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -RotateSpeed);

        if (Input.GetKey(KeyCode.D))
          transform.Rotate(Vector3.up, RotateSpeed);


        if (character.isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        _velocity += Gravity * Time.deltaTime;
 
        if (Input.GetKey(KeyCode.Space) && character.isGrounded)
            _velocity += new Vector3(0, JumpStrength, 0);

        character.Move(_velocity * Time.deltaTime);
    }
}

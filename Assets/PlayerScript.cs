using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{
    [SerializeField]
    CharacterController character;
    [SerializeField]
    GameObject BulletPrefab;

    public float GroundCheckDistance = .1f;
    public float MoveSpeed = 2f;
    public float RotateSpeed = 5f;
    public float JumpStrength = 2f;
    public float Gravity = -100f;

    float VerticalVelocity = 0;

    [Header("Input")]
    [Range(0, 1)]
    public int PlayerNumber = 0;
    string HorizontalInput = "";
    string VerticalInput = "";
    string FireInput = "";
    string JumpInput = "";

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * GroundCheckDistance);
    }

    void Start()
    {
        HorizontalInput = "Horizontal_" + PlayerNumber;
        VerticalInput = "Vertical_" + PlayerNumber;
        FireInput = "Fire_" + PlayerNumber;
        JumpInput = "Jump_" + PlayerNumber;
    }

    void Update () 
    {
        var moveDelta = Vector3.zero;
        var jumpDown = Input.GetButtonDown(JumpInput);
        var fireDown = Input.GetButton(FireInput);
        var horizontalAxis = Input.GetAxis(HorizontalInput);
        var verticalAxis = Input.GetAxis(VerticalInput);
        var isGrounded = Physics.Raycast(transform.position, Vector3.down, GroundCheckDistance);

        moveDelta = transform.forward * Time.deltaTime * MoveSpeed * verticalAxis;
        transform.Rotate(Vector3.up, RotateSpeed * horizontalAxis);

<<<<<<< HEAD
        if (Input.GetKey(KeyCode.F))
            Fire();

=======
        if (fireDown)
            Fire();
>>>>>>> 9d5318bd9475b876f005aa7fb60a45f2d9d1f2b0

        if (isGrounded)
        {
            if (jumpDown)
            {
                VerticalVelocity = JumpStrength;
            }
            else
            {
                VerticalVelocity = 0f;
            }
        }
        else
        {
            VerticalVelocity += Gravity * Time.deltaTime;
        }
        moveDelta.y += VerticalVelocity * Time.deltaTime;
        character.Move(moveDelta);
    }

    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(BulletPrefab, transform.position, transform.rotation);

<<<<<<< HEAD
        character.Move(_velocity * Time.deltaTime);


    }

    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            transform.position,
            transform.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }


}
=======
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
>>>>>>> 9d5318bd9475b876f005aa7fb60a45f2d9d1f2b0

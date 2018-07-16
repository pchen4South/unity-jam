using UnityEngine;

public class Player : MonoBehaviour 
{
    public CharacterController controller;

    public AbstractWeapon Weapon;

    public float GroundCheckDistance = .01f;
    public float MoveSpeed = 2f;
    public float RotateSpeed = 5f;
    public float JumpStrength = 2f;
    public float Gravity = -100f;

    float VerticalVelocity = 0f;

    [Header("Input")]
    public int PlayerNumber = 0;
    string HorizontalInput = "";
    string VerticalInput = "";
    string FireInput = "";
    string JumpInput = "";

    public int Health = 1;

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

    void Update() 
    {
        var ray = new Ray(transform.position, Vector3.down);
        var rayHit = new RaycastHit();
        var moveDelta = Vector3.zero;
        var jumpDown = Input.GetButtonDown(JumpInput);
        var fireDown = Input.GetButton(FireInput);
        var fireUp = Input.GetButtonUp(FireInput);
        var horizontalAxis = Input.GetAxis(HorizontalInput) * MoveSpeed;
        var verticalAxis = Input.GetAxis(VerticalInput) * MoveSpeed;
        var didHit = Physics.Raycast(ray, out rayHit, 1000f);
        var isGrounded = didHit && rayHit.distance < GroundCheckDistance;

        //isGrounded = true;

        if( !float.IsNaN(horizontalAxis) && !float.IsNaN(verticalAxis))
            moveDelta = new Vector3(horizontalAxis, 0, verticalAxis);

        if (moveDelta != Vector3.zero)
            transform.forward = moveDelta;

        // Steve: New weapon logic should go here. Deprecate the use of Weapon.Fire completely
        if (Weapon != null && fireDown)
        {
            Weapon.PullTrigger(this);
        }
        if (Weapon != null && fireUp)
        {
            Weapon.ReleaseTrigger(this);
        }

        if (controller.isGrounded)
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

        //moveDelta.x += .001f;
        //moveDelta.y += .001f;
        //moveDelta.z += .001f;

        if (!float.IsNaN(moveDelta.x) && !float.IsNaN(moveDelta.y) && !float.IsNaN(moveDelta.z))
        {
            try
            {
                controller.Move(moveDelta);
            }
            catch {
                moveDelta = Vector3.zero;
                controller.Move(moveDelta);
            }

        }
    }
}
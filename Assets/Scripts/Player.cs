using UnityEngine;

public class Player : MonoBehaviour 
{
    [SerializeField]
    CharacterController character;

    public AbstractWeapon Weapon;

    public float GroundCheckDistance = .01f;
    public float MoveSpeed = 2f;
    public float RotateSpeed = 5f;
    public float JumpStrength = 2f;
    public float Gravity = -100f;
    private bool allowFire = true;
    private float nextFire;
    public float fireRate;

    float VerticalVelocity = 0;

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
        var horizontalAxis = Input.GetAxis(HorizontalInput) * MoveSpeed;
        var verticalAxis = Input.GetAxis(VerticalInput) * MoveSpeed;
        var didHit = Physics.Raycast(ray, out rayHit, 1000f);
        var isGrounded = didHit && rayHit.distance < GroundCheckDistance;

        moveDelta = new Vector3(horizontalAxis, 0, verticalAxis);

        if (moveDelta != Vector3.zero)
            transform.forward = moveDelta;

        if (Weapon != null && fireDown && Time.time > nextFire)
            Weapon.Fire(this);

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

        if(moveDelta != null)
            character.Move(moveDelta);
    }
}
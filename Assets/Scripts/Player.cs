using UnityEngine;
using Rewired;

public class Player : MonoBehaviour 
{
    const float GROUNDED_DOWNWARD_VELOCITY = -10f;

    // TODO: Would like to remove Invincible 
    public enum PlayerStatus { Alive, Dead, Invincible }

    [SerializeField] PlayerIndicator playerIndicator;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource spawnSound;

    public Color color = Color.red;
    public int MaxHealth = 3;
    public float MoveSpeed = 2f;
    public float JumpStrength = 2f;
    public float CrouchMovementModifier = 0.5f;
    public System.Action<int, int> OnDeath;

    public AbstractWeapon Weapon;
    public int PlayerNumber = 0;
    public int Health = 1;
    public bool canMove = true;
    public bool canRotate = true;
    public float aerialHeight = 0f;
    public float VerticalVelocity = 0f;
	public PlayerStatus status = PlayerStatus.Alive;
    public bool isGrounded = true;

    Rewired.Player player;
    float standingHeight;
    Vector3 standingCenter;
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
        Gizmos.color = controller.isGrounded ? Color.green : Color.gray;
        Gizmos.DrawCube(transform.position + transform.up * 3f, Vector3.one * .2f);
    }

    void Start()
    {
        player = ReInput.players.GetPlayer(PlayerNumber);
        standingHeight = controller.height;
        standingCenter = controller.center;
    }

    void Update() 
    {
        var ray = new Ray(transform.position, Vector3.down);
        var rayHit = new RaycastHit();
        var jumpDown = player.GetButtonDown("Jump");
        var fireDown = player.GetButtonDown("Fire");
        var fireUp = player.GetButtonUp("Fire");
        var fireHold = player.GetButtonTimedPress("Fire", .01f);
        var crouch = player.GetButtonTimedPress("Crouch", .01f);
        var horizontalAxis = player.GetAxis(0);
        var verticalAxis = player.GetAxis(1);
        var didHit = Physics.Raycast(ray, out rayHit, 1000f);
        var input = new Vector3(horizontalAxis, 0, verticalAxis);
        var moveDelta = Vector3.zero;
        var totalMovementModifier = 1f;
        var hasMouse = player.controllers.hasMouse;

        isGrounded = controller.isGrounded;
        aerialHeight = didHit ? rayHit.distance : 0f;

        // look in direction
        if (canRotate && (horizontalAxis != 0 || verticalAxis != 0))
        {
            transform.forward = input.normalized;
        }

        if (hasMouse)
        {
            // TODO: this is not ... good and shouldn't be here anyway... remove
            Vector2 tInViewport = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 delta = player.controllers.Mouse.screenPosition - tInViewport;
            Vector2 direction = delta.normalized;
            Vector3 orientedDirection = new Vector3(direction.x, 0, direction.y);

            transform.rotation = Quaternion.LookRotation(orientedDirection, Vector3.up);
        }

        // Check/Jump
        if (isGrounded)
        {
            if (jumpDown)
            {
                VerticalVelocity = JumpStrength;
            }
            else
            {
                VerticalVelocity = GROUNDED_DOWNWARD_VELOCITY;
                if (crouch == true)
                {
                    totalMovementModifier *= CrouchMovementModifier;
                    controller.height = standingHeight / 2;
                    controller.center = new Vector3(standingCenter.x, standingCenter.y / 2, standingCenter.z);
                }
                else 
                {
                    controller.height = standingHeight;
                    controller.center = standingCenter;
                }
            }
        }
        else
        {
            if (canMove)
            {
                VerticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                VerticalVelocity = 0f;
            }
        }
        
        // move if not rooted
        if (canMove)
        {
            moveDelta.x += horizontalAxis * Time.deltaTime * MoveSpeed * totalMovementModifier;
            moveDelta.y += VerticalVelocity * Time.deltaTime;
            moveDelta.z += verticalAxis * Time.deltaTime * MoveSpeed * totalMovementModifier;
        }

        // Weapon inputs
        if (Weapon != null && (fireDown || fireHold))
        {
            Weapon.PullTrigger(this);
        }
        if (Weapon != null && fireUp)
        {
            Weapon.ReleaseTrigger(this);
        }
        
        controller.Move(moveDelta);
        
        // TODO: looks like this could be simplified a lot
        // Animation stuff
        if(animator != null)
        {
            if (MoveSpeed != 0f)
            {
                float move = 0f;
                Vector3 twodmove = new Vector3(moveDelta.x, 0, moveDelta.z);

                if (Mathf.Abs(horizontalAxis) > 0 || Mathf.Abs(verticalAxis) > 0)
                {
                    move = Vector3.Magnitude(twodmove) * (MoveSpeed / Time.deltaTime);
                }
                animator.SetFloat("Forward", move);
                animator.SetFloat("Jump", VerticalVelocity + (GROUNDED_DOWNWARD_VELOCITY * -1));
            }
            animator.SetBool("OnGround", isGrounded);
            animator.SetBool("Crouch", crouch);
        }

        playerIndicator.transform.position = didHit ? rayHit.point : transform.position;
        playerIndicator.meshRenderer.material.color = color;
        meshRenderer.material.color = color;
    }

    public void SetWeapon(AbstractWeapon newWeapon)
    {
        if (Weapon)
        {
            Destroy(Weapon.gameObject);
        }
        Weapon = Instantiate(newWeapon, transform);
        Weapon.player = this;
        canRotate = true;
        canMove = true;
    }

    // guns call this damage function which calls a callback to handle death
    // not sure if this is any good...
    public void Damage(int amountOfDamage, int attackerIndex)
    {
        if (status == PlayerStatus.Dead)
            return;

        Health = Mathf.Max(0, Health - amountOfDamage);
        animator.SetTrigger("Hit");
        takeDamageSound.Play();

        if (Health <= 0)
        {
            Kill(attackerIndex);
        }
    }

    public void Kill(int attackerIndex)
    {
        status = PlayerStatus.Dead;
        Health = 0;
        canMove = false;
        canRotate = false;
        VerticalVelocity = 0f;

        OnDeath.Invoke(PlayerNumber, attackerIndex);
        animator.SetBool("PlayDeathAnimation", true);
        deathSound.Play();
    }

	public void Spawn(Transform t)
	{
        status = PlayerStatus.Alive;
		Health = MaxHealth;
        canMove = true;
        canRotate = true;
        VerticalVelocity = 0f;

		transform.SetPositionAndRotation(t.position, t.rotation);
        animator.SetBool("PlayDeathAnimation", false);
        spawnSound.Play();
	}
}
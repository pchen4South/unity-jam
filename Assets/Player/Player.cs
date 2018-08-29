using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Player : MonoBehaviour 
{
    public enum PlayerStatus { Alive, Dying, Dead, Invincible}
    // N.B. This must be applied to the character every frame they are grounded to keep them grounded
    const float GROUNDED_DOWNWARD_VELOCITY = -10f;

    [SerializeField]
    PlayerIndicator playerIndicator;

    public Rigidbody head;
    public SkinnedMeshRenderer meshRenderer;
    public CharacterController controller;
    public AbstractWeapon Weapon;
    public Animator animator;
    public Color color = Color.red;
    public AudioSource TakeDamageSound;
    public AudioSource InvicibleSound;
    public AudioSource DeathSound;

    public int MaxHealth = 3;
    public float MoveSpeed = 2f;
    public float JumpStrength = 2f;
    public float JumpPadStrength = 3f;
    public float CrouchMovementModifier = 0.5f;

    public float MaxHorizontalAimAngle = 60;
    public float MaxVerticalAimAngle = 60;
    public bool InvertAimVertical = false;
    public int PlayerNumber = 0;
    public int Health = 1;
    public bool canMove = true;
    public bool canRotate = true;
    public float aerialHeight = 0f;
    public float VerticalVelocity = 0f;

    public int lastAttackerIndex;

    Camera mainCamera;

    //reinput
    private Rewired.Player player;
    //state
    float standingHeight;
    Vector3 standingCenter;
   
    [Header("State")]
	public PlayerStatus status = PlayerStatus.Alive;
    private Vector3 PlayerDiedPosition;
    private Vector3 PlayerDeadBodyPosition;
    bool isFlashing = false;
    public bool isGrounded = true;
    public bool IsDead = false;
    
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
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(PlayerNumber);
        standingHeight = controller.height;
        standingCenter = controller.center;
        mainCamera = Camera.main;
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
            Vector2 tInViewport = mainCamera.WorldToScreenPoint(transform.position);
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
                var strength = didHit && rayHit.collider.CompareTag("JumpPad") ? JumpPadStrength : JumpStrength;

                VerticalVelocity = strength;
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
                else {
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
        
        //Animation stuff
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
            if (status == PlayerStatus.Dying) 
            {
                animator.SetBool("PlayDeathAnimation", true);
                canMove = false;
                canRotate = false;
                KnockbackPlayerOnDeath();
            }
        }

        // playerHUD.UpdateHealth((float)Health / (float)MaxHealth);
        // playerHUD.transform.rotation = Quaternion.LookRotation(-mainCamera.transform.forward, mainCamera.transform.up);
        playerIndicator.transform.position = didHit ? rayHit.point : transform.position;
        playerIndicator.meshRenderer.material.color = color;
        
        meshRenderer.material.color = color;

        if(status == PlayerStatus.Invincible && !isFlashing)
        {
            StartCoroutine(FlashPlayerModel());
        }
    }

    public void KnockbackPlayerOnDeath(){
        transform.position = Vector3.Lerp(transform.position, PlayerDeadBodyPosition, Time.deltaTime * 5);
    }

    public void DeathAnimationFinished() 
    {
        animator.SetBool("PlayDeathAnimation", false);
        IsDead = true;
        canMove = true;
        canRotate = true;
        status = PlayerStatus.Dead;
    }

    // TODO: Call some kind of reset on the weapon to clear modifiers to the player?
    public void SetWeapon(AbstractWeapon newWeapon)
    {
        var oldWeapon = Weapon;

        Weapon = Instantiate(newWeapon, transform);
        Weapon.player = this;

        if (oldWeapon != null)
        {
            oldWeapon.player = null;
            Destroy(oldWeapon.gameObject);
        }
    }

    public void Damage(int amountOfDamage, int attackerIndex)
    {
        if (Health <= 0){
            return;
        } 

        if(status != PlayerStatus.Invincible){
            Health -= amountOfDamage;
            lastAttackerIndex = attackerIndex;

            if (Health > 0 )
            {
                animator.SetTrigger("Hit");
                status = PlayerStatus.Invincible;
                TakeDamageSound.Play();
                StartCoroutine(PlayerDamaged());
            } else if (Health <= 0){
                PlayerDiedPosition = transform.position;
                PlayerDeadBodyPosition = new Vector3(PlayerDiedPosition.x - 1f, PlayerDiedPosition.y - 1f, PlayerDiedPosition.z);
                status = PlayerStatus.Dying;
                DeathSound.Play();
            }
        } else {
            InvicibleSound.Play();            
        }
    }

    IEnumerator PlayerDamaged(){
        yield return new WaitForSeconds(2f);
        status = PlayerStatus.Alive;
    }

    IEnumerator FlashPlayerModel(){
        isFlashing = true;
        color.a = 0f;
        //meshRenderer.material.color = color;    
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FlashPlayerModelToOriginal());
    }
    IEnumerator FlashPlayerModelToOriginal(){
        color.a = 1f;
        //meshRenderer.material.color = color;    
        yield return new WaitForSeconds(0.1f);
        isFlashing = false;
    }

	public void Respawn(Vector3 position, Quaternion rotation)
	{
		transform.SetPositionAndRotation(position, rotation);
		Health = MaxHealth;
        IsDead = false;
		canMove = true;
		canRotate = true;
		VerticalVelocity = 0f;
	}
}
using System;
using UnityEngine;

public class Player : MonoBehaviour 
{
    // N.B. This must be applied to the character every frame they are grounded to keep them grounded
    const float GROUNDED_DOWNWARD_VELOCITY = -10f;

    public SkinnedMeshRenderer meshRenderer;
    public CharacterController controller;
    public AbstractWeapon Weapon;
    public Animator animator;
    public Color color = Color.red;

    public float MoveSpeed = 2f;
    public float JumpStrength = 2f;
    public string HorizontalInput = "";
    public string VerticalInput = "";
    public string FireInput = "";
    public string JumpInput = "";

    public int PlayerNumber = 0;
    public int Health = 1;
    public bool canMove = true;
    public bool canRotate = true;
    public float aerialHeight = 0f;
    public float VerticalVelocity = 0f;
    public bool isGrounded = true;
    public int lastAttackerIndex;

    [Header("Animation")]
    private float Turn;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
        Gizmos.color = controller.isGrounded ? Color.green : Color.gray;
        Gizmos.DrawCube(transform.position + transform.up * 3f, Vector3.one * .2f);
    }

    void Update() 
    {
        var ray = new Ray(transform.position, Vector3.down);
        var rayHit = new RaycastHit();
        var jumpDown = Input.GetButtonDown(JumpInput);
        var fireDown = Input.GetButton(FireInput);
        var fireUp = Input.GetButtonUp(FireInput);
        var horizontalAxis = Input.GetAxis(HorizontalInput);
        var verticalAxis = Input.GetAxis(VerticalInput);
        var didHit = Physics.Raycast(ray, out rayHit, 1000f);
        var input = new Vector3(horizontalAxis, 0, verticalAxis);
        var moveDelta = Vector3.zero;

        isGrounded = controller.isGrounded;
        aerialHeight = didHit ? rayHit.distance : 0f;

        // look in direction
        if (canRotate && (horizontalAxis != 0 || verticalAxis != 0))
        {
            transform.forward = input.normalized;
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
            moveDelta.x += horizontalAxis * MoveSpeed;
            moveDelta.y += VerticalVelocity * Time.deltaTime;
            moveDelta.z += verticalAxis * MoveSpeed;
        }

        // Weapon inputs
        if (Weapon != null && fireDown)
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
            /*
            float turn = 0f;

            if(moveDelta.x != 0 || moveDelta.z != 0){
                Vector3 movedir = new Vector3( moveDelta.x - transform.forward.x, 0, moveDelta.z = transform.forward.z);
                Debug.Log("turndir " + movedir );
                turn = Vector3.Magnitude(movedir);

            } else {
                turn = 0f;
            }
            */
            float move = 0f;

            if(Math.Abs(horizontalAxis) > 0 || Math.Abs(verticalAxis) > 0)
            {
                move = 1f;
            }
            animator.SetFloat("Forward", move);
            //animator.SetFloat("Turn", turn);
            animator.SetFloat("Jump", VerticalVelocity + (GROUNDED_DOWNWARD_VELOCITY * -1));
            animator.SetBool("OnGround", isGrounded);
        }

        meshRenderer.material.color = color;
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
        // TODO: This probably should be some kind of state so that controls stop working etc
        if (Health <= 0)
            return;

        Health -= amountOfDamage;
        lastAttackerIndex = attackerIndex;
    }
}
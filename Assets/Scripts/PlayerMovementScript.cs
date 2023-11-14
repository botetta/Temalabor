using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
   


    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    private float speed;

    [Header("Crouching")]
    public float crouchSpeed;
    [Tooltip("By how much the player's size should \"shrink\" when crouching.")]
    public float crouchYScale;
    private float startYScale;

    [Header("Others")]
    public float gravity;
    public float jumpHeight;
    [Tooltip("The player can still jump for a small amount of time, even after they supposedly left the ground")]
    public float jumpInAirAllowedDelay;
    private float jumpDelayTimer; //Counts down from jumpInAirAllowedDelay to 0. Player can jump as long as this is above 0


    public float groundDistance;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump; //Whether or not the player is allowed to jump. Even after the player is no longer grounded, they can still jump for a small amount of time
    private bool wantsToGetOutOfCrouch;

    public MovementState state;
    public enum MovementState
    {
        Walking,
        Running,
        Crouching,
        Air
    }

    private void Start()
    {
        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 groundPosition = transform.position - new Vector3(0, transform.localScale.y, 0);
        isGrounded = controller.isGrounded; //Physics.CheckSphere(groundPosition, groundDistance, groundMask); // Checks if the player is on the ground


        JumpDelayHandler();

        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }
        

        float x = Input.GetAxis("Horizontal"); // A and D keys
        float z = Input.GetAxis("Vertical"); // W and S keys

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(speed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && canJump)
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        if (Input.GetButtonUp("Jump"))
        {
            canJump = false; //This is to prevent the player from jumping multiple times in the air
        }
        CrouchHandler();
        StateHandler();
        
    }

    private void StateHandler()
    {
        if (Input.GetButton("Crouch"))
        {
            state = MovementState.Crouching;
            speed = crouchSpeed;
        }
        else if (Input.GetButton("Sprint"))
        {
            state = MovementState.Running;
            speed = runSpeed;
        }
        else if (!wantsToGetOutOfCrouch)
        {
            state = MovementState.Walking;
            speed = walkSpeed;
        }
        if (!isGrounded)
        {
            state = MovementState.Air;
        }
    }

    private void CrouchHandler()
    {
        float heightOfRaycast = transform.localScale.y + crouchYScale;
        //Start crouching
        if (Input.GetButtonDown("Crouch")) 
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - (1 - crouchYScale), transform.localPosition.z);
            wantsToGetOutOfCrouch = false;
            //speed = crouchSpeed;
        }
        //Stop crouching
        if (Input.GetButtonUp("Crouch"))
        {
            wantsToGetOutOfCrouch = true; //We can't immideately get out of crouch, because the player might be under a low ceiling
            
        }
        if (wantsToGetOutOfCrouch && !Physics.Raycast(transform.position, Vector3.up, heightOfRaycast)) //We can only get out of crouch if there is nothing above us
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            wantsToGetOutOfCrouch = false;
            //speed = walkSpeed;
        }

    }

    //The player can still jump for a small amount of time, even after they supposedly left the ground
    private void JumpDelayHandler()
    {

        if (isGrounded)
        {
            canJump = true;
            jumpDelayTimer = jumpInAirAllowedDelay;
        }
        else
        {
            // If the player is not grounded, they can still jump for a small amount of time
            if (canJump)
            {
                if (jumpDelayTimer > 0)
                {
                    jumpDelayTimer -= Time.deltaTime;
                }
                else
                {
                    canJump = false;
                }
            }
        }
    }
}
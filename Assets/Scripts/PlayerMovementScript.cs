using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[SelectionBase]
public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
    private PostProcessVolume postProcessVolume;
    private ChromaticAberration chromaticAberration;


    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    private float speed;


    [Header("Crouching")]
    public float crouchSpeed;
    [Tooltip("By how much the player's size should \"shrink\" when crouching.")]
    public float crouchYScale;
    private float startYScale;

    [Header("Jumping")]
    public float gravity;
    public float jumpHeight;
    [Tooltip("The player can still jump for a small amount of time, even after they supposedly left the ground")]
    public float jumpInAirAllowedDelay;
    private float jumpDelayTimer; //Counts down from jumpInAirAllowedDelay to 0. Player can jump as long as this is above 0

    [Header("Dashing")]
    [Tooltip("Whether or not the player can dash")]
    public bool dashingAllowed;
    [Tooltip("How long the player should dash for. Shouldn't be too long (<1sec)")]
    public float dashTime;
    [Tooltip("How fast the player should dash. Should be pretty big (>20)")]
    public float dashSpeed;
    private bool isDashing = false;
    //Whether or not the player is currently dashing. 
    public bool IsDashing => isDashing;
    //Values for chromatic aberration effect when dashing
    private float chromaticChangeSpeed = 10f; //How fast the chromatic aberration effect changes
    [Tooltip("The maximum intensity of the chromatic aberration effect when dashing")]
    public float chromaticMaxIntensity = 5f;
    private float chromaticMinIntensity = 0f; //Should be 0, but I'm leaving it here in case we want to change it later
    private float chromaticTargetIntensity = 0f; //The intensity that the chromatic aberration effect is trying to reach (either chromaticMaxIntensity or chromaticMinIntensity)
    private bool canDash = false; //Wheter or not the player can CURRENTLY dash. This is different from dashingAllowed, which is whether or not the player can dash at all. The player can only dash once per jump, so this is used to prevent the player from dashing multiple times in the air

    [Header("Others")]
    public float groundDistance;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump; //Whether or not the player is allowed to jump. Even after the player is no longer grounded, they can still jump for a small amount of time
    private bool wantsToGetOutOfCrouch;
    [SerializeField] private float resistance;

    //Coordinates for the player's spawn point
    public Vector3 SpawnPoint { get; set; }

    private int deaths = 0;
    public int Deaths => deaths;

    private int deathsSinceLastCheckpoint = 0;
    public int DeathsSinceLastCheckpoint => deathsSinceLastCheckpoint;
    private bool cheatPromptShown = false; //Whether or not the cheat prompt has been shown already

    [SerializeField]
    [Tooltip("Whether or not the player is cheating. This is used to turn off gravity when the player is cheating")]
    private bool isCheating = false; //Whether or not the player is currently cheating. This is used to turn off gravity when the player is cheating
    public bool IsCheating => isCheating;

    private bool hasCheated = false; //Whether or not the player has cheated at all.
    public bool HasCheated => hasCheated;


    private Rigidbody rb;



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
        postProcessVolume = GetComponentInChildren<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);

        //Set the spawn point to the player's starting position
        SpawnPoint = transform.position;

        //We need the rigidbody to apply knockback
        rb = GetComponentInChildren<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 groundPosition = transform.position - new Vector3(0, transform.localScale.y, 0);
        isGrounded = controller.isGrounded; //Physics.CheckSphere(groundPosition, groundDistance, groundMask); // Checks if the player is on the ground


        JumpDelayHandler();

        
        if (isGrounded && velocity.y < 0 && !isCheating)
        {
            velocity.y = -0.5f;
        }

        //If the player has a velocity on the x or z axis from some external force, slowly decrease it to 0

        if (Mathf.Abs(velocity.x) <= resistance && Mathf.Abs(velocity.z) <= resistance)
        {
            velocity.x = 0;
            velocity.z = 0;
        }
        if (velocity.x > resistance)
            velocity.x -= resistance;
        else if (velocity.x < -resistance)
            velocity.x += resistance;
        if (velocity.z > resistance)
            velocity.z -= resistance;
        else if (velocity.y < -resistance)
            velocity.z += resistance;

        float x = Input.GetAxis("Horizontal"); // A and D keys
        float z = Input.GetAxis("Vertical"); // W and S keys

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(speed * Time.deltaTime * move);
        if (!isCheating)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 1.0f;
        }
        

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && canJump)
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        //Dash if the player is allowed to dash and presses the dash jump button again while in the air
        else if (Input.GetButtonDown("Jump") && dashingAllowed && !isGrounded && !isDashing && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
        if (Input.GetButtonUp("Jump"))
        {
            canJump = false; //This is to prevent the player from jumping multiple times in the air
        }
        CrouchHandler();
        StateHandler();
        DieWhenOutOfMap(); //If the player falls out of the map, they die
        CheatHandler(); //If the player presses "T", they can turn on cheats
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, chromaticTargetIntensity, chromaticChangeSpeed * Time.deltaTime);
    }

    public void IncreaseDeathsSinceLastCheckpoint()
    {
        deathsSinceLastCheckpoint++;
    }

    public void ResetDeathsSinceLastCheckpoint()
    {
        deathsSinceLastCheckpoint = 0;
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
            canDash = true;
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

    private void CheatHandler()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isCheating = !isCheating;
            if (isCheating)
            {
                //Show a message telling the player that they are cheating, and that they can turn off cheats by pressing "T" again (this is done in DashingMessage.cs)
                DashingMessage dashingMessage = GameObject.Find("DashingMessage").GetComponent<DashingMessage>();
                dashingMessage.DisplayMessage("Cheats enabled!<br>Press \"T\" again to turn off cheats", true, 5);
                hasCheated = true;
            }
            else
            {
                //Show a message telling the player that they are no longer cheating (this is done in DashingMessage.cs)
                DashingMessage dashingMessage = GameObject.Find("DashingMessage").GetComponent<DashingMessage>();
                dashingMessage.DisplayMessage("Cheats disabled!", true, 2);
            }
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        //chromaticAberration.active = true;
        chromaticTargetIntensity = chromaticMaxIntensity;
        float startTime = Time.time; // need to remember this to know how long to dash
        var dashDirection = transform.forward;
        //Give a small upwards impulse to the player
        velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight / 4);
        while (Time.time < startTime + dashTime)
        {
            //IsGrounded and CheckSphere is almost the same, but CheckSphere is more accurate, and isGrounded is
            //better for detecting when the player is near an edge. So we use both..
            Vector3 groundPosition = transform.position - new Vector3(0, transform.localScale.y, 0);

            //If the player touches the ground while dashing, stop dashing
            if (Physics.CheckSphere(groundPosition, groundDistance, groundMask) || isGrounded)
            {
                break; // break out of the while loop, stop dashing
            }
            

            controller.Move(dashSpeed * Time.deltaTime * dashDirection);
            yield return null; // this will make Unity stop here and continue next frame
        }
        //chromaticAberration.active = false;
        chromaticTargetIntensity = chromaticMinIntensity;
        isDashing = false;
        canDash = false; //The player can't dash again until they jump again
    }


    public void AddVelocity(Vector3 v)
    {
        velocity += v;
    }

    public void RemoveVelocity(Vector3 v)
    {
        velocity -= v;
    }


    //Should be called when the player dies
    public void OnDeath()
    {
        transform.position = SpawnPoint;
        velocity = Vector3.zero; //Reset the velocity to make sure the player doesn't keep moving after they die
        deaths++;
        deathsSinceLastCheckpoint++;
        Debug.Log("Player deaths: "+deaths+"; since last checkpoint:" + deathsSinceLastCheckpoint );
        //If the player has died 5 times since the last checkpoint, show the cheat prompt
        if (deathsSinceLastCheckpoint >= 5)
        {
            ShowCheatPrompt();
            cheatPromptShown = true;
        }
    }

    //If the player falls out of the map, they die (this is called in Update)
    public void DieWhenOutOfMap()
    {
        if (transform.position.y < -10)
        {
            OnDeath();
        }
    }

    // This method is called when the controller collides with another collider
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "DeadlyCube")
        {
            //Get the DeadlyCubeScript component of the hit collider
            DeadlyCubeScript deadlyCubeScript = hit.collider.GetComponent<DeadlyCubeScript>();

            // Check if the object is deadly
            if (deadlyCubeScript.IsDeadly)
            {
                OnDeath();
            }
        }
    }
    //Use DashingMessage.DisplayMessage to display a message on the screen
    private void ShowCheatPrompt()
    {
        if (cheatPromptShown)
        {
            return;
        }
        DashingMessage dashingMessage = GameObject.Find("DashingMessage").GetComponent<DashingMessage>();
        dashingMessage.DisplayMessage("Struggling with an obstacle?<br>You can cheat by pressing \"T\"!<br>This will temporarily turn off gravity", true, 12);

    }




}

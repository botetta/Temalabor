using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class MouseLook : MonoBehaviour
{
    //Objects
    public Transform playerBody;
    public new Camera camera;
    public PlayerMovementScript playerMovementScript;

    [Header("Values")] //Public variables
    public float FOV;
    public float mouseSensitivity;

    //FOV Private variables
    private readonly float sprintFovMultiplier = 1.25f; 
    private readonly float dashFovMultiplier = 1.5f;
    private float sprintFov;
    private float dashFov;
    //How fast the FOV changes
    private float fovSpeed = 20f;

    private float xRotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
        Application.targetFrameRate = 165;
        // Calculate the sprint FOV
        sprintFov = FOV * sprintFovMultiplier;
        dashFov = FOV * dashFovMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        FovHandler();
    }

    private void FovHandler()
    {
        // If the player is crouching, they can't sprint so don't change the FOV
        if (playerMovementScript.state == PlayerMovementScript.MovementState.Crouching)
        {
            return;
        }
        // Check if the player is sprinting
        bool isSprinting = Input.GetButton("Sprint");
        // Check if the player is dashing
        bool isDashing = playerMovementScript.IsDashing;

        //If else statement for setting target fov: Check dashing first, then sprinting, then default
        float targetFov;
        if (isDashing)
        {
            targetFov = dashFov;
        }
        else if (isSprinting)
        {
            targetFov = sprintFov;
        }
        else
        {
            targetFov = FOV;
        }

        // Smoothly interpolate the current FOV to the target FOV
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, fovSpeed * Time.deltaTime);
    }
}

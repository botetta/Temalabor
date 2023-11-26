using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    PlayerMovementScript playerMovementScript;
    private void Start()
    {
        // Get the reference to the parent game object of the Capsule, which is the First Person Player
        GameObject firstPersonPlayer = transform.parent.gameObject;

        // Get the reference to PlayerMovementScript using GetComponent
        playerMovementScript = firstPersonPlayer.GetComponent<PlayerMovementScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            print("Player is touching moving platform");
            playerMovementScript.OnDeath();
        }
    }


}

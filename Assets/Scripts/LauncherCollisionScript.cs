using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherCollisionScript : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovementScript>().AddVelocity(velocity);
        }
    }
}
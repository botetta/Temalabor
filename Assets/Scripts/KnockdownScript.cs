using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockdownScript : MonoBehaviour
{
    //[SerializeField] private Vector3 velocity;
    [SerializeField] private GameObject obstacle;
    private KnockdownObstacleLogic obstacleLogic;

    private void Start()
    {
        obstacleLogic = obstacle.GetComponent<KnockdownObstacleLogic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovementScript>().AddVelocity(obstacleLogic.Direction * obstacleLogic.Velocity * 3);
        }
    }
}

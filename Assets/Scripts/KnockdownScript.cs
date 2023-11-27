using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockdownScript : MonoBehaviour
{
    //[SerializeField] private Vector3 velocity;
    [SerializeField] private GameObject obstacle;
    private KnockdownObstacleLogic obstacleLogic;
    private PlayerMovementScript playerMovement;
    private Vector3 direction;

    private void Start()
    {
        obstacleLogic = obstacle.GetComponent<KnockdownObstacleLogic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerMovement = other.gameObject.GetComponent<PlayerMovementScript>();
            //Get the direction from the obstacle to the player
            direction = obstacleLogic.Direction * obstacleLogic.Velocity * 3;
            //Start the coroutine to knockdown the player
            StartCoroutine(KnockdownCoroutine());
            
        }
    }

    //Coroutine to AddVelocity to the player, then wait for a bit (0.5 seconds), then remove the velocity
    private IEnumerator KnockdownCoroutine()
    {
        Debug.Log("KnockdownCoroutine");
        //Add the velocity to the player
        playerMovement.AddVelocity(direction);
        //Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        playerMovement.RemoveVelocity(direction);

    }
}

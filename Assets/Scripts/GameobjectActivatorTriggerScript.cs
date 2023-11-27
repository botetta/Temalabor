using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectActivatorTriggerScript : MonoBehaviour
{
    [Tooltip("List of gameobjects to activate or deactivate")]
    public List<GameObject> gameObjects = new();

    [Tooltip("Whether or not the gameobjects should be activated or deactivated.\nticked: activate game objects, unticked: deactivate game objects")]
    public bool activate = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        //Check if the player entered the trigger
        if (other.gameObject.tag == "Player")
        {
            //Activate or deactivate the gameobjects
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(activate);
            }
        }
    }
}

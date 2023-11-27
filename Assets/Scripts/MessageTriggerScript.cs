using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTriggerScript : MonoBehaviour
{
    [Tooltip("The message to display when the player enters the trigger")]
    public string messageToDisplay;

    [Tooltip("Whether or not the message is informational (true) or a thought from the character (false)")]
    public bool informational = true;

    [Tooltip("How long the message should be displayed")]
    public float timeToDisplay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This function is called when the player enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        //Check if the player entered the trigger
        if (other.gameObject.tag == "Player")
        {
            //Disable the trigger
            gameObject.SetActive(false);
            DashingMessage dashingMessage = GameObject.Find("DashingMessage").GetComponent<DashingMessage>();
            dashingMessage.DisplayMessage(messageToDisplay, informational, timeToDisplay);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking.PlayerConnection;

public class DashingMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private float timeToDisplay;
    

    /* Displays a message on the screen for a short time (message is passed as a string in "message")
     * Informational is true if the message is a tip on how to play the game, false if it's a thought from the character (By default it's true)
     * Time to display is how long the message should be displayed (By default it's 2 seconds)
    */
    public void DisplayMessage(string message, bool informational = true, float timeToDisplay = 2f)
    {
        text.text = message;

        if (informational)
        {
            text.color = new Color(0.35f, 0.75f, 0.35f); //Green
        }
        else
        {
            text.color = new Color(0.5f, 0.5f, 0.9f); //Blue
        }
        //Stop the coroutine if it's already running
        StopAllCoroutines();
       
        StartCoroutine(DisplayMessageCoroutine());
        this.timeToDisplay = timeToDisplay;
    }

    private IEnumerator DisplayMessageCoroutine()
    {
        //Reset the position of the text
        text.transform.position = new Vector3(518, 350, 0);
       
        //Make the text slowly fade in
        text.alpha = 0;
        text.enabled = true;
        //Also move the text upwards slightly
        while (text.alpha < 1)
        {
            text.transform.position += new Vector3(0, 0.2f, 0);
            
            text.alpha += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(timeToDisplay);

        //Make the text slowly fade out
        while (text.alpha > 0)
        {     
            text.alpha -= Time.deltaTime * 3;
            yield return null;
        }
        text.enabled = false;
    }


}

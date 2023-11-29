using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsTriggerScript : MonoBehaviour
{

    private bool creditsPlayed = false;

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
        if (other.gameObject.tag == "Player" && !creditsPlayed)
        {
            creditsPlayed = true;
            StartCoroutine(PlayCredits());
        }
    }

    private IEnumerator PlayCredits()
    {
        //we need to get the PlayerMovement script and get the amount of deaths
        var playerMovement = GameObject.Find("First Person Player").GetComponent<PlayerMovementScript>();
        var deaths = playerMovement.Deaths;
        //get the time it took to complete the game
        var time = Time.timeSinceLevelLoad;
        //time is in seconds, but we want it in minutes and seconds
        var minutes = (int)time / 60;
        var seconds = (int)time % 60;

        //Only save the time and deaths if the player hasn't cheated, and also if the player has a lower time or death count than the current high score
        if (!playerMovement.HasCheated)
        {
            //Get the current high score
            var minutesHighScore = PlayerPrefs.GetInt("Minutes", -1);
            var secondsHighScore = PlayerPrefs.GetInt("Seconds", -1);
            var deathsHighScore = PlayerPrefs.GetInt("Deaths", -1);

            //Check if the player has a lower time than the current high score
            if (minutesHighScore == -1 || (minutes < minutesHighScore || (minutes == minutesHighScore && seconds < secondsHighScore)))
            {
                //Save the time it took to complete the game to PlayerPrefs (minutes and seconds)
                PlayerPrefs.SetInt("Minutes", minutes);
                PlayerPrefs.SetInt("Seconds", seconds);
            }
            //Check if the player has a lower death count than the current high score
            if (deathsHighScore == -1 || deaths < deathsHighScore)
            {
                //Save the amount of deaths to PlayerPrefs
                PlayerPrefs.SetInt("Deaths", deaths);
            }
        }


        yield return new WaitForSeconds(3f);
        //Slowly fade the screen to black. To do this, we have a black panel in the canvas that we can make visible slowly by changing its alpha value
        var blackPanel = GameObject.Find("BlackPanel");

        //Get the image component of the black panel
        var image = blackPanel.GetComponent<UnityEngine.UI.Image>();

        while (image.color.a < 1)
        {
            image.color += new Color(0, 0, 0, Time.deltaTime / 3);
            yield return null;
        }
        yield return new WaitForSeconds(1f);


       
        //Play credits
        //Use the DashingMessage script to display the credits
        DashingMessage dashingMessage = GameObject.Find("DashingMessage").GetComponent<DashingMessage>();
        dashingMessage.DisplayMessage("Beat The beat", true, 5);
        //We need to wait for the message to be displayed before we can display the next one. We always add 2 seconds to the wait time to account for the time it takes to display the message
        yield return new WaitForSeconds(5f+2f);

        dashingMessage.DisplayMessage("Thank you for playing <3", true, 3);
        yield return new WaitForSeconds(3f + 2f);

        //Check if the player has cheated (HasCheated)
        if (playerMovement.HasCheated)
        {
            dashingMessage.DisplayMessage($"You completed the game in {deaths} deaths<br>With Cheats", true, 5);
        }
        else
        {
            dashingMessage.DisplayMessage($"You completed the game in {deaths} deaths", true, 5);
        }
        yield return new WaitForSeconds(5f + 2f);

        if (playerMovement.HasCheated)
        {
            dashingMessage.DisplayMessage($"You completed the game in {string.Format("{0:00}:{1:00}", minutes, seconds)}<br>With Cheats", true, 5);
        }
        else
        {
            dashingMessage.DisplayMessage($"You completed the game in {string.Format("{0:00}:{1:00}", minutes, seconds)}", true, 5);
        }

        yield return new WaitForSeconds(5f + 2f);

        //Show a message if the player has cheated that their time and deaths won't be saved
        if (playerMovement.HasCheated)
        {
            dashingMessage.DisplayMessage("You cheated, so your time and deaths won't be saved", true, 5);
            yield return new WaitForSeconds(5f + 2f);
        }

        dashingMessage.DisplayMessage("Music:<br>Gergely Mátyás", true, 2);
        yield return new WaitForSeconds(2f+2f);
        dashingMessage.DisplayMessage("Character Movement:<br>Hölbling Botond", true, 2);
        yield return new WaitForSeconds(2f+2f);
        dashingMessage.DisplayMessage("Cassette System:<br>Gergely Mátyás", true, 2);
        yield return new WaitForSeconds(2f+2f);
        dashingMessage.DisplayMessage("Obstacle Design:<br>Hölbling Botond<br>Gergely Mátyás", true, 2);
        yield return new WaitForSeconds(2f+2f);
        dashingMessage.DisplayMessage("Level Design:<br>Hölbling Botond", true, 2);
        yield return new WaitForSeconds(2f+2f);



        


       
        //Fade out the music. They are stored in the "Music" game object
        var music = GameObject.Find("Music");
        var musicAudioSources = music.GetComponentsInChildren<AudioSource>();

        foreach (var audioSource in musicAudioSources)
        {
            Debug.Log("Fading out music");
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / 1.5f;
                yield return null;
            }
        }
        




        //Load the main menu scene
        PlayerMovementScript.GoToMainMenu();

    }


}

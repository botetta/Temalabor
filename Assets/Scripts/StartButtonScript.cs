using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartButtonScript : MonoBehaviour
{
    public TextMeshProUGUI lowestDeathsText;
    public TextMeshProUGUI lowestTimeText;
    //Method to start the game (Load Game Scene)
    public void StartGame()
    {
        //Load Game Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    //Method to quit the game
    public void QuitGame()
    {
        //Quit the game
        Application.Quit();
    }

    //Method to load the high scores. It is stored in player prefs (minutes, seconds, deaths)
    public void LoadHighScores()
    {
        //Get the high scores from player prefs
        var minutes = PlayerPrefs.GetInt("Minutes", -1);
        var seconds = PlayerPrefs.GetInt("Seconds", -1);
        var deaths = PlayerPrefs.GetInt("Deaths", -1);

        //Display the high scores
        if(minutes == -1)
        {
            lowestTimeText.text = "Fastest time: __:__";
        }
        else
        {
            lowestTimeText.text = ($"Fastest time: {string.Format("{0:00}:{1:00}", minutes, seconds)}");
        }
        if(deaths == -1)
        {
            lowestDeathsText.text = "Lowest amount of deaths: _";
        }
        else
        {
            lowestDeathsText.text = ($"Lowest amount of deaths: {deaths}");
        }
    }

    //Run the load high scores method when the scene starts
    private void Start()
    {
        LoadHighScores();
        //Unlock the cursor so the player can click on the buttons
        Cursor.lockState = CursorLockMode.None;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCasette : Casette
{
    //for OO, this is going to be ObstacleLogic instead of PlatformLogic
    [SerializeField]
    private PlatformLogic platformLogic;
    

    private void Start()
    {
        Init();
        
        platformLogic = GameObject.FindGameObjectWithTag("Disappearing Platforms").GetComponent<PlatformLogic>();
    }

    private void Update()
    {
        ParentUpdate();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            casette.SetActive(false);

            AudioSource track = playAudioScript.playTrack();

            float timeLeft = track.clip.length - track.time;
            float secPerBeat = 60 / platformLogic.Bpm;
            float currentBeats = track.time / secPerBeat;

            Debug.Log($"Time left: {timeLeft}\tSec per Beat: {secPerBeat}\tCurrent Beats: {currentBeats}\tBpm: {platformLogic.Bpm}");

            if ((currentBeats + 1) * secPerBeat >= track.clip.length)
            {
                Debug.Log($"Waiting full length, time left: {timeLeft}");
                platformLogic.Invoke("Enable", timeLeft);
            }
            else
            {
                Debug.Log($"Waiting until next beat, time left: {(currentBeats + 1) * secPerBeat - track.time}");
                platformLogic.Invoke("Enable", (currentBeats + 1) * secPerBeat - track.time);
            }
            
            
        }
    }
}

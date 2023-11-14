using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource track;
    [SerializeField]
    private GameObject syncTrackObject;
    private AudioSource syncTrack;

    private void Start()
    {   
        syncTrack = syncTrackObject.GetComponent<AudioSource>();
    }

    public AudioSource playTrack()
    {
        if (syncTrack != null && syncTrack.isPlaying)
        {
            track.time = syncTrack.time;
            track.Play();
        }
        else
        {
            throw new System.Exception("Synchronization track doesn't exist or is not playing");
        }
        
        Debug.Log($"Track played: {Time.time}");

        return track;
    }    
}

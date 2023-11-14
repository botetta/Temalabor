using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleLogic : MonoBehaviour
{
    [SerializeField] private float bpm;
    public float Bpm
    {
        get { return bpm; }
        set { value = bpm; }
    }

    protected AudioSource syncTrack;

    protected float secPerBeat;

    protected void Init()
    {
        secPerBeat = 60 / bpm;
        syncTrack = GameObject.FindGameObjectWithTag("SynchronizationTrack").GetComponent<AudioSource>();
    }

    protected float secondsUntilNextBeat()
    {
        float timeLeft = syncTrack.clip.length - syncTrack.time;
        float currentBeats = System.MathF.Floor(syncTrack.time / secPerBeat);

        if ((currentBeats + 1) * secPerBeat >= syncTrack.clip.length)
        {
            //Debug.Log($"Waiting full length, time left: {timeLeft}");
            return timeLeft;
        }
        else
        {
            //Debug.Log($"Waiting until next beat, time left: {(currentBeats + 1) * secPerBeat - syncTrack.time}");
            return (currentBeats + 1) * secPerBeat - syncTrack.time;
        }
    }
    abstract public void Enable();
}

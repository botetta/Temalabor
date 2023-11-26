using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchingPlatformLogic : ObstacleLogic
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject launchingPlatform;

    private Material originalMaterial;
    private int currentLocalBeat;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        platform.SetActive(true);
        launchingPlatform.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Enable()
    {
        float currentBeat = System.MathF.Floor(syncTrack.time / secPerBeat);
        currentLocalBeat = (int)(currentBeat + 1 )% 4 + 1;
        InvokeRepeating("Control", secondsUntilNextBeat(), secPerBeat);
        Debug.Log($"Invoked {Time.time}");
    }
    
    void Control()
    {
       if (currentLocalBeat == 1)
        {
            platform.SetActive(false);
            launchingPlatform.SetActive(true);
            currentLocalBeat++;
        }
       else
        {
            launchingPlatform.SetActive(false);
            platform.SetActive(true);
            currentLocalBeat++;
        }
       if (currentLocalBeat > 4)
        {
            currentLocalBeat = 1;
        }
    }
    
}

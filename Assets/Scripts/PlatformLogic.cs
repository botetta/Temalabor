using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformLogic : ObstacleLogic
{
    
    [SerializeField] private GameObject cube1;
    [SerializeField] private GameObject cube2;
    [SerializeField] private GameObject cube3;
    [SerializeField] private GameObject cube4;
    

    private int activeCube = 0;

    override public void Enable()
    {
        float currentBeat = System.MathF.Floor(syncTrack.time / secPerBeat);
        activeCube = (int) currentBeat % 4 + 1;
        InvokeRepeating("Control", secondsUntilNextBeat(), secPerBeat);
        //for a different rythm, call another invokeRepeating at a different delay
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        cube1.SetActive(false);
        cube2.SetActive(false);
        cube3.SetActive(false);
        cube4.SetActive(false);
    }

    void Control()
    {
        switch (activeCube)
        {
            case 1:
                cube2.SetActive(true);
                cube1.SetActive(false);
                activeCube = 2;
                break;
            case 2:
                cube3.SetActive(true);
                cube2.SetActive(false);
                activeCube = 3;
                break;
            case 3:
                cube4.SetActive(true);
                cube3.SetActive(false);
                activeCube = 4;
                break;
            case 4:
                cube1.SetActive(true);
                cube4.SetActive(false);
                activeCube = 1;
                break;
            default:
                Debug.Log($"First cube activated: {Time.time}");
                cube1.SetActive(true);
                activeCube = 1;
                break;
        }
    }
}

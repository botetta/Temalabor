using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakePlatformLogic : ObstacleLogic
{

    [SerializeField] private GameObject cube1;
    [SerializeField] private GameObject cube2;
    [SerializeField] private GameObject cube3;
    [SerializeField] private GameObject cube4;


    private bool[] activeCubes = new bool[4];
    private int currentLocalBeat = 0;

    override public void Enable()
    {
        float currentBeat = System.MathF.Floor(syncTrack.time / secPerBeat);
        currentLocalBeat = (int)(currentBeat % 4 + 1 ) * 2;
        InvokeRepeating("Control", secondsUntilNextBeat(), secPerBeat/2);
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
        for (int i = 0; i < activeCubes.Length; ++i) activeCubes[i] = false;
    }

    void Control()
    {
        for (int i = 0; i < activeCubes.Length; ++i) activeCubes[i] = false;

        switch (currentLocalBeat)
        {
            case 1:
                activeCubes[0] = true;
                currentLocalBeat = 2;
                break;
            case 2:
                activeCubes[0] = true;
                activeCubes[1] = true;
                currentLocalBeat = 3;
                break;
            case 3:
                activeCubes[0] = true;
                activeCubes[1] = true;
                activeCubes[2] = true;
                currentLocalBeat = 4;
                break;
            case 4:
                activeCubes[0] = true;
                activeCubes[1] = true;
                activeCubes[2] = true;
                activeCubes[3] = true;
                currentLocalBeat = 5;
                break;
            case 5:
                activeCubes[1] = true;
                activeCubes[2] = true;
                activeCubes[3] = true;
                currentLocalBeat = 6;
                break;
            case 6:
                activeCubes[2] = true;
                activeCubes[3] = true;
                currentLocalBeat = 7;
                break;
            case 7:
                activeCubes[3] = true;
                currentLocalBeat = 8;
                break;
            case 8:
                currentLocalBeat = 1;
                break;
            default:
                throw new System.Exception("currentLocalBeat is not 1-8");
        }

        cube1.SetActive(activeCubes[0]);
        cube2.SetActive(activeCubes[1]);
        cube3.SetActive(activeCubes[2]);
        cube4.SetActive(activeCubes[3]);
    }
}

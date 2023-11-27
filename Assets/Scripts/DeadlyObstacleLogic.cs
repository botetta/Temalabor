using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyObstacleLogic : ObstacleLogic
{
    //List of obstacles (cubes) that will be activated
    public List<GameObject> obstacles = new();

    public List<DeadlyCubeScript> deadlyCubeScripts = new();




    public override void Enable()
    {
        //Set the obstacles to be active
        foreach (var obstacle in obstacles)
        {
            obstacle.SetActive(true);
        }
        float currentBeat = System.MathF.Floor(syncTrack.time / secPerBeat);
       
        //Get the DeadlyCubeScript from each obstacle and store them in a list
        foreach (var obstacle in obstacles)
        {
            deadlyCubeScripts.Add(obstacle.GetComponent<DeadlyCubeScript>());
        }



        InvokeRepeating("Control", secondsUntilNextBeat(), secPerBeat * 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        //Set the obstacles to be inactive
        foreach (var obstacle in obstacles)
        {
            obstacle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Control()
    {
        //Call ChangeDeadly() on each DeadlyCubeScript
        foreach (var script in deadlyCubeScripts)
        {
            script.ChangeDeadly();
        }
    }

}

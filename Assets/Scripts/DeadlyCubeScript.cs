using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyCubeScript : MonoBehaviour
{
    public Material neutralMaterial;
    public Material deadlyMaterial;

    [Tooltip("Whether or not the cube should be deadly at the start of the game.")]
    public bool initiallyDeadly;

    public bool IsDeadly { get; private set; } = true;
    [Tooltip("Whether or not the cube should automatically change between deadly and not deadly.")]
    public bool changingDeadly;

    // Start is called before the first frame update
    void Start()
    {
        IsDeadly = initiallyDeadly;
        SetDeadly(IsDeadly);
        //Change whether the cube is deadly or not every 1 second, using invoke repeating
        // !!! This is just for testing, later this should be synced with the music !!!
        /*
        if (changingDeadly)
        {
            InvokeRepeating(nameof(ChangeDeadly), 1, 1);
        }
        */
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeDeadly()
    {
        //If the cube is deadly, make it not deadly
        if (IsDeadly)
        {
            SetDeadly(false);
        }
        //If the cube is not deadly, make it deadly
        else
        {
            SetDeadly(true);
        }
    }
    
    public void SetDeadly(bool deadly)
    {
        if (deadly)
        {
            //Set the material to the deadly material
            GetComponent<MeshRenderer>().material = deadlyMaterial;
            IsDeadly = true;
        }
        else
        {

            //Set the material to the neutral material
            GetComponent<MeshRenderer>().material = neutralMaterial;
            IsDeadly = false;
        }

    }



}

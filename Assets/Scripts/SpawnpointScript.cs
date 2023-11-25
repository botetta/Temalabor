using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointScript : MonoBehaviour
{
    //A list of all spawnpoint objects in the scene (tagged with "Spawnpoint")
    public static List<GameObject> Spawnpoints { get; private set; } = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Add all spawnpoints to the list if it is empty
        if (Spawnpoints.Count == 0)
        {
            //Find all spawnpoints in the scene
            GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
            //Add all spawnpoints to the list
            foreach (GameObject spawnpoint in spawnpoints)
            {
                Spawnpoints.Add(spawnpoint);
            }
        }

        //Log the number of spawnpoints
        Debug.Log("Number of spawnpoints: " + Spawnpoints.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When the player touches the spawn point, set the player's spawn point to the spawn point's position
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovementScript>().SpawnPoint = transform.position;
            //Set all other spawn points to inactive
            SetAllSpawnpointsInactive();
            //Set this spawn point to active
            SetActive();
        }
    }
    //Change the currently active spawnpoint's color to green
    public void SetActive()
    {
        //Set the color to 0FFF0F (green)
        GetComponent<Renderer>().material.color = new Color(0.0625f, 1, 0.0625f, 0.2890625f);
        //Get the parent object (the spawnpoint's parent is the spawnpoint group)
        GameObject parent = transform.parent.gameObject;
        //Set the point light to green
        parent.GetComponentInChildren<Light>().color = new Color(0.0625f, 1, 0.0625f, 1);
        //Find the child called "Base" and set it to green
        parent.transform.Find("Base").GetComponent<Renderer>().material.color = new Color(0.0625f, 1, 0.0625f, 1);
        //Find the particle system and set it to green
        var ps = parent.GetComponentInChildren<ParticleSystem>();
        var main = ps.main;
        main.startColor = new Color(0.0625f, 1, 0.0625f, 1);
    }

    //Change the color back to the original blueish color when it is no longer active
    public void SetInactive()
    {
        //Set the color to 0FA69C (blueish)
        GetComponent<Renderer>().material.color = new Color(15.0f/255.0f, 166.0f/255.0f, 156.0f/255.0f, 74.0f/255.0f);
        //Get the parent object (the spawnpoint's parent is the spawnpoint group)
        GameObject parent = transform.parent.gameObject;
        //Set the point light to blueish
        parent.GetComponentInChildren<Light>().color = new Color(67.0f/255.0f, 226.0f/255.0f, 218.0f/255.0f, 1);
        //Find the child called "Base" and set it to blueish
        parent.transform.Find("Base").GetComponent<Renderer>().material.color = new Color(33.0f / 255.0f, 97.0f / 255.0f, 101.0f / 255.0f, 1);
        //Find the particle system and set it to blueish
        var ps = parent.GetComponentInChildren<ParticleSystem>();
        var main = ps.main;
        main.startColor = new Color(56.0f/255.0f, 122.0f/255.0f, 173.0f/255.0f, 1);
    }

    public void SetAllSpawnpointsInactive()
    {
        foreach (GameObject spawnpoint in Spawnpoints)
        {
            spawnpoint.GetComponentInChildren<SpawnpointScript>().SetInactive();
        }
    }
}

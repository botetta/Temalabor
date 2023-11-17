using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCasette : Casette
{
    //for OO, this is going to be ObstacleLogic instead of PlatformLogic
    private List<ObstacleLogic> obstacleLogics = new List<ObstacleLogic>();

    [SerializeField]
    private List<GameObject> obstacles = new List<GameObject>();

    private void Start()
    {
        Init();

        for (int i = 0; i < obstacles.Count; ++i) {
            obstacleLogics.Add(obstacles[i].GetComponent<ObstacleLogic>());
        }
        
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

            foreach(var ol in obstacleLogics)
            {
                if (ol == null)
                {
                    throw new System.Exception("obstacle logic is null");
                    
                }
                ol.Enable();
            }


        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCasette : Casette
{
    //for OO, this is going to be ObstacleLogic instead of PlatformLogic
    private ObstacleLogic obstacleLogic;

    [SerializeField]
    private GameObject obstacle;

    private void Start()
    {
        Init();

        if (obstacle != null) obstacleLogic = obstacle.GetComponent<ObstacleLogic>();
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

            if (obstacleLogic == null) return;

            obstacleLogic.Enable();


        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class KnockdownObstacleLogic : ObstacleLogic
{

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private float travelTimeBeats;


    private bool moving = false;
    private bool forward = false;
    private bool accelerating = false;
    private Vector3 direction;
    float distance;
    private float travelTimeSeconds;
    float acceleration;
    float velocity;
    float traveled = 0;

    public float Velocity
    {
        get { return velocity; }
        private set { }
    }
    public Vector3 Direction
    {
        get { return direction;}
        private set { }
    }

    override public void Enable()
    {
        body.transform.position = startPoint.transform.position;
        acceleration = distance / Mathf.Pow(travelTimeSeconds / 2, 2);
        body.SetActive(true);
        forward = true;
        accelerating = true;
        float currentBeat = System.MathF.Floor(syncTrack.time / secPerBeat);
        Invoke("startMovement", secondsUntilNextBeat());
    }

    private void startMovement()
    {
        moving = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        body.SetActive(false);
        direction = Vector3.Normalize(endPoint.transform.position - startPoint.transform.position);
        //stance = Mathf.Sqrt(Vector3.Dot(endPoint.transform.position, endPoint.transform.position) + Vector3.Dot(startPoint.transform.position, startPoint.transform.position));
        distance = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
        travelTimeSeconds = travelTimeBeats * secPerBeat;
    }


    private void FixedUpdate()
    {
        if (!moving) return;

        if (accelerating)
        {
            velocity += acceleration * Time.fixedDeltaTime;
            if (traveled >= travelTimeSeconds / 2)
                accelerating = false;
        }
        else
        {
            velocity -= acceleration * Time.fixedDeltaTime;
            if (traveled >= travelTimeSeconds)
            {
                direction = -direction;
                accelerating = true;
                velocity = 0;
                traveled = 0;
            }
        }

        body.transform.position += direction * velocity * Time.fixedDeltaTime;
        traveled += Time.fixedDeltaTime;

    }


}

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoy : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float m_Speed = 10000f;


    /*
    // Update is called once per frame
    void Update()
    {
        transform.Translate(- Vector3.forward * Time.deltaTime);
    }
    */

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //Store user input as a movement vector
        Vector3 m_Input = new Vector3(0.0f, 0.0f, -1.0f);

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
    }

}

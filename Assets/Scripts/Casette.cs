using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Casette : MonoBehaviour
{
    [SerializeField] protected GameObject casette;
    [SerializeField] private float rotateSpeed = 100;
    [SerializeField] private float bobSpeed = 1;
    [SerializeField] private float bobAmplitude = 0.5f;
    private float t = 0;
    private float y;
    // Start is called before the first frame update

    protected PlayAudioScript playAudioScript;
    [SerializeField]
    private GameObject track;

    protected void Init()
    {
        y = casette.transform.position.y;
        if (track != null)
            playAudioScript = track.GetComponent<PlayAudioScript>();
        else
            playAudioScript = GameObject.FindGameObjectWithTag("SynchronizationTrack").GetComponent<PlayAudioScript>();
        
    }

    // Update is called once per frame
    protected void ParentUpdate()
    {
        t += bobSpeed * Time.deltaTime;
        casette.transform.position = new Vector3(casette.transform.position.x, y + bobAmplitude * ((float)System.Math.Sin(t)), casette.transform.position.z);
        transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime, Space.World);
    }
}

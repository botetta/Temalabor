using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashingMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float timeToDisplay = 2f;
    
    public void DisplayMessage()
    {
        StartCoroutine(DisplayMessageCoroutine());
    }

    private IEnumerator DisplayMessageCoroutine()
    {
        //Make the text slowly fade in
        text.alpha = 0;
        text.enabled = true;
        //Also move the text upwards slightly
        while (text.alpha < 1)
        {
            text.transform.position += new Vector3(0, 0.2f, 0);
            
            text.alpha += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(timeToDisplay);

        //Make the text slowly fade out
        while (text.alpha > 0)
        {     
            text.alpha -= Time.deltaTime * 3;
            yield return null;
        }
        text.enabled = false;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCasette : Casette
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        ParentUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Dashing is now enabled!");
            //Get the player movement script so we can enable dashing
            PlayerMovementScript playerMovement = other.gameObject.GetComponent<PlayerMovementScript>();
            playerMovement.dashingAllowed = true;
            casette.SetActive(false);
            //Find the dashing message and display it
            DashingMessage dashingMessage = GameObject.Find("DashingMessage").GetComponent<DashingMessage>();
            dashingMessage.DisplayMessage();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactivityDisconnect : MonoBehaviour {

    private float idleCounter = 0.0f;
    private Touch touch;
    public int DisconnectIdleTime = 900;
    void Update()
    {
        
        if (Input.anyKeyDown || touch.phase == TouchPhase.Began)
        {
            idleCounter = 0.0f;  // reset counter          
        }
        else
        {
            idleCounter += Time.deltaTime; // increment counter

            // Disconnect time is hardcoded and it must be changed
            if(idleCounter > DisconnectIdleTime)
            {
                //GameObject.Find("PhotonEngine").GetComponent<PhotonEngine>().Disconnect();
            }
        }

    }
}

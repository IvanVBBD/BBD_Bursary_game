using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTrigger : MonoBehaviour
{
    private uiManager uiControl;


    void Awake(){
        uiControl = GameObject.FindGameObjectWithTag("uiManager").GetComponent<uiManager>();
    }

    public void startNextChallenge(){    
        Debug.Log("Is this called?");  
        uiControl.toggleWinPanel();
    }

}

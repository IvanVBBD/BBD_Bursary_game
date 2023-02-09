using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTrigger : MonoBehaviour
{
    private uiManager uiControl;
    private pipeManager pipeControl;


    void Awake(){
        uiControl = GameObject.FindGameObjectWithTag("uiManager").GetComponent<uiManager>();
        pipeControl = GameObject.FindGameObjectWithTag("pipeManager").GetComponent<pipeManager>();
    }

    public void startNextChallenge(){    
        if(pipeControl.getSuccCon()){
            uiControl.toggleWinPanel();
        }
        else{
            Debug.Log("ANIMATION END TRIGGERED BUT WIN CON NOT MET");
        }
    }

}

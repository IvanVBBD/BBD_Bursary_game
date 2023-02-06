using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTrigger : MonoBehaviour
{
    private challengeManager challengeControl;


    void Awake(){
        challengeControl = GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>();
    }

    public void startNextChallenge(){    
        Debug.Log("Is this called?");  
        challengeControl.startChallenge();
    }

}

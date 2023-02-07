using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticLink : MonoBehaviour
{
    // Start is called before the first frame update
    challengeManager challengeControl;


    void Awake(){
        challengeControl = GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>();

    }
    void Start()
    {
        
    }

    public void staticNextLevelLink(){
        challengeControl.startChallenge();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

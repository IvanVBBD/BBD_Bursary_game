using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateTrigger : MonoBehaviour
{
    private animationManager animationControl;


    void Awake(){
        animationControl = GameObject.FindGameObjectWithTag("animationManager").GetComponent<animationManager>();
    }

    public void animationEnd(){
        int temp = this.gameObject.GetComponent<Animator>().GetInteger("SplitNumber");
        Debug.Log($"OMG OMG OMG OMG: {temp}");
        animationControl.triggerNextAnimations(this.gameObject);
    }
}

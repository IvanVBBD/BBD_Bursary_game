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
        animationControl.triggerNextAnimations(this.gameObject.transform.parent.gameObject);
    }
}

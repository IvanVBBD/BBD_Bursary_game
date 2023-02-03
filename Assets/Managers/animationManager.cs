using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    private List<Animator> toAnimate = new List<Animator>();
    private int toAnimateCounter = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void checkAnimationOver(){
        if(toAnimateCounter > 0){
            toAnimate[0].SetTrigger("StartFlow");
            Debug.Log(toAnimate[0].GetCurrentAnimatorClipInfo(0).Length);
            if (toAnimate[0].GetCurrentAnimatorStateInfo(0).normalizedTime > 2)
            {
                toAnimate[1].SetTrigger("StartFlow");
            }
        }
        //     if (toAnimate[0].GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        //     {
        //         toAnimate.RemoveAt(0);
        //         toAnimateCounter--;
                
        //     }
        //     else if(toAnimate[0].GetBool("CanFlow")){
        //         toAnimate[0].SetTrigger("StartFlow");
        //         toAnimate[0].SetBool("CanFlow", false);
        //     }
        
    }
    public void addToAnimate(GameObject pipe){
        toAnimate.Add(pipe.GetComponentInChildren<Animator>());
        toAnimateCounter++;

        if(toAnimateCounter > 0){
            toAnimate[0].SetTrigger("StartFlow");
            
        }
    }

    public void triggerNextAnimation(){
        toAnimate.RemoveAt(0);
        toAnimateCounter--;
        Debug.Log("OMG LOOK HERE");
        if(toAnimateCounter > 0){
            toAnimate[0].SetTrigger("StartFlow");
        }    
    }


}

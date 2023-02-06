using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    private List<List<Animator>> toAnimate = new List<List<Animator>>();
    private List<int> toAnimateCounter = new List<int>();
    private int mySplitCounter = 0;
    private bool firstAnimationCheck = false;
    gridManager gridControl;
    private int boardHeight;
    private Vector2 startPos;
    private int[,] board; 
    private bool[,] animationBoard;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }
        void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // void checkAnimationOver(){
    //     if(toAnimateCounter > 0){
    //         toAnimate[0].SetTrigger("StartFlow");
    //         Debug.Log(toAnimate[0].GetCurrentAnimatorClipInfo(0).Length);
    //         if (toAnimate[0].GetCurrentAnimatorStateInfo(0).normalizedTime > 2)
    //         {
    //             toAnimate[1].SetTrigger("StartFlow");
    //         }
    //     }        
    // }
    public void addToAnimate(GameObject pipe, int splitCounter){
        
        // Debug.Log($"SPLIT COUNTER: {splitCounter} + MY SPLIT COUNTER: {mySplitCounter}");

        if(!firstAnimationCheck){
            toAnimate.Add(new List<Animator>());
            toAnimateCounter.Add(1);
            toAnimate[0].Add(pipe.GetComponentInChildren<Animator>());
            toAnimate[0][0].SetInteger("SplitNumber", splitCounter);
            toAnimate[0][0].SetTrigger("StartFlow");
            firstAnimationCheck = true;
            //mySplitCounter++;

        }
        else if(splitCounter > mySplitCounter)
        {   
            toAnimate.Add(new List<Animator>());
            toAnimateCounter.Add(1);
            toAnimate[splitCounter].Add(pipe.GetComponentInChildren<Animator>());
            toAnimate[splitCounter][0].SetInteger("SplitNumber", splitCounter);

            mySplitCounter++;


        }else{
            toAnimate[splitCounter].Add(pipe.GetComponentInChildren<Animator>());
            toAnimate[splitCounter].Last().SetInteger("SplitNumber", splitCounter);
            toAnimateCounter[splitCounter]++;
        }
    }

    public void triggerNextAnimation(int split){

        // int count = 0;
        // if(split == 0){
        //     for(int i = 0; i <= mySplitCounter; i++){
        //         if(count > 0){
        //             if(toAnimateCounter[0] - toAnimateCounter[i] == 0)
        //             {
        //                 toAnimate[count].RemoveAt(0);
        //                 toAnimateCounter[count]--;

        //                 if(toAnimateCounter[count] > 0){
        //                     toAnimate[count][0].SetTrigger("StartFlow");
        //                 }    

        //                 toAnimate[0].RemoveAt(0);
        //                 toAnimateCounter[0]--;      
                        
        //                 if(toAnimateCounter[0] > 0){
        //                     toAnimate[0][0].SetTrigger("StartFlow");
        //                 }   
        //             }
        //             else{
        //                 if(toAnimateCounter[0] > 0){
        //                     toAnimate[0].RemoveAt(0);
        //                     toAnimateCounter[0]--;      
                            
        //                     if(toAnimateCounter[0] > 0){
        //                         toAnimate[0][0].SetTrigger("StartFlow");
        //                     }     
        //                 }
        //             }
        //         }else{
        //             if(toAnimateCounter[0] > 0){
        //                 toAnimate[0].RemoveAt(0);
        //                 toAnimateCounter[0]--;      
                        
        //                 if(toAnimateCounter[0] > 0){
        //                     toAnimate[0][0].SetTrigger("StartFlow");
        //                 }     
        //             }
        //         }
        //         count++;
        //     }
        // }
    }

    public void transverseAnimations(){
        boardHeight = gridControl.returnBoardHeight();
        board = new int[boardHeight, boardHeight];
        animationBoard = new bool[boardHeight, boardHeight]; // Should set all values to false by default

        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);
        startPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
        animationBoard[(int)startPos.x, (int)startPos.y] = true;

        Vector2 currentPos = startPos + Vector2.right;
        GameObject currentPiece = gridControl.returnBoardObject(currentPos);

        if(currentPiece == null){

        }
        else if (currentPiece.gameObject.tag == "end"){
            // Add end animation here with different triggers/bools for each end state (i.e. fail, win, semi-success, etc.)
        }
        else if(currentPiece.gameObject.tag == "pipe"){

            currentPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
            animationBoard[(int)currentPos.x, (int)currentPos.y] = true;
            triggerNextAnimations(currentPiece);

        }
    }

    public void triggerNextAnimations(GameObject currentPiece){
    
        Debug.Log("I'm BEING TRIGGERED RN");

        Vector2[] connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
        
        foreach(Vector2 connectingPoint in connectingPoints){

            Vector2 currentPos = currentPiece.gameObject.transform.position;
            Vector2 nextPos = currentPos + connectingPoint;

            Debug.Log($"Current Pos: {currentPos}");
            Debug.Log($"Next Pos: {nextPos}");
            Debug.Log(animationBoard[(int)nextPos.x, (int)nextPos.y]);
            if(animationBoard[(int)nextPos.x, (int)nextPos.y] == false){

                Debug.Log("TRY 1");

                GameObject tempPiece = gridControl.returnBoardObject(nextPos);
                Debug.Log("TRY 2");

                if(tempPiece != null){
                    Debug.Log("TRY 3");

                    tempPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                    Debug.Log("TRY 4");

                    animationBoard[(int)nextPos.x, (int)nextPos.y] = true;

                }

            }
            else{
                Debug.Log("Say sike rn");
            }    


        }

        
    }
}

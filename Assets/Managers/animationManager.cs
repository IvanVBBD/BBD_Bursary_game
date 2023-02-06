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
        else if(currentPiece.gameObject.tag == "pipe"){

            currentPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
            animationBoard[(int)currentPos.x, (int)currentPos.y] = true;
            triggerNextAnimations(currentPiece);

        }
    }

    public void triggerNextAnimations(GameObject currentPiece){
        // NEed to check if it's the end piece here


        Debug.Log("I'm BEING TRIGGERED RN");

        Vector2[] connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
        
        foreach(Vector2 connectingPoint in connectingPoints){

            Vector2 currentPos = currentPiece.gameObject.transform.position;
            Vector2 nextPos = currentPos + connectingPoint;

            Debug.Log($"Current Pos: {currentPos}");
            Debug.Log($"Next Pos: {nextPos}");
            Debug.Log(animationBoard[(int)nextPos.x, (int)nextPos.y]);

            if(animationBoard[(int)nextPos.x, (int)nextPos.y] == false){

                GameObject tempPiece = gridControl.returnBoardObject(nextPos);

                if(tempPiece != null){
                    if(tempPiece.gameObject.tag == "end"){
                        tempPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                        Debug.Log("END!");
                    }else if(tempPiece.GetComponent<pipe>().getConnectedStatus()){
                        tempPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                        animationBoard[(int)nextPos.x, (int)nextPos.y] = true;
                    }

                }

            }
            else{
                Debug.Log("Say sike rn");
            }    
            


        }

        
    }
}

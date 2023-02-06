using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    gridManager gridControl;
    private int boardHeight;
    private Vector2 startPos;
    private bool[,] animationBoard;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }

    public void transverseAnimations(){
        boardHeight = gridControl.returnBoardHeight();
        animationBoard = new bool[boardHeight, boardHeight]; // Should set all values to false by default

        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);
        startPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
        animationBoard[(int)startPos.x, (int)startPos.y] = true;

        Vector2 currentPos = startPos + Vector2.right;
        GameObject currentPiece = gridControl.returnBoardObject(currentPos);

        if(currentPiece != null && currentPiece.gameObject.tag == "pipe"){

            currentPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
            animationBoard[(int)currentPos.x, (int)currentPos.y] = true;
            triggerNextAnimations(currentPiece);
        }
    }

    public void triggerNextAnimations(GameObject currentPiece){
        Vector2[] connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
        
        foreach(Vector2 connectingPoint in connectingPoints){
            Vector2 currentPos = currentPiece.gameObject.transform.position;
            Vector2 nextPos = currentPos + connectingPoint;

            if(animationBoard[(int)nextPos.x, (int)nextPos.y] == false){
                GameObject tempPiece = gridControl.returnBoardObject(nextPos);
                if(tempPiece != null){
                    if(tempPiece.gameObject.tag == "end"){
                        tempPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                        // JESSE TO DO: 
                        // Check if game was won
                        // Trigger next scene 
                    }else if(tempPiece.GetComponent<pipe>().getConnectedStatus()){
                        tempPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                        animationBoard[(int)nextPos.x, (int)nextPos.y] = true;
                    }
                }
            }
            else{
                // JESSE TO DO: 
                // Check if need to change state/type of water and restart the animation loop
            }    
        }
    }
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    gridManager gridControl;
    pipeManager pipeControl;
    private int boardHeight;
    private Vector2 startPos;
    private bool[,] animationBoard;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
        pipeControl = GameObject.FindGameObjectWithTag("pipeManager").GetComponent<pipeManager>();
    }

    public void transverseAnimations(){
        boardHeight = gridControl.returnBoardHeight();
        animationBoard = new bool[boardHeight, boardHeight]; // Should set all values to false by default

        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);
        startPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
        animationBoard[(int)startPos.x, (int)startPos.y] = true;
    }

    public void triggerNextAnimations(GameObject currentPiece){
        Vector2[] connectingPoints;

        if(currentPiece.gameObject.tag == "start"){
            connectingPoints = new[] { Vector2.right };            
        }
        else{
            connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
        }
        foreach(Vector2 connectingPoint in connectingPoints){
            Vector2 currentPos = currentPiece.gameObject.transform.position;
            Vector2 nextPos = currentPos + connectingPoint;

            if(animationBoard[(int)nextPos.x, (int)nextPos.y] == false){
                GameObject tempPiece = gridControl.returnBoardObject(nextPos);
                if(tempPiece != null){
                    if(tempPiece.gameObject.tag == "end"){
                        if(pipeControl.getSuccCon()){
                            tempPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                        }
                        else{
                            Debug.Log("DID NOT WIN!");
                        }
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
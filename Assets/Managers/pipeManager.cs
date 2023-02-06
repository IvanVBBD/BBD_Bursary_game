using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeManager : MonoBehaviour
{
    private gridManager gridControl; 
    private waterManager waterControl;
    private inventoryManager inventoryControl;
    private animationManager animationControl;
    private bool succCon = false;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
        waterControl = GameObject.FindGameObjectWithTag("waterManager").GetComponent<waterManager>();
        inventoryControl = GameObject.FindGameObjectWithTag("inventoryManager").GetComponent<inventoryManager>();
        animationControl = GameObject.FindGameObjectWithTag("animationManager").GetComponent<animationManager>();
    }

    public void beginTrans(){
        succCon = false;
        Vector2 foundStart = gridControl.returnStartPosition();
        waterSpace.waterObject water = waterControl.issueFreshWaterState();
        tranverse(Vector2.right,foundStart,water);
        animationControl.transverseAnimations();
        if(succCon == true){
            Debug.Log("YAAAAS U WIN");
        }else{
            Debug.Log("Failed End");
        }
    }

     void tranverse(Vector2 change, Vector2 currentPos, waterSpace.waterObject water){
        Vector2 oldPos = currentPos;
        currentPos = new Vector2((int)(currentPos.x + change.x),(int)(currentPos.y + change.y));
        GameObject currentPiece = gridControl.returnBoardObject(currentPos);
        if(currentPiece == null){
            return;
        }else if (currentPiece.gameObject.tag == "end"){
            if(water.waterDirtState == 0f && water.waterPhaseState == waterSpace.waterStates.WATER){
                succCon = true;
            }
            if(GameObject.FindGameObjectWithTag("challengeManager")){
                // This has been moved to the end piece's animation event trigger 
                // GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>().startChallenge();
            }
            return;
        }else if (currentPiece.gameObject.tag == "pipe"){
            // Get stuff off object and call transverse again
            Vector2[] connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
            water = waterControl.alterWaterPhaseState(currentPiece.GetComponent<pipe>().returnPipeEffect(),water);

            // Validating pipe connection section of algorithm
            Vector2 connectingPostion = Vector2.zero; // init
            foreach(Vector2 pointSet in connectingPoints){
                if (new Vector2((int)(currentPos.x + pointSet.x),(int)(currentPos.y + pointSet.y)) == oldPos){
                    connectingPostion = pointSet;// new Vector2((int)(currentPos.x + pointSet.x),(int)(currentPos.y + pointSet.y));
                }
            }
            
            //This section deals with transversing out all possible ends of the pipe that is not the entry side
            foreach(Vector2 element in connectingPoints){
                if(element != connectingPostion && connectingPostion != Vector2.zero){
                    if(waterControl.canMoveDirection(element,water)){
                        if(connectingPoints.Length > 2 && currentPiece.GetComponent<pipe>().returnIsBalanceSplitter()){
                            Vector2[] combinedData = currentPiece.GetComponent<pipe>().returnPipeBalanceDirections();
                            Vector2 cleanDirection = combinedData[0];
                            Vector2 dirtyDirection = combinedData[1];
                            if(water.waterDirtState <= 0 && cleanDirection == element){
                                currentPiece.GetComponent<pipe>().setConnectedStatus();
                                tranverse(element,currentPos,water); // Go down clean split because water is clean
                            }else if(water.waterDirtState > 0 && dirtyDirection == element){
                                currentPiece.GetComponent<pipe>().setConnectedStatus();
                                tranverse(element,currentPos,water); // Go down dirty split because water is dirty
                            }
                        }else{
                            currentPiece.GetComponent<pipe>().setConnectedStatus();
                            tranverse(element,currentPos,water); // No split
                        }
                    }else{
                        currentPiece.GetComponent<pipe>().setConnectedStatus();
                    }
                }
            }
            return;
        }
        // Should never reach here
        return;
    }

    //Spawn pipe section
    public void spawnPipe(string type){
        GameObject currentPipe = inventoryControl.requestPipeSpawn(type);
        if(currentPipe == null){
            Debug.Log($"Cannot spawn {type} as type has reached its spawn limit!");
        }else{
            currentPipe.GetComponent<dragAndDrop>().setFirstPickup();
            //You now have reference to the object that spawns;
       }
    }
}
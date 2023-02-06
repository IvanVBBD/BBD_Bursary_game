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
    private int splitCounter = 0;
    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
        waterControl = GameObject.FindGameObjectWithTag("waterManager").GetComponent<waterManager>();
        inventoryControl = GameObject.FindGameObjectWithTag("inventoryManager").GetComponent<inventoryManager>();
        animationControl = GameObject.FindGameObjectWithTag("animationManager").GetComponent<animationManager>();
    }
    void Update()
    {

    }

    public void beginTrans(){
        succCon = false;
        Vector2 foundStart = gridControl.returnStartPosition();
        // Debug.Log("Starting Trans");
        waterSpace.waterObject water = waterControl.issueFreshWaterState();
        tranverse(Vector2.right,foundStart,water);
        animationControl.transverseAnimations();
        if(succCon == true){
            Debug.Log("YAAAAS U WIN");
        }else{
            Debug.Log("Failed End");
        }
    }

    void addAnimationToList(GameObject currentPiece, int s){
        animationControl.addToAnimate(currentPiece, s);
        // Debug.Log("START ANIMATION");


    }

     void tranverse(Vector2 change, Vector2 currentPos, waterSpace.waterObject water){
        Vector2 oldPos = currentPos;
        currentPos = new Vector2((int)(currentPos.x + change.x),(int)(currentPos.y + change.y));
        GameObject currentPiece = gridControl.returnBoardObject(currentPos);
        if(currentPiece == null){
            return;
        }else if (currentPiece.gameObject.tag == "end"){
            gridControl.returnBoardObject(oldPos).gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            if(water.waterDirtState == 0f && water.waterPhaseState == waterSpace.waterStates.WATER){
                succCon = true;
            }
            if(GameObject.FindGameObjectWithTag("webManager")){
                GameObject.FindGameObjectWithTag("webManager").GetComponent<webManager>().startGame();
            }
            return;
        }else if (currentPiece.gameObject.tag == "pipe"){
            //get stuff off object and call transverse again
            Vector2[] connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
            water = waterControl.alterWaterPhaseState(currentPiece.GetComponent<pipe>().returnPipeEffect(),water);

            ///Validating pipe connection section of algorithim
            Vector2 connectingPostion = Vector2.zero; // init
            foreach(Vector2 pointSet in connectingPoints){
                if (new Vector2((int)(currentPos.x + pointSet.x),(int)(currentPos.y + pointSet.y)) == oldPos){
                    connectingPostion = pointSet;// new Vector2((int)(currentPos.x + pointSet.x),(int)(currentPos.y + pointSet.y));
                }
            }
            int localSplitCounter = 0;
            //This section deals with transversing out all possible ends of the pipe that is not the entry side
            foreach(Vector2 element in connectingPoints){
                // Debug.Log($"LOCAL SPLIT COUNTER: {localSplitCounter}");
                if(element != connectingPostion && connectingPostion != Vector2.zero){
                    if(waterControl.canMoveDirection(element,water)){
                        if(connectingPoints.Length > 2 && currentPiece.GetComponent<pipe>().returnIsBalanceSplitter()){
                            Vector2[] combinedData = currentPiece.GetComponent<pipe>().returnPipeBalanceDirections();
                            Vector2 cleanDirection = combinedData[0];
                            Vector2 dirtyDirection = combinedData[1];
                            if(water.waterDirtState <= 0 && cleanDirection == element){
                                splitCounter++;
                                addAnimationToList(currentPiece, splitCounter);
                                Debug.Log($"SPLIT: {splitCounter}");
                                tranverse(element,currentPos,water); // Go down clean split because water is clean
                            }else if(water.waterDirtState > 0 && dirtyDirection == element){
                                splitCounter++;
                                addAnimationToList(currentPiece, splitCounter);
                                tranverse(element,currentPos,water); // Go down dirty split because water is dirty
                            }
                        }else{
                            splitCounter = splitCounter + localSplitCounter;
                            addAnimationToList(currentPiece, splitCounter);
                            localSplitCounter++;
                            tranverse(element,currentPos,water); // No split
                        }
                    }else{
                        addAnimationToList(currentPiece, splitCounter); // No split
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




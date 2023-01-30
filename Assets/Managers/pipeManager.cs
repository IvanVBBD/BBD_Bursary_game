using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeManager : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 startPos = new Vector2(0,0); // need to plug into the start point;

    private gridManager gridControl; 
    private waterManager waterControl;
    private bool succCon = false;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
        waterControl = GameObject.FindGameObjectWithTag("waterManager").GetComponent<waterManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            beginTrans();
        }
    }

    void beginTrans(){
        succCon = false;
        Vector2 foundStart = gridControl.returnStartPosition();
        Debug.Log("Starting Trans");
        waterSpace.waterObject water = waterControl.issueFreshWaterState();
        tranverse(Vector2.right,foundStart,water);
        if(succCon == true){
            Debug.Log("YAAAAS U WIN");
        }else{
            Debug.Log("Failed End");
        }
    }

     void tranverse(Vector2 change, Vector2 currentPos, waterSpace.waterObject water){
        Vector2 oldPos = currentPos;
        Debug.Log($"old Postion: {oldPos}");
        currentPos = new Vector2((int)(currentPos.x + change.x),(int)(currentPos.y + change.y));
        Debug.Log($"New Postion: {currentPos}");
        GameObject currentPiece = gridControl.returnBoardObject(currentPos);
        if(currentPiece == null){
            Debug.Log("YUUUP its null");
            return;
        }else if (currentPiece.gameObject.tag == "end"){
            gridControl.returnBoardObject(oldPos).gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            if(water.waterDirtState == 0f && water.waterPhaseState == waterSpace.waterStates.WATER){
                succCon = true;
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

            //This section deals with transerving out all possible ends of the pipe that is not the entry side
            foreach(Vector2 element in connectingPoints){
                if(element != connectingPostion && connectingPostion != Vector2.zero){
                    Debug.Log($"chosen point {element}");
                    if(waterControl.canMoveDirection(element,water)){
                        if(connectingPoints.Length > 2 && currentPiece.GetComponent<pipe>().returnIsBalanceSplitter()){
                            Vector2[] combinedData = currentPiece.GetComponent<pipe>().returnPipeBalanceDirections();
                            Vector2 cleanDirection = combinedData[0];
                            Vector2 dirtyDirection = combinedData[1];
                            if(water.waterDirtState <= 0 && cleanDirection == element){
                                currentPiece.GetComponent<SpriteRenderer>().color = Color.blue;
                                tranverse(element,currentPos,water);
                            }else if(water.waterDirtState > 0 && dirtyDirection == element){
                                currentPiece.GetComponent<SpriteRenderer>().color = Color.blue;
                                tranverse(element,currentPos,water);
                            }
                        }else{
                            currentPiece.GetComponent<SpriteRenderer>().color = Color.blue;
                            tranverse(element,currentPos,water);
                        }
                    }else{
                        Debug.Log("Failed Water state check for direction");
                        currentPiece.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                }
               
            }
            return;
        }
        //SHould never reach here
        return;
        
    }

    //Spawn pipe section
    public void spawnPipe(string type){
    switch(type){
        case "straightPipe":
        GameObject currentPipe = Instantiate(Resources.Load<GameObject>("straightPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        //spawn right pipe
        break;
        case "bendyPipe":
        //spawn left pipe
        GameObject temp = Instantiate(Resources.Load<GameObject>("bendyPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        break;
        default:
        //shouldnt ever get here tbh
        break;
    }
}
}




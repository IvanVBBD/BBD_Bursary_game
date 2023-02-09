using UnityEngine;

public class animationManager : MonoBehaviour
{
    gridManager gridControl;
    pipeManager pipeControl;
    waterManager waterControl;
    private int boardHeight;
    private Vector2 startPos;
    private bool[,] animationBoard;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
        pipeControl = GameObject.FindGameObjectWithTag("pipeManager").GetComponent<pipeManager>();
        waterControl = GameObject.FindGameObjectWithTag("waterManager").GetComponent<waterManager>();
    }

    public void resetAnimations(){
        Debug.Log("RESET");
        for(int x = 0; x < boardHeight + 4; x++){
            for(int y = 0; y < boardHeight; y++){
                animationBoard[(int)x, (int)y] = false;
                GameObject tempPiece = gridControl.returnBoardObject(new Vector2(x, y));
                if(tempPiece != null){
                    tempPiece.GetComponentInChildren<Animator>().ResetTrigger("StartFlow");
                    //tempPiece.GetComponentInChildren<Animator>().ResetTrigger("White");
                    //tempPiece.GetComponentInChildren<Animator>().ResetTrigger("Blue");
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("State",1);
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("Balance",0);
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("Orientation",0);
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection",0);
                    tempPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
                    tempPiece.GetComponentInChildren<Animator>().ResetTrigger("Reset");

                }
            }
        }

        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);
        startPiece.GetComponentInChildren<Animator>().ResetTrigger("StartFlow");
        startPiece.GetComponentInChildren<Animator>().ResetTrigger("White");
        startPiece.GetComponentInChildren<Animator>().ResetTrigger("Blue");
        startPiece.GetComponentInChildren<Animator>().SetInteger("Orientation",0);
        startPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection",0);
        startPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
        startPiece.GetComponentInChildren<Animator>().ResetTrigger("Reset");
    }

    void resetPieceAnimation(GameObject piece){
        if(piece.GetComponent<Animator>())
            piece.GetComponent<Animator>().SetTrigger("Reset");
    }

    public void transverseAnimations(){
        boardHeight = gridControl.returnBoardHeight();
        animationBoard = new bool[boardHeight + 4, boardHeight]; // Should set all values to false by default

        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);

        startPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
        startPiece.GetComponentInChildren<Animator>().SetInteger("InputState", 1);
        startPiece.GetComponentInChildren<Animator>().SetInteger("OutputState", 1);
        animationBoard[(int)startPos.x, (int)startPos.y] = true;

    }

    public void triggerNextAnimations(GameObject currentPiece){
        Vector2[] connectingPoints;
        
        //resetPieceAnimation(currentPiece);
        if(currentPiece.gameObject.tag == "start"){
            connectingPoints = new[] { Vector2.right };       
            currentPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
     
        }
        else{
            connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
        }

        Vector2 currentPos = currentPiece.gameObject.transform.position;
        foreach(Vector2 connectingPoint in connectingPoints){
            Vector2 nextPos = currentPos + connectingPoint;
            GameObject nextPiece = gridControl.returnBoardObject(nextPos);
            if(nextPiece != null){
                if(nextPiece.gameObject.tag == "end"){
                    //nextPiece.GetComponentInChildren<Animator>().SetInteger("InputState", 1); // Need to check for actual state
                    nextPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");

                }
                else if(nextPiece.gameObject.tag == "pipe"){
                    Vector2[] nextConnectingPoints = nextPiece.GetComponent<pipe>().returnPipeDirections();
                    waterSpace.waterObject water = new waterSpace.waterObject();
                    switch(currentPiece.GetComponentInChildren<Animator>().GetInteger("OutputState")){
                        case 0:
                        water.waterPhaseState = waterSpace.waterStates.ICE;
                        break;
                        case 1:
                        water.waterPhaseState = waterSpace.waterStates.WATER;
                        break;
                        case 2:
                        water.waterPhaseState = waterSpace.waterStates.STEAM;
                        break;
                    }
                    water.waterDirtState = currentPiece.GetComponentInChildren<Animator>().GetInteger("Balance");
                    water = waterControl.alterWaterPhaseState(nextPiece, water);
                    switch(water.waterPhaseState){
                        case waterSpace.waterStates.ICE:
                        nextPiece.GetComponentInChildren<Animator>().SetInteger("OutputState",0);
                        break;
                        case waterSpace.waterStates.WATER:
                        nextPiece.GetComponentInChildren<Animator>().SetInteger("OutputState",1);
                        break;
                        case waterSpace.waterStates.STEAM:
                        nextPiece.GetComponentInChildren<Animator>().SetInteger("OutputState",2);
                        break;
                    }
                    nextPiece.GetComponentInChildren<Animator>().SetInteger("Balance",(int)water.waterDirtState);
                    bool connection = false;
                    // Validating pipe connection section of algorithm
                    Vector2 connectionPostion = Vector2.zero;
                    bool isConnected = false;
                    foreach(Vector2 nextConnectingPoint in nextConnectingPoints){
                        if((int)nextConnectingPoint.x + (int)connectingPoint.x == 0 && (int)nextConnectingPoint.y + (int)connectingPoint.y == 0){
                            connectionPostion = nextConnectingPoint;
                            isConnected = true;
                        }
                    }
                    foreach(Vector2 nextConnectingPoint in nextConnectingPoints){
                        if(isConnected && nextConnectingPoint != connectionPostion && connectionPostion != Vector2.zero){
                            if(waterControl.canMoveDirection(nextConnectingPoint,water)){
                                    if(nextPiece.GetComponent<pipe>().returnIsBalanceSplitter()){
                                        Vector2[] combinedData = nextPiece.GetComponent<pipe>().returnPipeBalanceDirections();
                                        Vector2 cleanDirection = combinedData[0];
                                        Vector2 dirtyDirection = combinedData[1];
                                        if(water.waterDirtState <= 0 && cleanDirection == nextConnectingPoint){
                                            connection = true;
                                             
                                        }else if(water.waterDirtState > 0 && dirtyDirection == nextConnectingPoint){
                                            connection = true;
                                        }
                                    }else{
                                        connection = true;
                                    }
                                }
                        }

                        if(connection){
                        nextPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
                        if(connectingPoint == Vector2.right){
                            nextPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection", 0);
                        }else if(connectingPoint == Vector2.left){
                            nextPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection", 2);
                        }else if(connectingPoint == Vector2.up){
                            nextPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection", 3);
                        }else if(connectingPoint == Vector2.down){
                            nextPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection", 1);
                        }
                        nextPiece.GetComponentInChildren<Animator>().SetInteger("InputState",currentPiece.GetComponentInChildren<Animator>().GetInteger("OutputState"));
                        nextPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                        }

                    }
                                   
                }       
            }                           
        }
    }
}
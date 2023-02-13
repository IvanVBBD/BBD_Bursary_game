using UnityEngine;

public class animationManager : MonoBehaviour
{
    gridManager gridControl;
    pipeManager pipeControl;
    waterManager waterControl;
    challengeManager challengeControl;
    private int boardHeight;
    private Vector2 startPos;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
        pipeControl = GameObject.FindGameObjectWithTag("pipeManager").GetComponent<pipeManager>();
        waterControl = GameObject.FindGameObjectWithTag("waterManager").GetComponent<waterManager>();
        challengeControl = GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>();
    }

    public void resetAnimations(){
        Debug.Log("RESET");
        for(int x = 0; x < boardHeight + 4; x++){
            for(int y = 0; y < boardHeight; y++){
                GameObject tempPiece = gridControl.returnBoardObject(new Vector2(x, y));
                if(tempPiece != null && tempPiece.gameObject.tag != "start"){
                    tempPiece.GetComponentInChildren<Animator>().ResetTrigger("StartFlow");
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("InputState",1);
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("OutputState",1);
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("Balance",challengeControl.returnWaterDirtLevel());
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection",0);
                    tempPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
                    tempPiece.GetComponentInChildren<Animator>().ResetTrigger("Reset");

                }
            }
        }

        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);
        startPiece.GetComponentInChildren<Animator>().ResetTrigger("StartFlow");
        startPiece.GetComponentInChildren<Animator>().SetInteger("InputState",1);
        startPiece.GetComponentInChildren<Animator>().SetInteger("OutputState",1);
        startPiece.GetComponentInChildren<Animator>().SetInteger("Balance",challengeControl.returnWaterDirtLevel());
        startPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
        startPiece.GetComponentInChildren<Animator>().ResetTrigger("Reset");
    }

    public void transverseAnimations(){
        boardHeight = gridControl.returnBoardHeight();
        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);
        startPiece.GetComponentInChildren<Animator>().SetInteger("InputState", 1);
        startPiece.GetComponentInChildren<Animator>().SetInteger("OutputState", 1);
        startPiece.GetComponentInChildren<Animator>().SetInteger("Balance",challengeControl.returnWaterDirtLevel());
        resetAnimations();
        startPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");

    }

    public void triggerNextAnimations(GameObject currentPiece){
        Vector2[] connectingPoints;
        int runtime = 0;
        //resetPieceAnimation(currentPiece);
        //if we have the start piece then we give Vector2.right
        if(currentPiece.gameObject.tag == "start"){
            connectingPoints = new[] { Vector2.right };       
            currentPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
     
        }
        else{
            //ELse we go and get the pipes direction;
            connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
        }


        //connectingPoints = old
        //nextConnectingPoints = new
        Vector2 currentPos = currentPiece.gameObject.transform.position;
        foreach(Vector2 connectingPoint in connectingPoints){
            runtime++;
            Vector2 inputFlowDirection = new Vector2();
            switch(currentPiece.GetComponentInChildren<Animator>().GetInteger("InputDirection")){
                case 0:
                inputFlowDirection = Vector2.right;
                break;
                case 1:
                inputFlowDirection = Vector2.down;
                break;
                case 2:
                inputFlowDirection = Vector2.left;
                break;
                case 3:
                inputFlowDirection = Vector2.up;
                break;
            }
            if((connectingPoint.x + inputFlowDirection.x == 0) && (connectingPoint.y + inputFlowDirection.y == 0)){
                continue;
            }

            //Logic section to deal with output flows of multi dimensioal pipe
            if(currentPiece.GetComponent<pipe>()){
                if(currentPiece.GetComponent<pipe>().returnIsBalanceSplitter()){
                    int balance = currentPiece.GetComponentInChildren<Animator>().GetInteger("Balance");
                    Vector2 [] specialDirections = currentPiece.GetComponent<pipe>().returnPipeBalanceDirections();
                    Vector2 cleanDirection = specialDirections[0];
                    Vector2 dirtyDirection = specialDirections[1];
                    if(balance <= 0 && connectingPoint != cleanDirection){
                        continue;
                    }else if(balance > 0 && connectingPoint != dirtyDirection){
                        continue;
                    }
                }else if(currentPiece.GetComponent<pipe>().returnIsNormalSplitter()){
                    Vector2 outputDir = currentPiece.GetComponent<pipe>().returnNormalOutput();
                    if(connectingPoint == outputDir){
                        
                    }else{
                        continue;
                    }
                }
            }
            Vector2 nextPos = currentPos + connectingPoint;
            GameObject nextPiece = gridControl.returnBoardObject(nextPos);
            if(nextPiece == null || nextPiece.gameObject.tag == "start"){
                Debug.Log("Whoop we caught a start or null");
            }else
            if(nextPiece != null){
                if(nextPiece.gameObject.tag == "end"){
                    //nextPiece.GetComponentInChildren<Animator>().SetInteger("InputState", 1); // Need to check for actual state
                     Debug.Log("WHAT WE HAVE AT END");
                    Debug.Log(currentPiece.GetComponentInChildren<Animator>().GetInteger("OutputState"));
                    Debug.Log(currentPiece.GetComponentInChildren<Animator>().GetInteger("Balance"));
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
                    
                    // Validating pipe connection section of algorithm
                    Debug.Log("new water state: " + water.waterPhaseState);
                    Debug.Log("new dirt state: " + water.waterDirtState);
                    Vector2 connectionPostion = Vector2.zero;
                    foreach(Vector2 nextConnectingPoint in nextConnectingPoints){
                        //
                        //(new Vector2((int)(nextPos.x + nextConnectingPoint.x),(int)(nextPos.y + nextConnectingPoint.y)) == currentPos)
                        
                        if((int)nextConnectingPoint.x + (int)connectingPoint.x == 0 && (int)nextConnectingPoint.y + (int)connectingPoint.y == 0){
                            connectionPostion = nextConnectingPoint;
                            Debug.Log("Runtime: " + runtime + " - " + nextPiece.name + " Direction Forbid: " + connectionPostion);
                        }
                        //Debug.Log(currentPiece.name + " pre: " + connectingPoint);
                        //Debug.Log(nextPiece.name + " post: " + nextConnectingPoint);
                    }
                    
                    foreach(Vector2 nextConnectingPoint in nextConnectingPoints){
                        bool connection = false;
                        if(nextConnectingPoint != connectionPostion && connectionPostion != Vector2.zero){
                           // Debug.Log("Runtime: " + runtime + " - " + nextPiece.name + " Direction Chosen: " + nextConnectingPoint);
                            if(waterControl.canMoveDirection(nextConnectingPoint,water)){
                                    if(nextConnectingPoints.Length > 2 && nextPiece.GetComponent<pipe>().returnIsBalanceSplitter()){
                                        Debug.Log("WE ARE A SPECIAL SPLITTER");
                                        Vector2[] combinedData = nextPiece.GetComponent<pipe>().returnPipeBalanceDirections();
                                        Vector2 cleanDirection = combinedData[0];
                                        Vector2 dirtyDirection = combinedData[1];
                                        if(water.waterDirtState <= 0 && cleanDirection == nextConnectingPoint && (dirtyDirection + connectingPoint != Vector2.zero)){
                                            connection = true;
                                            Debug.Log("we clean");                                        
                                                
                                        }else if(water.waterDirtState > 0 && dirtyDirection == nextConnectingPoint && (cleanDirection + connectingPoint != Vector2.zero)){
                                            connection = true;
                                           Debug.Log("we dirty");
                                        }

                                    }else if(nextConnectingPoints.Length > 2 && nextPiece.GetComponent<pipe>().returnIsNormalSplitter()){
                                        Vector2 outputDirection = nextPiece.GetComponent<pipe>().returnNormalOutput();
                                        if(outputDirection == nextConnectingPoint){
                                            connection = false;
                                        }else{
                                            connection = true;
                                        }
                                    }else{
                                        connection = true;
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
                                    connection = false;

                                }
                        }
                        
                        connection = false;
                    }
                    
                                   
                }       
            }                           
        }
    }
}
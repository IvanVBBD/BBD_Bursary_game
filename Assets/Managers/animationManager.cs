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
                    tempPiece.GetComponentInChildren<Animator>().ResetTrigger("White");
                    tempPiece.GetComponentInChildren<Animator>().ResetTrigger("Blue");
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("Orientation",0);
                    tempPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection",0);
                    tempPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
                    tempPiece.GetComponentInChildren<Animator>().ResetTrigger("Reset");

                }
            }
        }

        startPos = gridControl.returnStartPosition();
        GameObject startPiece = gridControl.returnBoardObject(startPos);
        // startPiece.GetComponentInChildren<Animator>().ResetTrigger("StartFlow");
        // startPiece.GetComponentInChildren<Animator>().ResetTrigger("White");
        // startPiece.GetComponentInChildren<Animator>().ResetTrigger("Blue");
        // startPiece.GetComponentInChildren<Animator>().SetInteger("Orientation",0);
        // startPiece.GetComponentInChildren<Animator>().SetInteger("InputDirection",0);
        // startPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
        // startPiece.GetComponentInChildren<Animator>().ResetTrigger("Reset");
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
        startPiece.GetComponentInChildren<Animator>().SetTrigger("Blue");
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
                    animationBoard[(int)nextPos.x, (int)nextPos.y] = true;
                    nextPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                }
                else if(nextPiece.gameObject.tag != "start"){
                    Vector2[] nextConnectingPoints = nextPiece.GetComponent<pipe>().returnPipeDirections();
                    bool connection = false;
                    // Validating pipe connection section of algorithm
                    foreach(Vector2 nextConnectingPoint in nextConnectingPoints){
                        if((int)nextConnectingPoint.x + (int)connectingPoint.x == 0 && (int)nextConnectingPoint.y + (int)connectingPoint.y == 0){
                            if(nextPiece.GetComponent<pipe>().returnIsBalanceSplitter()){
                                Vector2[] combinedData = nextPiece.GetComponent<pipe>().returnPipeBalanceDirections();
                                Vector2 cleanDirection = combinedData[0];
                                Vector2 dirtyDirection = combinedData[1];

                                Debug.Log($"CLEAN DIRECTION: {cleanDirection}");
                                Debug.Log($"DIRTY DIRECTION: {dirtyDirection}");
                                Debug.Log($"NEXT CONNECTION: {nextConnectingPoint}");
                                Debug.Log($"PREVIOUS CONNECTION: {connectingPoint}");
                                if(cleanDirection == nextConnectingPoint){
                                    connection = false;
                                }else if(dirtyDirection == nextConnectingPoint){
                                    connection = false;
                                }else{
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
                        nextPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
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
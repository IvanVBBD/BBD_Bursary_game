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

                if(animationBoard[(int)x, (int)y]){
                    GameObject tempPiece = gridControl.returnBoardObject(new Vector2(x, y));
                    if(tempPiece != null){
                        tempPiece.GetComponentInChildren<Animator>().SetTrigger("Reset");
                    }
                    animationBoard[(int)x, (int)y] = false;
                }
            }
        }
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

        if(currentPiece.gameObject.tag == "start"){
            connectingPoints = new[] { Vector2.right };            
        }
        else{
            connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();
        }

        Vector2 currentPos = currentPiece.gameObject.transform.position;
        foreach(Vector2 connectingPoint in connectingPoints){
            Vector2 nextPos = currentPos + connectingPoint;

            if(animationBoard[(int)nextPos.x, (int)nextPos.y] == false){
                GameObject nextPiece = gridControl.returnBoardObject(nextPos);
                if(nextPiece != null){
                    if(nextPiece.gameObject.tag == "end"){
                        animationBoard[(int)nextPos.x, (int)nextPos.y] = true;
                        nextPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");
                    }
                    else if(nextPiece.GetComponent<pipe>().getConnectedStatus()){
                        Vector2[] nextConnectingPoints = nextPiece.GetComponent<pipe>().returnPipeDirections();
                        bool connection = false;
                        // Validating pipe connection section of algorithm
                        foreach(Vector2 nextConnectingPoint in nextConnectingPoints){
                            if((int)nextConnectingPoint.x + (int)connectingPoint.x == 0 && (int)nextConnectingPoint.y + (int)connectingPoint.y == 0){
                                connection = true;
                            }
                        }

                        if(connection){
                            animationBoard[(int)nextPos.x, (int)nextPos.y] = true;

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
            }
            else{
                // JESSE TO DO: 
                // Check if need to change state/type of water and restart the animation loop
            }    
        }
    }
}
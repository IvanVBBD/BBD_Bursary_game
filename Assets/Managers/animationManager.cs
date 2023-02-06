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
                        animationBoard[(int)nextPos.x, (int)nextPos.y] = true;
                        tempPiece.GetComponentInChildren<Animator>().SetTrigger("StartFlow");

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
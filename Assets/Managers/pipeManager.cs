using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeManager : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 startPos = new Vector2(0,0); // need to plug into the start point;

    private gridManager gridControl; 
    private bool succCon = false;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
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
        tranverse(Vector2.right,foundStart);
        if(succCon == true){
            Debug.Log("YAAAAS U WIN");
        }else{
            Debug.Log("Failed End");
        }
    }

    void tranverse(Vector2 change, Vector2 currentPos){
        Vector2 oldPos = currentPos;
        Debug.Log($"old Postion: {oldPos}");
        currentPos = new Vector2((int)(currentPos.x + change.x),(int)(currentPos.y + change.y));
        Debug.Log($"New Postion: {currentPos}");
        GameObject currentPiece = gridControl.returnBoardObject(currentPos);
        if(currentPiece == null){
            Debug.Log("YUUUP its null");
            return;
        }else if (currentPiece.gameObject.tag == "end"){
            succCon = true;
            return;
        }else if (currentPiece.gameObject.tag == "pipe"){
            //get stuff off object and call transverse again
            Debug.Log("we in here!");
            Vector2[] connectingPoints = currentPiece.GetComponent<pipe>().returnPipeDirections();


            ///Validating pipe connection section of algorithim
            Vector2 connectingPostion = Vector2.zero; // init
            foreach(Vector2 pointSet in connectingPoints){
                if (new Vector2((int)(currentPos.x + pointSet.x),(int)(currentPos.y + pointSet.y)) == oldPos){

                    connectingPostion = pointSet;// new Vector2((int)(currentPos.x + pointSet.x),(int)(currentPos.y + pointSet.y));
                }
            }

            //This section deals with transerving out all possible ends of the pipe that is not the entry side
            foreach(Vector2 element in connectingPoints){
                Debug.Log("STAGE 1");
                Debug.Log($"checking point {element}");
                if(element != connectingPostion && connectingPostion != Vector2.zero){
                    Debug.Log("STAGE 2");
                    Debug.Log($"chosen point {element}");
                    currentPiece.GetComponent<SpriteRenderer>().color = Color.blue;
                    tranverse(element,currentPos);
                    return;
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
        case "rightPipe":
        GameObject currentPipe = Instantiate(Resources.Load<GameObject>("rightPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        //spawn right pipe
        break;
        case "leftPipe":
        //spawn left pipe
        GameObject temp = Instantiate(Resources.Load<GameObject>("leftPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        break;
        case "upRightPipe":
        Instantiate(Resources.Load<GameObject>("upRightPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        break;
        case "upLeftPipe":
        Instantiate(Resources.Load<GameObject>("upLeftPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        break;
        case "downLeftPipe":
        Instantiate(Resources.Load<GameObject>("downLeftPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        break;
        case "downRightPipe":
        Instantiate(Resources.Load<GameObject>("downRightPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
        break;

        default:
        //shouldnt ever get here tbh
        break;
    }
}
}




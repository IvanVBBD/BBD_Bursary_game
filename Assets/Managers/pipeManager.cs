using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeManager : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 startPos = new Vector2(0,0); // need to plug into the start point;

    private gridManager gridControl; 

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void beginTrans(){
        Vector2 foundStartChange = new Vector2(0,0);
        bool result = tranverse(foundStartChange,startPos);
        if(result == true){
            Debug.Log("YAAAAS U WIN");
        }
    }

    bool tranverse(Vector2 change, Vector2 currentPos){
        currentPos = new Vector2((int)(currentPos.x + change.x),(int)(currentPos.y + change.y));
        GameObject nextPiece = gridControl.returnBoardObject(currentPos);
        if(nextPiece == null){
            return false;
        }else if (nextPiece.tag == "end"){
            return true;
        }else if (nextPiece.tag == "pipe"){
            //get stuff off object and call transverse again
            Vector2[] nextPoints = nextPiece.GetComponent<pipe>().returnPipeDirections();
            bool tempResult = false;
            foreach(Vector2 element in nextPoints){
                bool result = tranverse(element,currentPos);
                if(result){
                    tempResult = true;
                }
            }
            return tempResult;
        }

        //SHould never reach here
        return false;
        
    }
}

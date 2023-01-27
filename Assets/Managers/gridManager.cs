using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int width,height;

    [SerializeField] private GameObject tile;

    [SerializeField] private GameObject pieceSelected;

    [SerializeField] private GameObject[,] board = new GameObject[10,10];

    [SerializeField] private Vector2 startPos,endPos;

  

    void Start(){
        resetGrid();
        GenerateGrid();
    }
    void GenerateGrid(){
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                GameObject spawnedTile = Instantiate(tile,new Vector3(x,y,0),Quaternion.identity);
                spawnedTile.name = $"Tile - {x} , {y}";
                if((y % 2 != 0 && x % 2 == 0) || (y % 2 == 0 && x % 2 != 0)){
                    spawnedTile.GetComponent<tile>().init(true);
                }
            }
           
        }

        //hook in here to set-up end/start point
        endPos = new Vector2((int)Random.Range(0f,width - 1),(int)Random.Range(0f,height - 1));
        startPos = new Vector2((int)Random.Range(0f,width - 1),(int)Random.Range(0f,height - 1));
        board[(int)startPos.x,(int)startPos.y] = Instantiate(Resources.Load<GameObject>("start"),new Vector3 ((int)startPos.x, (int)startPos.y, -1f),Quaternion.identity);
        board[(int)endPos.x,(int)endPos.y] = Instantiate(Resources.Load<GameObject>("end"),new Vector3 ((int)endPos.x, (int)endPos.y, -1f),Quaternion.identity);
        Camera.main.transform.position = new Vector3((float)width/2 -0.5f,(float)height/2 - 0.5f,-10f);

    }


    //Public interface for gridManager for selecting objects with dragAndDrop
    public void setPickUpObject(GameObject _object){
        pieceSelected = _object;
    }

    public GameObject returnCurrentPickUp(){
        return pieceSelected;
    }


    //Used to resetGrid to null on start of game, needed to get around GameObject[,] not init as null but empty object
    void resetGrid(){
         for(int j = 0 ; j < width;j++){
                for(int k = 0; k < height; k++){
                    board[j,k] = null;
                }
            }
    }

      void pieceDetection(GameObject _piece){
        for(int x = 0;x < width;x++){
            for (int y = 0; y < height; y++){
                if (board[x,y] == _piece){
                    board[x,y] = null;
                }
            }
        }
    }
    public void snapToGrid(){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePosition.x > 0 && mousePosition.x < width && mousePosition.y > 0 && mousePosition.y < height){
            int gridX = Mathf.RoundToInt(mousePosition.x);
            int gridY = Mathf.RoundToInt(mousePosition.y);
            if(board[gridX,gridY] == null){
                pieceSelected.transform.position = new Vector3(gridX,gridY,-1f);
                pieceDetection(pieceSelected);
                board[gridX,gridY] = pieceSelected;
                Debug.Log($"Object was added to : {gridX},{gridY}");
            }else if(board[gridX,gridY] != null){
                Destroy(pieceSelected); //might need to inc piece property later
            }
           
        }else{
            pieceDetection(pieceSelected);
        }
        pieceSelected = null;
    }

    public GameObject returnBoardObject(Vector2 _input){
        
        return board[(int)_input.x,(int)_input.y];
    }

    public Vector2 returnStartPosition(){
        return startPos;
    }
}

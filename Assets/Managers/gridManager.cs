using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int numTiles_y; //numTiles_x,
    private int numTiles_x;
    private int maxNumTiles_y = 16;
    private int minNumTiles_y = 8;
    [SerializeField] private GameObject tile;

    [SerializeField] private GameObject pieceSelected;

    [SerializeField] private GameObject[,] board = new GameObject[10,10];

    [SerializeField] private Vector2 startPos,endPos;



    void Start(){
        if(numTiles_y < minNumTiles_y){
            numTiles_y = minNumTiles_y;
        }
        if(numTiles_y > maxNumTiles_y){
            numTiles_y = maxNumTiles_y;
        }

        numTiles_x = numTiles_y + 4;

        board = new GameObject[numTiles_x, numTiles_y];
        resetGrid();
        GenerateGrid();
    }
    void GenerateGrid(){
        for(int x = 0; x < numTiles_x; x++){
            for(int y = 0; y < numTiles_y; y++){
                GameObject spawnedTile = Instantiate(tile,new Vector3(x,y,0),Quaternion.identity);
                spawnedTile.name = $"Tile - {x} , {y}";

                int dirtChooser = Random.Range(0, 4);
                SpriteRenderer sprRend = spawnedTile.GetComponent<SpriteRenderer>();
                sprRend.sprite = Resources.Load<Sprite>($"Textures/DirtBlock_{dirtChooser}");
                sprRend.drawMode = SpriteDrawMode.Sliced;
                sprRend.size = new Vector2(1f, 1f);

                /*
                if((y % 2 != 0 && x % 2 == 0) || (y % 2 == 0 && x % 2 != 0)){
                    spawnedTile.GetComponent<tile>().init(true);
                }*/


            }
           
        }

        //hook in here to set-up end/start point
        endPos = new Vector2((int)Random.Range(0f,numTiles_x - 1),(int)Random.Range(0f,numTiles_y - 1));
        startPos = new Vector2((int)Random.Range(0f,numTiles_x - 1),(int)Random.Range(0f,numTiles_y - 1));
        board[(int)startPos.x,(int)startPos.y] = Instantiate(Resources.Load<GameObject>("start"),new Vector3 ((int)startPos.x, (int)startPos.y, -1f),Quaternion.identity);
        board[(int)endPos.x,(int)endPos.y] = Instantiate(Resources.Load<GameObject>("end"),new Vector3 ((int)endPos.x, (int)endPos.y, -1f),Quaternion.identity);
        float screenSize_x = Screen.width;
        float screenSize_y = Screen.height;

        //screenSize_x*50% = numTiles_x
        //
        float temp_x = screenSize_x/numTiles_x;
        float temp_y = screenSize_y/numTiles_y;
        //Camera.main.transform.position = new Vector3((float)width/2 -0.5f,(float)height/2 - 0.5f,-10f);
        //Camera.main.transform.position = new Vector3((float)width/2 -0.5f,(float)height/2 - 0.5f,-10f);

        Camera.main.transform.position = new Vector3((float)0.0f,(float)numTiles_y/2.0f,-10f);
        Camera.main.orthographicSize = ((float)(numTiles_y-8)*0.5f + 6.0f); //TAKE CARE OF THESE MAGIC NUMBERS :( )


/* CAMERA SIZE LOGIC
index   numTiles_y  diff    orthographicSize
1	    8	        -2.0	6.0
2	    9	        -2.5	6.5	0.5
3	    10	        -3.0	7.0	1.0
4	    11	        -3.5	7.5	1.5
5	    12	        -4.0	8.0
6	    13	        -4.5	8.5
7	    14	        -5.0	9.0
8	    15	        -5.5	9.5
9	    16	        -6.0	10.0
*/
    }


    //Public interface for gridManager for selecting objects with dragAndDrop
    public void setPickUpObject(GameObject _object){
        pieceDetection(_object);
        pieceSelected = _object;
    }

    public GameObject returnCurrentPickUp(){
        return pieceSelected;
    }


    //Used to resetGrid to null on start of game, needed to get around GameObject[,] not init as null but empty object
    void resetGrid(){
         for(int j = 0 ; j < numTiles_x;j++){
                for(int k = 0; k < numTiles_y; k++){
                    board[j,k] = null;
                }
            }
    }

      void pieceDetection(GameObject _piece){
        for(int x = 0;x < numTiles_x;x++){
            for (int y = 0; y < numTiles_y; y++){
                if (board[x,y] == _piece){
                    board[x,y] = null;
                }
            }
        }
    }
    public void snapToGrid(){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePosition.x > 0 && mousePosition.x < numTiles_x && mousePosition.y > 0 && mousePosition.y < numTiles_y){
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

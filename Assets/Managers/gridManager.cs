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
    private int inventoryWidth = 2;
    private int inventoryPadding = 1;
    private int inventoryStart, inventoryEnd;
    [SerializeField] private GameObject tile;

    [SerializeField] private GameObject pieceSelected;

    private GameObject[,] board; //= new GameObject[10,10];
    private GameObject[,] inventory;
    private int[,] inventoryCount;
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
        inventory = new GameObject[inventoryWidth, numTiles_y];
        inventoryCount = new int[inventoryWidth, numTiles_y];

        inventoryStart = numTiles_x + inventoryPadding;
        inventoryEnd = numTiles_x + inventoryPadding + inventoryWidth;

        resetGrid();
        generateGrid();
        resetInventoryGrid();
        generateInventoryGrid();
        populateInventoryGrid(); // Will need to send in arguments of which pieces to add
    }

    void addStartEndPipes(){
        //hook in here to set-up end/start point
        endPos = new Vector2((int)Random.Range(0f,numTiles_x - 1),(int)Random.Range(0f,numTiles_y - 1));
        startPos = new Vector2((int)Random.Range(0f,numTiles_x - 1),(int)Random.Range(0f,numTiles_y - 1));
        board[(int)startPos.x,(int)startPos.y] = Instantiate(Resources.Load<GameObject>("start"),new Vector3 ((int)startPos.x, (int)startPos.y, -1f),Quaternion.identity);
        board[(int)endPos.x,(int)endPos.y] = Instantiate(Resources.Load<GameObject>("end"),new Vector3 ((int)endPos.x, (int)endPos.y, -1f),Quaternion.identity);
    }

    void moveCameraToGrid(){
        float screenSize_x = Screen.width;
        float screenSize_y = Screen.height;
        float temp_x = screenSize_x/numTiles_x;
        float temp_y = screenSize_y/numTiles_y;
        Debug.Log($"SCREEN WIDTH: {screenSize_x}");
        Debug.Log($"SCREEN HEIGHT: {screenSize_y}");
        
        Camera.main.transform.position = new Vector3((float)(inventoryEnd)/2 -0.5f,(float)numTiles_y/2,-10f);
        Camera.main.orthographicSize = ((float)(numTiles_y/2.0f + 1.5f)); //TAKE CARE OF THESE MAGIC NUMBERS :( )
    }

    void generateGrid(){
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
        addStartEndPipes();
        moveCameraToGrid();
    }

    void generateInventoryGrid(){
        for(int x = numTiles_x + inventoryPadding; x < numTiles_x + inventoryPadding + inventoryWidth; x++){
            for(int y = 0; y < numTiles_y; y++){
                GameObject spawnedTile = Instantiate(tile,new Vector3(x,y,0),Quaternion.identity);
                spawnedTile.name = $"Tile - {x} , {y}";

                int dirtChooser = Random.Range(0, 4);
                SpriteRenderer sprRend = spawnedTile.GetComponent<SpriteRenderer>();
                sprRend.sprite = Resources.Load<Sprite>($"Textures/DirtBlock_{dirtChooser}");
                sprRend.drawMode = SpriteDrawMode.Sliced;
                sprRend.size = new Vector2(1f, 1f);
            }
        }
    }

    //Spawn pipe section NB NB NB This assumes the spawner isn't making a mistake and trying to spawn multiple types of pipes on top of one another
    public void spawnPipe(string type, int x, int y){
        int inventory_x = x-inventoryStart;

        switch(type){
            case "straightPipe":
                GameObject straightPipe = Instantiate(Resources.Load<GameObject>("straightPipe"),new Vector3 (x, y, -1f),Quaternion.identity);
                inventory[inventory_x, y] = straightPipe;
                inventoryCount[inventory_x, y]++;
            break;
            case "bendyPipe":
                GameObject bendyPipe = Instantiate(Resources.Load<GameObject>("bendyPipe"),new Vector3 (x, y, -1f),Quaternion.identity);
                inventory[inventory_x, y] = bendyPipe;
                inventoryCount[inventory_x, y]++;
            break;
            default:
                // Shouldn't ever get here tbh
            break;
        }
    }



    //Public interface for gridManager for selecting objects with dragAndDrop
    public void setPickUpObject(GameObject _object){
        removePieceFromArray(_object);
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
    void resetInventoryGrid(){
         for(int j = 0 ; j < inventoryWidth; j++){
            for(int k = 0; k < numTiles_y; k++){
                inventory[j,k] = null;
                inventoryCount[j,k] = 0;
            }
        }
    }
    
    // NOTE: Renamed from "pieceDetection"
    void removePieceFromArray(GameObject _piece){
        for (int y = 0; y < numTiles_y; y++){
            for(int x = 0;x < numTiles_x;x++){
                if (board[x,y] == _piece){
                    board[x,y] = null;
                }
            }
            for(int x = 0; x < inventoryWidth; x++){
                if(inventory[x,y] != null){
                    if(inventory[x,y].name == _piece.name){
                        if(inventoryCount[x,y] > 1){
                            inventoryCount[x,y]--;
                        }else if(inventoryCount[x,y] > 0){
                            inventory[x,y] = null;
                            inventoryCount[x,y]--;
                        }
                    }
                }

            }
        }
    }


    // NOTE: This function will need to be greatly expanded to take in the inventory from the backend algorithm
    void populateInventoryGrid(){
        /*for(int x = 0; x < inventoryWidth; x++){
            for(int y = 0; y < numTiles_y; y++){
                spawnPipe("straightPipe", x+inventoryStart, y);
            }
        }*/
        spawnPipe("straightPipe", inventoryStart, numTiles_y-1);
        spawnPipe("straightPipe", inventoryStart, numTiles_y-1);
        spawnPipe("straightPipe", inventoryStart, numTiles_y-2);
        spawnPipe("bendyPipe", inventoryStart, numTiles_y-3);
        spawnPipe("bendyPipe", inventoryStart, numTiles_y-3);
        spawnPipe("bendyPipe", inventoryStart, numTiles_y-3);
        spawnPipe("bendyPipe", inventoryStart, numTiles_y-3);
        spawnPipe("bendyPipe", inventoryStart, numTiles_y-4);

    }

    public void snapToGrid(){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int gridX = Mathf.RoundToInt(mousePosition.x);
        int gridY = Mathf.RoundToInt(mousePosition.y);

        // Check if mouse pos is within the grid+inventory space
        if(gridX >= 0 && gridY >= 0 && gridY < numTiles_y){
            // Check if mouse pos is within only the grid space
            if(gridX < numTiles_x){
                if(board[gridX,gridY] == null){
                    pieceSelected.transform.position = new Vector3(gridX,gridY,-1f);
                    //pieceDetection(pieceSelected);
                    board[gridX,gridY] = pieceSelected;
                }else if(board[gridX,gridY] != null){
                    Destroy(pieceSelected); //might need to inc piece property later
                }
            // Check if mouse pos is within only the inventory space
            }else if(gridX >= inventoryStart && gridX < inventoryEnd){
                if(inventory[gridX-inventoryStart,gridY] == null){
                    pieceSelected.transform.position = new Vector3(gridX,gridY,-1f);
                    inventory[gridX-inventoryStart,gridY] = pieceSelected;
                    inventoryCount[gridX-inventoryStart,gridY]++;
                }
                else if(inventory[gridX-inventoryStart,gridY].name.ToString() == pieceSelected.name.ToString()){
                    pieceSelected.transform.position = new Vector3(gridX,gridY,-1f);
                    inventoryCount[gridX-inventoryStart,gridY]++;
                    Debug.Log("OMG THIS IS NOT WORKING");
                }else{
                    Destroy(pieceSelected); //might need to inc piece property later
                }
                Debug.Log($"PIECE SELECTED NAME: {pieceSelected.name}");
                Debug.Log($"INVENTORY NAME: {inventory[gridX-inventoryStart,gridY].name}");
            }          
        }else{
            //pieceDetection(pieceSelected);
        }
        pieceSelected = null;

        string toPrint = "\n";
        for(int i = 0; i < 2; i ++){
            for(int j = 0; j < gridY; j++)
            {
                toPrint = toPrint + (inventoryCount[i,j]).ToString();
            }
            toPrint = toPrint + "||";
        }
        Debug.Log(toPrint);
    }

    public GameObject returnBoardObject(Vector2 _input){
        return board[(int)_input.x,(int)_input.y];
    }

    public Vector2 returnStartPosition(){
        return startPos;
    }
}
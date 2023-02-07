using UnityEngine;

public class gridManager : MonoBehaviour
{
    // Start is called before the first frame update
    challengeManager challengeControl;
    [SerializeField] private int numTiles_y; //numTiles_x,    
    private int numTiles_x;
    private const int maxNumTiles_y = 16;
    private const int minNumTiles_y = 12;
    private int inventoryWidth = 2;
    private int inventoryPadding = 1;
    private int inventoryStart, inventoryEnd;
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject tile_inventory;
    

    [SerializeField] private GameObject pieceSelected;

    private GameObject[,] board; //= new GameObject[10,10];
    [SerializeField] private Vector2 startPos,endPos;
    uiManager uiControl;

    void Awake(){
        uiControl = GameObject.FindGameObjectWithTag("uiManager").GetComponent<uiManager>();
        challengeControl = GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>();
    }
    
    void Start(){
        if(numTiles_y < minNumTiles_y){
            numTiles_y = minNumTiles_y;
        }
        if(numTiles_y > maxNumTiles_y){
            numTiles_y = maxNumTiles_y;
        }
        numTiles_x = numTiles_y + 4;

        board = new GameObject[numTiles_x, numTiles_y];

        inventoryStart = numTiles_x + inventoryPadding;
        inventoryEnd = numTiles_x + inventoryPadding + inventoryWidth;

        resetGrid();
        generateGrid();
        moveCameraToGrid();
        generateInventoryGrid();
    }

    void addStartEndPipes(){
        //hook in here to set-up end/start point
        endPos = challengeControl.returnEndPos();
        startPos = challengeControl.returnStartPos();
        board[(int)startPos.x,(int)startPos.y] = Instantiate(Resources.Load<GameObject>("start"),new Vector3 ((int)startPos.x, (int)startPos.y, -1f),Quaternion.identity);
        board[(int)endPos.x,(int)endPos.y] = Instantiate(Resources.Load<GameObject>("end"),new Vector3 ((int)endPos.x, (int)endPos.y, -1f),Quaternion.identity);
    }

    void moveCameraToGrid(){
        // float screenSize_x = Screen.width;
        // float screenSize_y = Screen.height;
        // float temp_x = screenSize_x/numTiles_x;
        // float temp_y = screenSize_y/numTiles_y;
        // Debug.Log($"SCREEN WIDTH: {screenSize_x}");
        // Debug.Log($"SCREEN HEIGHT: {screenSize_y}");
        
        Camera.main.transform.position = new Vector3((float)(inventoryEnd)/2 -0.5f,(float)numTiles_y/2 +0.5f,-10f);
        Camera.main.orthographicSize = ((float)(numTiles_y/2.0f + 2.0f)); //TAKE CARE OF THESE MAGIC NUMBERS :( )
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
            }
        }
        addStartEndPipes();
    }

    void generateInventoryGrid(){
        int min_x = numTiles_x + inventoryPadding;
        int max_x = numTiles_x + inventoryPadding + inventoryWidth;
        int min_y = 0;
        int max_y = numTiles_y - 2;

        for(int x = min_x; x < max_x; x++){
            for(int y = min_y; y < max_y; y++){
                
                GameObject spawnedTile = Instantiate(tile_inventory,new Vector3(x,y,0),Quaternion.identity);
                spawnedTile.name = $"Tile - {x} , {y}";

                int dirtChooser = Random.Range(0, 4);
                SpriteRenderer sprRend = spawnedTile.GetComponent<SpriteRenderer>();
                sprRend.sprite = Resources.Load<Sprite>($"Textures/DirtBlock_{dirtChooser}");
                sprRend.drawMode = SpriteDrawMode.Sliced;
                sprRend.size = new Vector2(1f, 1f);
            }
        }
        uiControl.setInventoryPositions(min_x, min_y, max_x, max_y);

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
    
    // NOTE: Renamed from "pieceDetection"
    void removePieceFromArray(GameObject _piece){
        for (int y = 0; y < numTiles_y; y++){
            for(int x = 0;x < numTiles_x;x++){
                if (board[x,y] == _piece){
                    board[x,y] = null;
                }
            }
        }
    }

    public void snapToGrid(){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int gridX = Mathf.RoundToInt(mousePosition.x);
        int gridY = Mathf.RoundToInt(mousePosition.y);

        // Check if mouse pos is within the grid+inventory space
        if(gridX >= 0 && gridY >= 0 && gridY < numTiles_y){
            // Check if mouse pos is within only the grid space
            if(gridX < numTiles_x){
                if(board[gridX,gridY] == null && pieceSelected != null){
                    pieceSelected.transform.position = new Vector3(gridX,gridY,-1f);
                    //pieceDetection(pieceSelected);
                    board[gridX,gridY] = pieceSelected;
                }else if(board[gridX,gridY] != null && board[gridX,gridY].tag != "start" && board[gridX,gridY].tag != "end"){
                   // Destroy(pieceSelected); //might need to inc piece property later
                   string creditType = board[gridX,gridY].GetComponent<pipe>().returnCreditType(); //crediting inventory
                   GameObject.FindGameObjectWithTag("inventoryManager").GetComponent<inventoryManager>().creditPipes(creditType);
                   Destroy(board[gridX,gridY]);
                   board[gridX,gridY] = pieceSelected;
                   pieceSelected.transform.position = new Vector3(gridX,gridY,-1f);
                }         
            }
            else if(gridX >= inventoryStart){
                string creditType = pieceSelected.GetComponent<pipe>().returnCreditType(); //crediting inventory
                GameObject.FindGameObjectWithTag("inventoryManager").GetComponent<inventoryManager>().creditPipes(creditType);   
                Destroy(pieceSelected);
            }
        }
        pieceSelected = null;
    }

    public GameObject returnBoardObject(Vector2 _input){
        return board[(int)_input.x,(int)_input.y];
    }

    public Vector2 returnStartPosition(){
        return startPos;
    }

    public int returnBoardHeight(){
        return numTiles_y;
    }
}
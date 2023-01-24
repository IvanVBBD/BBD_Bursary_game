using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int width,height;

    [SerializeField] private GameObject tile;

    [SerializeField] private GameObject pieceSelected;
  

    void Start(){
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

        Camera.main.transform.position = new Vector3((float)width/2 -0.5f,(float)height/2 - 0.5f,-10f);
    }


    //Public interface for gridManager for selecting objects

    public void setPickuPObject(GameObject _object){
        pieceSelected = _object;
    }

    public void snapToGrid(){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePosition.x > 0 && mousePosition.x < width && mousePosition.y > 0 && mousePosition.y < height){
            int gridX = Mathf.RoundToInt(mousePosition.x);
            int gridY = Mathf.RoundToInt(mousePosition.y);
            pieceSelected.transform.position = new Vector3(gridX,gridY,-1f);
        } 
        pieceSelected = null;
    }
}

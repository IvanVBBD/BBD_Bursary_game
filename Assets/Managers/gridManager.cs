using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int width,height;
    [SerializeField] private GameObject tile;
  

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace responseStructures{
    public class serverChallenge{
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public int waterData;
    public int straightPipe;
    public int bendyPipe;
    public int filterPipe;
    public int contaminatorPipe;
    public int heatPipe;
    public int freezePipe;
    public int splitterPipe;
    public int specialSplitterPipe;
    public string theme;
    

}

}

public class challengeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static challengeManager instance;
    inventorySpace.inventory currentInventory = new inventorySpace.inventory();
    Vector2 startPos, endPos;

    string theme;
    int waterDirtLevel = 0;
   

   void Awake(){
        if(instance != null){
            Destroy(gameObject);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    
    startPos = Vector2.zero;
    endPos = Vector2.zero;
   }


   public void setStartPos(int _x, int _y){
        startPos = new Vector2(_x,_y);   
   }

    public Vector2 returnStartPos(){
        return startPos;
    }


    public void setWaterDirtLevel(int _level){
        waterDirtLevel = _level;
    }

    public int returnWaterDirtLevel() => waterDirtLevel;

    public void setEndPos(int _x, int _y){
        endPos = new Vector2(_x,_y);
    }

    public Vector2 returnEndPos(){return endPos;}

    public void setUpInventory(responseStructures.serverChallenge _inventory){
        this.currentInventory.straightPipe = _inventory.straightPipe;
        this.currentInventory.bendyPipe = _inventory.bendyPipe;
        this.currentInventory.filterPipe = _inventory.filterPipe;
        this.currentInventory.contaminatorPipe =_inventory.contaminatorPipe;
        this.currentInventory.heatPipe = _inventory.heatPipe;
        this.currentInventory.freezePipe = _inventory.freezePipe;
        this.currentInventory.splitterPipe = _inventory.splitterPipe;
        this.currentInventory.specialSplitterPipe = _inventory.specialSplitterPipe;
    }

    public inventorySpace.inventory returnInventory() => currentInventory;

    public string returnTheme() => theme;

    void setTheme(string _theme) => theme = _theme;

    public void setUpChallenge(string _input){
        currentInventory = new inventorySpace.inventory();
        responseStructures.serverChallenge _challenge = JsonUtility.FromJson<responseStructures.serverChallenge>(_input);
        this.setStartPos(_challenge.startX,_challenge.startY);
        this.setEndPos(_challenge.endX,_challenge.endY);
        this.setWaterDirtLevel(_challenge.waterData);
        this.setUpInventory(_challenge);
        this.setTheme(_challenge.theme);

        SceneManager.LoadScene(1);
    }


}
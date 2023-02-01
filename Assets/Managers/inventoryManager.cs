using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inventorySpace{
    public struct inventory {
        public int straightPipe;
        public int bendyPipe;
        public int filterPipe;
        public int contaminatorPipe;
        public int heatPipe;
        public int freezePipe;
        public int splitterPipe;
        public int specialSplitterPipe;
    }
}


public class inventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    uiManager uiControl;
    challengeManager challengeControl;

    inventorySpace.inventory currentInventory;
    void generateInventory(){

        //hook in to number of pipes to spawn here for each type
       currentInventory = challengeControl.returnInventory();
       uiControl.updatePipeNumbersUI(currentInventory);
    }


    void Awake(){
        uiControl = GameObject.FindGameObjectWithTag("uiManager").GetComponent<uiManager>();
        challengeControl = GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>();
    }
    void Start()
    {
        generateInventory();
        // Debug.Log("====Starting======");
        // Debug.Log("Inventory: ");
        // Debug.Log(currentInventory.straightPipe);
    }

    public GameObject requestPipeSpawn(string type){
         switch(type){
            case "straightPipe":
                if(currentInventory.straightPipe > 0){
                    currentInventory.straightPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("straightPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }

            break;
            case "bendyPipe":
                if(currentInventory.bendyPipe > 0){
                    currentInventory.bendyPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("bendyPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }
                
            break;
            case "splitterPipe":
                if(currentInventory.splitterPipe > 0){
                    currentInventory.splitterPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("splitterPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }
                
            break;
            case "specialSplitterPipe":
                if(currentInventory.specialSplitterPipe > 0){
                    currentInventory.specialSplitterPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("specialSplitter"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }
               
            break;
            case "filterPipe":
                if(currentInventory.filterPipe > 0){
                    currentInventory.filterPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("filterPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }
                
            break;
            case "contaminatorPipe":
                if(currentInventory.contaminatorPipe > 0){
                    currentInventory.contaminatorPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("contaminatorPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }
                
            break;
            case "freezerPipe":
                if(currentInventory.freezePipe > 0){
                    currentInventory.freezePipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("freezePipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }
                
            break;
            case "heaterPipe":
                if(currentInventory.heatPipe > 0){
                    currentInventory.heatPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("heatPipe"),new Vector3 (11, 5, -1f),Quaternion.identity);
                }
                
            break;
            default:
                //shouldnt ever get here tbh
            break;
        }
        return null;
    }

    public void creditPipes(string type){
        // Debug.Log("HIT! should credit");
        switch(type){
            case "straightPipe":
                currentInventory.straightPipe++;
            break;
            case "bendyPipe":
                currentInventory.bendyPipe++;
            break;
            case "splitterPipe":
                currentInventory.splitterPipe++;
            break;
            case "specialSplitterPipe":
                currentInventory.specialSplitterPipe++;
            break;
            case "filterPipe":
                currentInventory.filterPipe++;
            break;
            case "contaminatorPipe":
                currentInventory.contaminatorPipe++;
            break;
            case "freezerPipe":
               currentInventory.freezePipe++;
            break;
            case "heaterPipe":
                currentInventory.heatPipe++;
            break;
            default:
                //shouldnt ever get here tbh
            break;
        }
        uiControl.updatePipeNumbersUI(currentInventory);
    }

    public inventorySpace.inventory returnInventory(){
        return currentInventory;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

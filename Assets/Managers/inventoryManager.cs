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
    [SerializeField] RectTransform canvas;

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
        Vector2 mouseLocalPos2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, Camera.main, out mouseLocalPos2);
        Vector3 mouseLocalPos3 = new Vector3(mouseLocalPos2.x, mouseLocalPos2.y, -2); // Note, this does not spawn the object in front of the button (looks weird for player)
        switch(type){
            case "straightPipe":
                if(currentInventory.straightPipe > 0){
                    currentInventory.straightPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("straightPipe"),mouseLocalPos3,Quaternion.identity);
                }

            break;
            case "bendyPipe":
                if(currentInventory.bendyPipe > 0){
                    currentInventory.bendyPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("bendyPipe"),mouseLocalPos3 ,Quaternion.identity);
                }
                
            break;
            case "splitterPipe":
                if(currentInventory.splitterPipe > 0){
                    currentInventory.splitterPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("splitterPipe"),mouseLocalPos3 ,Quaternion.identity);
                }
                
            break;
            case "specialSplitterPipe":
                if(currentInventory.specialSplitterPipe > 0){
                    currentInventory.specialSplitterPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("specialSplitter"),mouseLocalPos3 ,Quaternion.identity);
                }
               
            break;
            case "filterPipe":
                if(currentInventory.filterPipe > 0){
                    currentInventory.filterPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("filterPipe"),mouseLocalPos3 ,Quaternion.identity);
                }
                
            break;
            case "contaminatorPipe":
                if(currentInventory.contaminatorPipe > 0){
                    currentInventory.contaminatorPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("contaminatorPipe"),mouseLocalPos3 ,Quaternion.identity);
                }
                
            break;
            case "freezerPipe":
                if(currentInventory.freezePipe > 0){
                    currentInventory.freezePipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("freezePipe"),mouseLocalPos3 ,Quaternion.identity);
                }
                
            break;
            case "heaterPipe":
                if(currentInventory.heatPipe > 0){
                    currentInventory.heatPipe--;
                    uiControl.updatePipeNumbersUI(currentInventory);
                    return Instantiate(Resources.Load<GameObject>("heatPipe"),mouseLocalPos3 ,Quaternion.identity);
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

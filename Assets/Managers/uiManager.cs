using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject straightPipeButton;
    [SerializeField] GameObject bendyPipeButton;
    [SerializeField] GameObject filterPipeButton;
    [SerializeField] GameObject contaminatorPipeButton;
    [SerializeField] GameObject heaterPipeButton;
    [SerializeField] GameObject freezePipeButton;
    [SerializeField] GameObject splitterPipeButton;
    [SerializeField] GameObject specialSplitterPipeButton;

    inventoryManager inventoryControl;
    [SerializeField] RectTransform canvas;
    private RectTransform straightRT, bendyRT, filterRT, contaminatorRT, heaterRT, freezeRT, splitterRT, specialSplitterRT;
    private RectTransform[] buttonRT;
    private int inventoryStart_x, inventoryEnd_x, inventoryStart_y, inventoryEnd_y;
    void Awake(){
        inventoryControl = GameObject.FindGameObjectWithTag("inventoryManager").GetComponent<inventoryManager>();
        buttonRT = new RectTransform[8];
        buttonRT[0] = straightPipeButton.GetComponent<RectTransform>();
        buttonRT[1] = bendyPipeButton.GetComponent<RectTransform>();
        buttonRT[2] = filterPipeButton.GetComponent<RectTransform>();
        buttonRT[3] = contaminatorPipeButton.GetComponent<RectTransform>();
        buttonRT[4] = heaterPipeButton.GetComponent<RectTransform>();
        buttonRT[5] = freezePipeButton.GetComponent<RectTransform>();
        buttonRT[6] = splitterPipeButton.GetComponent<RectTransform>();
        buttonRT[7] = specialSplitterPipeButton.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updatePipeNumbersUI(inventorySpace.inventory currentInventory){
        straightPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.straightPipe.ToString();
        bendyPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.bendyPipe.ToString();
        splitterPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.splitterPipe.ToString();
        specialSplitterPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.specialSplitterPipe.ToString();
        filterPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.filterPipe.ToString();
        contaminatorPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.contaminatorPipe.ToString();
        freezePipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.freezePipe.ToString();
        heaterPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.heatPipe.ToString();
    }

    public void setInventoryPositions(int xStart, int yStart, int xEnd, int yEnd){
        inventoryStart_x = xStart;
        inventoryStart_y = yStart;
        inventoryEnd_x = xEnd;
        inventoryEnd_y = yEnd;
        setPipeButtonPosition();
    }

    public void setPipeButtonPosition(){
        int count = 0; 
                    
        for(int y = inventoryEnd_y - 1; y > inventoryStart_y; y--){
            for(int x = inventoryStart_x; x < inventoryEnd_x; x++){
                Vector2 inventoryPos = new Vector2(x,y);
                if(count < 8){ //NB NB NB NB THIS MAGIC NUMBER NEEDS TO BE CHANGED
                    Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, inventoryPos);                 
                    Vector2 anchoredPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, Camera.main, out anchoredPos);
                    buttonRT[count].anchoredPosition = anchoredPos;
                    Debug.Log($"ANCHORED POS: {anchoredPos}");
                }
                else{
                    break;
                }
                count++;
            }
        }       
    }
}

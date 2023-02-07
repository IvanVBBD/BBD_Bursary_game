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

    [SerializeField] GameObject resetButton;

    [SerializeField] GameObject waterLevelLabel;


    //PANEL SECTION OF CODE
    [SerializeField] GameObject winnerPannel;

    [SerializeField] GameObject toolTipPanel;

    inventoryManager inventoryControl;
    [SerializeField] RectTransform canvas;
    private RectTransform straightRT, bendyRT, filterRT, contaminatorRT, heaterRT, freezeRT, splitterRT, specialSplitterRT;
    private RectTransform[] buttonRT;
    private int inventoryStart_x, inventoryEnd_x, inventoryStart_y, inventoryEnd_y;
    void Awake(){
        inventoryControl = GameObject.FindGameObjectWithTag("inventoryManager").GetComponent<inventoryManager>();
        initButtonRT();
        setWaterLabel();
    }

    void initButtonRT(){
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

    public void toggleWinPanel(){
        switch(winnerPannel.activeInHierarchy){
            case true:
            winnerPannel.SetActive(false);
            break;
            case false:
            winnerPannel.SetActive(true);
            break;
        }
    }

    public void enableToolTip(string _content){
        toolTipPanel.SetActive(true);
        toolTipPanel.GetComponentInChildren<TextMeshProUGUI>().text = _content;
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas,Input.mousePosition, Camera.main, out anchoredPos);
        toolTipPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(anchoredPos.x,anchoredPos.y,-5f);

        //toolTipPanel.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,-2);
    }

    public void disableToolTip(){
        toolTipPanel.SetActive(false);
    }

    // Update is called once per frame

    void setWaterLabel(){
        waterLevelLabel.GetComponent<TextMeshProUGUI>().text = "Balance: " + GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>().returnWaterDirtLevel().ToString();
    }

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
        updateButtonGreyScale(currentInventory);

        
    }

    void updateButtonGreyScale(inventorySpace.inventory currentInventory){
        if(currentInventory.straightPipe == 0){
            straightPipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            straightPipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        if(currentInventory.bendyPipe == 0){
            bendyPipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            bendyPipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        if(currentInventory.splitterPipe == 0){
            splitterPipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            splitterPipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        if(currentInventory.specialSplitterPipe == 0){
            specialSplitterPipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            specialSplitterPipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        if(currentInventory.filterPipe == 0){
            filterPipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            filterPipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        if(currentInventory.contaminatorPipe == 0){
            contaminatorPipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            contaminatorPipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        if(currentInventory.freezePipe == 0){
            freezePipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            freezePipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        if(currentInventory.heatPipe == 0){
            heaterPipeButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }else{
            heaterPipeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void setInventoryPositions(int xStart, int yStart, int xEnd, int yEnd){
        inventoryStart_x = xStart;
        inventoryStart_y = yStart;
        inventoryEnd_x = xEnd;
        inventoryEnd_y = yEnd;
        setPipeButtonPosition(); // This does not work :( 
    }

    public void setPipeButtonPosition(){
        int count = 0; 
                    
        // Calculating the current screen width of ONE inventory block (according to our current camera position and size)
        Vector2 tempScreenPos1 = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector2(0,0));                 
        Vector2 tempAnchoredPos1;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, tempScreenPos1, Camera.main, out tempAnchoredPos1);
        Vector2 tempScreenPos2 = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector2(1,0));                 
        Vector2 tempAnchoredPos2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, tempScreenPos2, Camera.main, out tempAnchoredPos2);
        float inventoryWidth = tempAnchoredPos2.x - tempAnchoredPos1.x;

        for(int y = inventoryEnd_y - 1; y > inventoryStart_y; y--){
            for(int x = inventoryStart_x; x < inventoryEnd_x; x++){
                Vector2 inventoryPos = new Vector2(x,y);
                if(count < 8){ //NB NB NB NB THIS MAGIC NUMBER NEEDS TO BE CHANGED
                    Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, inventoryPos);                 
                    Vector2 anchoredPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, Camera.main, out anchoredPos);
                    buttonRT[count].anchoredPosition = anchoredPos;
                    buttonRT[count].sizeDelta = new Vector2(0.9f*inventoryWidth, 0.9f*inventoryWidth);
                }
                else{
                    break;
                }
                count++;
            }
        }       
    }
}

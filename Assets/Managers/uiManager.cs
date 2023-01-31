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
    [SerializeField] GameObject specialSplitterButton;

    inventoryManager inventoryControl;

    void Awake(){
        inventoryControl = GameObject.FindGameObjectWithTag("inventoryManager").GetComponent<inventoryManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updatePipeNumbersUI(inventorySpace.inventory currentInventory){
        straightPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.straightPipe.ToString();
        bendyPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.bendyPipe.ToString();
        splitterPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.splitterPipe.ToString();
        specialSplitterButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.specialSplitterPipe.ToString();
        filterPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.filterPipe.ToString();
        contaminatorPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.contaminatorPipe.ToString();
        freezePipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.freezePipe.ToString();
        heaterPipeButton.GetComponentInChildren<TextMeshProUGUI>().text = currentInventory.heatPipe.ToString();
    }
}

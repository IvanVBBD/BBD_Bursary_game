using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class toolTip : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    uiManager uiControl;
    [SerializeField] private string description;

    void Awake(){
        uiControl = GameObject.FindGameObjectWithTag("uiManager").GetComponent<uiManager>();

    }
    private bool mouse_over = false;

 
     public void OnPointerEnter(PointerEventData eventData)
     {
         mouse_over = true;
         uiControl.enableToolTip(description);
     }
 
     public void OnPointerExit(PointerEventData eventData)
     {
         mouse_over = false;
         uiControl.disableToolTip();
     }
}

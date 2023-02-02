using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragAndDrop : MonoBehaviour
{
    // Start is called before the first frame update
    private bool drag = false;
    static bool canDrag = true;

    private gridManager gridControl;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }
    void Start()
    {
        
    }

    void OnMouseDown(){
        if(!drag && canDrag ){
            drag = true;
            canDrag = false;
            gridControl.setPickUpObject(this.gameObject);
        }else if(drag){
            drag = false;
            canDrag = true;
            gridControl.snapToGrid();
        }
    }

    // Update is called once per frame
    void checkDrag(){
        if(drag){
              Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.transform.position = new Vector3(mousePosition.x,mousePosition.y,-1f);
        }
    }
    void Update()
    {
        checkDrag();
    }

    public void setFirstPickup(){
        Debug.Log($"DRAG: {drag}");
        drag = true;
        gridControl.setPickUpObject(this.gameObject);

        Debug.Log("WHY");
    }
}

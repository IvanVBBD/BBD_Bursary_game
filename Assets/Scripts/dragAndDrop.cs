using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragAndDrop : MonoBehaviour
{
    // Start is called before the first frame update
    static bool canDrag = true;
    private bool drag = false;

    private gridManager gridControl;

    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }
    void Start()
    {
        
    }

    void OnMouseDown(){
        if(canDrag && !drag ){
            canDrag = false;
            drag = true;
            gridControl.setPickuPObject(this.gameObject);
        }else if( !canDrag && drag){
            canDrag = true;
            drag = false;
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
}

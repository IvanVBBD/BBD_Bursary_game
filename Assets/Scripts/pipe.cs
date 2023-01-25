using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipe : MonoBehaviour
{
    // Start is called before the first frame update

    private gridManager gridControl;

     enum DIRECTIONS{
        left,
        right,
        up,
        down
    };

    private DIRECTIONS[] allowedDirections = {DIRECTIONS.left, DIRECTIONS.right};
   


    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }


    void Start()
    {
       foreach(var item in allowedDirections){
            Debug.Log(item);
       }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

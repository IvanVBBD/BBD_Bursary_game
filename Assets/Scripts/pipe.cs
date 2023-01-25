using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipe : MonoBehaviour
{
    // Start is called before the first frame update

    private gridManager gridControl;
    [SerializeField] private Vector2[] allowedDirections;

    enum DIRECTION {
        LEFT,
        RIGHT,
        UP,
        DOWN
    };


    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2[] returnPipeDirections(){
        return allowedDirections;
    }

}

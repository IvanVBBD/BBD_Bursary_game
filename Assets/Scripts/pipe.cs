using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using waterSpace;

public class pipe : MonoBehaviour
{
    // Start is called before the first frame update

    private gridManager gridControl;
    [SerializeField] private Vector2[] allowedDirections;
    [SerializeField] private bool isBalanceSplitter = false;

    [SerializeField]private Vector2 cleanDirection;
    [SerializeField]private Vector2 dirtyDirection;
    [SerializeField] private string pipeState;



    void Awake(){
        gridControl = GameObject.FindGameObjectWithTag("gridManager").GetComponent<gridManager>();
    }

    void rotate(){
        if(Input.GetMouseButtonDown(1) && this.gameObject == gridControl.returnCurrentPickUp()){
            Debug.Log("=========================");
            for(int index = 0; index < allowedDirections.Length; index++){
                Debug.Log($"old direction: {allowedDirections[index]} ");
                Vector2 temp = rotate(allowedDirections[index], -1f * Mathf.PI/ 2);
                temp.x = Mathf.RoundToInt(temp.x);
                temp.y = Mathf.RoundToInt(temp.y);
                allowedDirections[index] = temp;
                Debug.Log($"new direction: {allowedDirections[index]} ");
            }
            if(isBalanceSplitter){
                Vector2 temp = rotate(cleanDirection,-1f * Mathf.PI/ 2);
                temp.x = Mathf.RoundToInt(temp.x);
                temp.y = Mathf.RoundToInt(temp.y);
                cleanDirection = temp;
                temp = rotate(dirtyDirection,-1f * Mathf.PI/ 2);
                temp.x = Mathf.RoundToInt(temp.x);
                temp.y = Mathf.RoundToInt(temp.y);
                dirtyDirection = temp;
            }
            
            gameObject.transform.Rotate(0,0,-90);
        }
    }

    public static Vector2 rotate(Vector2 v, float delta) {
    return new Vector2(
        v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
        v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
    );
    }

    // Update is called once per frame
    void Update()
    {
        rotate();
    }

    public string returnPipeEffect() => pipeState;

    public bool returnIsBalanceSplitter() => isBalanceSplitter;
 
    public Vector2[] returnPipeDirections() => allowedDirections;

    public Vector2[] returnPipeBalanceDirections() {
        Vector2[] tempReturn = {cleanDirection,dirtyDirection};
        return tempReturn;
    }

}

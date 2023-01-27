using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace waterSpace
    {
        public enum waterStates
        {
            STEAM,
            WATER,
            ICE,
        }
    }





public class waterManager : MonoBehaviour
{
    // Start is called before the first frame update
    private waterSpace.waterStates waterPhaseState = waterSpace.waterStates.WATER;
    [SerializeField]private float WaterDirtState = 0f;
    
    void Start()
    {
        
    }

    public waterSpace.waterStates returnWaterPhaseState(){
        return waterPhaseState;
    }

    public bool canMoveDirection(Vector2 _direction){
        Debug.Log($"Direction Recieved: {_direction}");
        Debug.Log($"What we need {Vector2.up}");
        Debug.Log($"Water State: {waterPhaseState}");
        switch(waterPhaseState){
            case waterSpace.waterStates.STEAM:
            if(_direction == Vector2.right || _direction == Vector2.left || _direction == Vector2.up){
                
                Debug.Log("STEAM TRUE");
                return true;
            }else{
                return false;
            }
            case waterSpace.waterStates.WATER:
            if(_direction == Vector2.right || _direction == Vector2.left || _direction == Vector2.down){
                return true;
            }else{
                return false;
            }
            case waterSpace.waterStates.ICE:
            return false;
            default:
            return false;

        }
    }

    public void alterWaterPhaseState(string _condition){
          switch(_condition){
                case "NONE":
                break;
                case "FREEZE":
                if(returnWaterPhaseState() == waterSpace.waterStates.WATER){
                    changeWaterPhaseState(waterSpace.waterStates.ICE);
                }else if(returnWaterPhaseState() == waterSpace.waterStates.STEAM){
                    changeWaterPhaseState(waterSpace.waterStates.WATER);
                }else{
                    changeWaterPhaseState(waterSpace.waterStates.ICE);
                }
                break;
                case "HEAT":
                    if(returnWaterPhaseState() == waterSpace.waterStates.WATER){
                    changeWaterPhaseState(waterSpace.waterStates.STEAM);
                }else if(returnWaterPhaseState() == waterSpace.waterStates.ICE){
                    changeWaterPhaseState(waterSpace.waterStates.WATER);
                }else{
                    changeWaterPhaseState(waterSpace.waterStates.STEAM);
                }
                break;
            }
            Debug.Log($"water state is now: {waterPhaseState}");
    }

    public float returnWaterDirtState(){
        return WaterDirtState;
    }

    public void changeWaterPhaseState(waterSpace.waterStates _state){
        waterPhaseState = _state;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

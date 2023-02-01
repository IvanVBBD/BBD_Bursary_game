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

        public struct waterObject {
        public float waterDirtState;
        public waterSpace.waterStates waterPhaseState;
        }
    }
public class waterManager : MonoBehaviour
{
    // Start is called before the first frame update
    private waterSpace.waterObject originalWater;
    void Awake(){
        originalWater.waterDirtState = 0f;
        originalWater.waterPhaseState = waterSpace.waterStates.WATER;
    }
    public bool canMoveDirection(Vector2 _direction, waterSpace.waterObject water){
        switch(water.waterPhaseState){
            case waterSpace.waterStates.STEAM:
            if(_direction == Vector2.right || _direction == Vector2.left || _direction == Vector2.up){
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

    public waterSpace.waterObject alterWaterPhaseState(string _condition, waterSpace.waterObject water){
          switch(_condition){
                case "NONE":
                break;
                case "FREEZE":
                if(water.waterPhaseState == waterSpace.waterStates.WATER){
                    water.waterPhaseState = waterSpace.waterStates.ICE;
                }else if(water.waterPhaseState == waterSpace.waterStates.STEAM){
                    water.waterPhaseState = waterSpace.waterStates.WATER;
                }else{
                    water.waterPhaseState = waterSpace.waterStates.ICE;
                }
                break;
                case "HEAT":
                    if(water.waterPhaseState == waterSpace.waterStates.WATER){
                    water.waterPhaseState = waterSpace.waterStates.STEAM;
                }else if(water.waterPhaseState == waterSpace.waterStates.ICE){
                    water.waterPhaseState = waterSpace.waterStates.WATER;
                }else{
                    water.waterPhaseState = waterSpace.waterStates.STEAM;
                }
                break;
                case "FILTER":
                    water.waterDirtState -= 1;
                break;
                case "CONTAMINATOR":
                    water.waterDirtState += 1;
                break;
            }
            // Debug.Log($"water state is now: {water.waterPhaseState}");
            return water;
    }

     public waterSpace.waterObject issueFreshWaterState() => originalWater;

}

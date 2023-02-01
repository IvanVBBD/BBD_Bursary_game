using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class serverChallenge{
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public int waterData;
}
public class challengeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static challengeManager instance;
    Vector2 startPos, endPos;
    int waterDirtLevel = 0;
   

   void Awake(){
        if(instance != null){
            Destroy(gameObject);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    
    startPos = Vector2.zero;
    endPos = Vector2.zero;
   }


   public void setStartPos(int _x, int _y){
        startPos = new Vector2(_x,_y);   
   }

    public Vector2 returnStartPos(){
        return startPos;
    }


    public void setWaterDirtLevel(int _level){
        waterDirtLevel = _level;
    }

    public int returnWaterDirtLevel(){
        return waterDirtLevel;
    }

    public void setEndPos(int _x, int _y){
        endPos = new Vector2(_x,_y);
    }

    public Vector2 returnEndPos(){
        return endPos;
    }

    public void setUpChallenge(string _input){
        serverChallenge _challenge = JsonUtility.FromJson<serverChallenge>(_input);
        this.setStartPos(_challenge.startX,_challenge.startY);
        this.setEndPos(_challenge.endX,_challenge.endY);
        this.setWaterDirtLevel(_challenge.waterData);
        SceneManager.LoadScene(1);
    }
}

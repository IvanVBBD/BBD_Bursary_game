using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace statSpace{
    public struct levelStats{
        public float timeTaken;
        public int mousePresses;
        public int pipeUsed;
    }
}

public class statManager : MonoBehaviour
{
    // Start is called before the first frame update
    private float timeTaken = 0f;
    private int mousePresses = 0;
    void Start()
    {
        
    }

    void tickTime(){
        timeTaken += Time.deltaTime;
    }

    void detectMouseClick(){
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            mousePresses++;
        }
    }

    public statSpace.levelStats returnLevelStats(){
        statSpace.levelStats currentStats = new statSpace.levelStats();
        currentStats.timeTaken = this.timeTaken;
        currentStats.mousePresses = this.mousePresses;
        currentStats.pipeUsed = GameObject.FindGameObjectsWithTag("pipe").Length;
        return currentStats;
    }

    // Update is called once per frame
    void Update()
    {
        tickTime();
        detectMouseClick();
    }
}

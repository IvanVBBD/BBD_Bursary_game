using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static soundManager instance;
    GameObject soundSource;
    
    void Awake(){
        init();
    }
    void Start()
    {
        
    }

    void init(){
         if(instance != null){
            Destroy(gameObject);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        soundSource = Resources.Load<GameObject>("Sounds/soundSource");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

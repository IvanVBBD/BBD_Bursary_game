using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class webManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static webManager instance;
    challengeManager challengeControl;
    const string url = "http://127.0.0.1:3002/bursar/requestchallenge";
   
    void Awake(){
        if(instance != null){
            Destroy(gameObject);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        challengeControl = GameObject.FindGameObjectWithTag("challengeManager").GetComponent<challengeManager>();
    }

    public string fetch(string url,string method){
        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
        myReq.Method = method;
        WebResponse webResponse = myReq.GetResponse();
        System.IO.Stream webStream = webResponse.GetResponseStream();
        System.IO.StreamReader reader = new System.IO.StreamReader(webStream);
        string data = reader.ReadToEnd();
        reader.Close();
        webStream.Close();
        webResponse.Close();
        return data;
    }


    public void startGame(){
        string response = fetch(url,"GET");
        Debug.Log(response);
        challengeControl.setUpChallenge(response);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

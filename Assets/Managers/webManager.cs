using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System.Net.Http;  
using System.Net.Http.Headers;

 struct requestWrapper
                {
                    public string ID;
                    public float timeTaken;
                    public int mousePresses;
                    public int pipeUsed;
                }
public class webManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static readonly HttpClient client = new HttpClient();
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

    public string GET(string url){
        
        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
        myReq.Method = "GET";
        WebResponse webResponse = myReq.GetResponse();
        System.IO.Stream webStream = webResponse.GetResponseStream();
        System.IO.StreamReader reader = new System.IO.StreamReader(webStream);
        string data = reader.ReadToEnd();
        reader.Close();
        webStream.Close();
        webResponse.Close();
        return data;
       
    }
    

    public async void levelRequest(string url){
            requestWrapper _requestWrapper = new requestWrapper();
            statSpace.levelStats stats = collectStats();
            _requestWrapper.ID = "IVAN";
            _requestWrapper.timeTaken = stats.timeTaken;
            _requestWrapper.mousePresses = stats.mousePresses;
            _requestWrapper.pipeUsed = stats.pipeUsed;
        var content = new StringContent(JsonUtility.ToJson(_requestWrapper), Encoding.UTF8,"application/json");
        content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
        var response = await client.PostAsync(url,content);
        string responseString = await response.Content.ReadAsStringAsync();
        challengeControl.setUpChallenge(responseString);

    }

    statSpace.levelStats collectStats(){
        if(GameObject.FindGameObjectWithTag("statManager")){
            return GameObject.FindGameObjectWithTag("statManager").GetComponent<statManager>().returnLevelStats();
        }
        else{
            return new statSpace.levelStats();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

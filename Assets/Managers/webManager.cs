using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;

public class backendResponse
{
    public string[,] grid;
    public string[] start;
    public string[] end;

    public string[] inventory;

}
public class webManager : MonoBehaviour
{
    // Start is called before the first frame update
    const string url = "http://127.0.0.1:3002/bursar/challenge";
    void Start()
    {
        
        
        
    }


    public string fetch(string url,string method){
        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
        myReq.Method = method;
        WebResponse webResponse = myReq.GetResponse();
        System.IO.Stream webStream = webResponse.GetResponseStream();
        System.IO.StreamReader reader = new System.IO.StreamReader(webStream);
        string data = reader.ReadToEnd();
        return data;
    }


    public void startGame(){
        string response = fetch(url,"GET");
        // Debug.Log(response);
        backendResponse joke = JsonUtility.FromJson<backendResponse>(response);
        // Debug.Log(joke.inventory[0]);
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

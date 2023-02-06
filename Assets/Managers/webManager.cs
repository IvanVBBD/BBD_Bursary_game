using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;

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
    public void levelRequest(string url){
        StartCoroutine(Upload(url));
    }

    IEnumerator Upload(string url)
    {
        statSpace.levelStats stats = collectStats();
        UnityWebRequest request = new UnityWebRequest(url,"POST");
        request.SetRequestHeader("Content-Type","application/json");
        requestWrapper _requestWrapper = new requestWrapper();
        _requestWrapper.ID = "IVAN";
        _requestWrapper.timeTaken = stats.timeTaken;
        _requestWrapper.mousePresses = stats.mousePresses;
        _requestWrapper.pipeUsed = stats.pipeUsed;
        string jsonString = JsonUtility.ToJson(_requestWrapper);
        byte[] rawJsonData = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(rawJsonData);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        switch(request.result){
            case UnityWebRequest.Result.Success:
            string response = request.downloadHandler.text;
            Debug.Log("HOLY");
            request.Dispose();
            challengeControl.setUpChallenge(response);

            break;
            default:
            Debug.Log("ayyy it break");
            break;
        }


    }

    statSpace.levelStats collectStats(){
        if(GameObject.FindGameObjectWithTag("statManager")){
            return GameObject.FindGameObjectWithTag("statManager").GetComponent<statManager>().returnLevelStats();
        }
        else{
            return new statSpace.levelStats();
        }
    }
}

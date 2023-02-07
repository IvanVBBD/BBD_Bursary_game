using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    bool initSound = false;
    AudioSource self;

    void Awake(){
        self = GetComponent<AudioSource>();
    }
    void Start()
    {

    }

    public void init(AudioClip _clip){
        initSound = true;
        self.clip = _clip;
        self.Play();
    }

    void DestroyOnDone(){
        if(initSound && !self.isPlaying){
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOnDone();
    }
}

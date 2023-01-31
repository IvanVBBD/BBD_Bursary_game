using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditInventory : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private string credits;
    void Start()
    {
        
    }

    public string returnCreditType(){
        return credits;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

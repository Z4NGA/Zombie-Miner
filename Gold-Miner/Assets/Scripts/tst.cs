using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tst : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("player name : " + PlayerPrefs.GetString("user_name"));    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

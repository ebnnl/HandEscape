using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ChangeScene : MonoBehaviour
{
    //public TMP_Text debug;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("Colliding "+other.name);
        //debug.text = "Colliding " + other.gameObject;
        // if(other.gameObject.name == "Key"){
        //     SceneManager.LoadScene("ElasticHands");
        // }
    }
}

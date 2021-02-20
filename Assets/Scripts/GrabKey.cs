using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GrabKey : MonoBehaviour
{
    public OVRHand rightHand;
    private bool isColliding;
    private bool isGrabbing;

    private Vector3 offset;
    private Quaternion rotationOffset;

    public int level;


    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
        isGrabbing = false;
    }

    // Update is called once per frame
    void Update()
    {        
        if(isColliding && rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index)){
            //Gtabbing the key
            isGrabbing = true;
            this.transform.parent = rightHand.transform;

        
        }
        if(isGrabbing && !rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index)){
            //Releasing the key
            isGrabbing = false;
            this.transform.parent = null;
        }
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.name=="Hand_Start_CapsuleCollider"){
            isColliding = true;

        }
        else if (other.gameObject.name == "Key"){
            //Changing scene
            if (level == 1)
            {
                SceneManager.LoadScene("CopyHands");
            }
            else if (level == 2)
            {
                SceneManager.LoadScene("FinalLevel");
            }
            else if (level == 3)
            {
                SceneManager.LoadScene("FreeScene");
            }
            
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.name=="Hand_Start_CapsuleCollider"){
            isColliding = false;
        }
    }
}

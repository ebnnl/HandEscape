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


    //public TMP_Text debug;
    //public TMP_Text debug2;



    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
        isGrabbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //debug.text = "Hand piching: "+ rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) + "isColliding: " + isColliding + "Is grabbing: " + isGrabbing;
        
        if(isColliding && rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index)){
            isGrabbing = true;
            this.transform.parent = rightHand.transform;

        
        }
        if(isGrabbing && !rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index)){
            isGrabbing = false;
            this.transform.parent = null;
        }
        // if(isGrabbing){
        //     this.transform.position = rightHand.transform.position+offset;
        //     this.transform.localRotation = rightHand.transform.localRotation * rotationOffset;
        // }
        
    }

    void OnTriggerEnter(Collider other){
        //debug2.text = "Is Entering"+ other.gameObject.name;
        if(other.gameObject.name=="Hand_Start_CapsuleCollider"){
            isColliding = true;

        }
        else if (other.gameObject.name == "Key"){
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

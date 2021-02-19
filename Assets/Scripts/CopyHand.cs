using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyHand : MonoBehaviour
{
    public GameObject handToCopy;
    public OVRHand rightHand;
    public OVRHand leftHand;
    //public GameObject toCopy;

    public List<GameObject> copiedHands;
    public Material currentHandMaterial;
    public Material handMaterial;
    public int c = 0;
    public int currentHand = 0;
    public int level;
    public GestureRecognizer gestureRecognizer;

    public float selectionDistance;

    public bool ringPinching = false;

    void Start()
    {
        copiedHands = new List<GameObject>();
        //copiedHands.Add(handToCopy);
        changeHandColor(0, handMaterial);
    }

    void Update()
    {
        // Copy hand
        if (OVRInput.GetDown(OVRInput.Button.Three) && !leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            Debug.Log("pinch");
            Vector3 pos = handToCopy.transform.position;
            Quaternion rot = handToCopy.transform.rotation;
            GameObject hand = Instantiate(handToCopy, pos, rot);
            copiedHands.Add(hand);
        }

        // Select closest hand
        if (!leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) && level!=3)
        {
            float distanceMin = selectionDistance;
            int distanceMinIndex = 0;
            for (int i = 0; i < copiedHands.Count; i++)
            {
                float distance = (copiedHands[i].transform.position - handToCopy.transform.position).magnitude;
                //Debug.Log(distance);
                if (distance < distanceMin)
                {
                    distanceMinIndex = i;
                    distanceMin = distance;
                }
            }
            if (copiedHands.Count > 0)
            {
                changeCurrentHand(distanceMinIndex);
            }
           
        }

        // Move selected hand
        if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) && copiedHands.Count > 0 && level!=3)
        {
            copiedHands[currentHand].transform.parent = handToCopy.transform;
        }
        else
        {
            if (copiedHands.Count > 0)
            {
                copiedHands[currentHand].transform.parent = null;
            }
            
        }

        // Remove selected hand
        if ( !ringPinching && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) && copiedHands.Count > 0 && level!=3)
        {
            Debug.Log("remove " + currentHand.ToString());
            copiedHands[currentHand].SetActive(false);
            copiedHands.Remove(copiedHands[currentHand]);
            currentHand = 0;
        }
        ringPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring);
       
    }

    public void changeCurrentHand(int i)
    {
        changeHandColor(currentHand, handMaterial);
        currentHand = i;
        changeHandColor(currentHand, currentHandMaterial);
    }

    public void changeHandColor(int i, Material material)
    {
        SkinnedMeshRenderer renderer = copiedHands[i].GetComponent<SkinnedMeshRenderer>();
        Material[] mats = renderer.materials;
        mats[1] = material;
        renderer.materials = mats;
    }


}
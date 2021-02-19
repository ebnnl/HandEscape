using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public struct Gesture {
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class GestureRecognizer : MonoBehaviour
{
    public GameObject rightHand;
    public OVRSkeleton skeletonR;
    public OVRSkeleton skeletonL;
    private List<OVRBone> fingerBonesR;
    private List<OVRBone> fingerBonesL;

    public Transform handAnchorR;
    public Vector3 positionDifference;
    public Vector3 handPosition;
    public float distance;
    public float amp = 1.0f;

    public List<Gesture> gestures;
    public float threshold;
    private bool isExtended;

    public TMP_Text debug;
    public TMP_Text debug2;

    private float speed = 0.01f;

    
    // Start is called before the first frame update
    void Start()
    {
        fingerBonesR = new List<OVRBone>(skeletonR.Bones);
        fingerBonesL = new List<OVRBone>(skeletonL.Bones);

        isExtended = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        Gesture currentGestureR = isRecognized(skeletonR,fingerBonesR);
        Gesture currentGestureL = isRecognized(skeletonL,fingerBonesL);

        if (fingerBonesR.Count ==0 || fingerBonesL.Count == 0)
        {
            fingerBonesR = new List<OVRBone>(skeletonR.Bones);
            fingerBonesL = new List<OVRBone>(skeletonL.Bones);
            positionDifference = new Vector3(0, 0, 0);
            rightHand.transform.position = handAnchorR.position;
            handPosition = handAnchorR.position;
        }

        Plane handPlane = new Plane();
        //Debug.Log(fingerBonesR.Count);
        handPlane.Set3Points(fingerBonesR[6].Transform.position, fingerBonesR[0].Transform.position, fingerBonesR[10].Transform.position);

        rightHand.transform.rotation = handAnchorR.rotation;
        distance = positionDifference.magnitude;
        Vector3 handTranslation = handPosition - handAnchorR.position;
        Vector3 handTranslationRelative = rightHand.transform.InverseTransformPoint(handTranslation);
        float newXComponent = rightHand.transform.InverseTransformPoint(new Vector3(0,0,0)).x;
        Vector3 handTranslation2 = rightHand.transform.TransformPoint(new Vector3(newXComponent, handTranslationRelative.y, handTranslationRelative.z));
        rightHand.transform.position -= handTranslation + handTranslation*distance*amp;
        handPosition = handAnchorR.position;



        if (currentGestureR.name == "HandOpen")
        {
            isExtended = true;
            rightHand.transform.parent = null;
            //skeletonR.transform.position = skeletonR.transform.position + handPlane.normal*speed;
            positionDifference = positionDifference + handPlane.normal * speed;
            rightHand.transform.position += handPlane.normal * speed;

            Vector3 start = fingerBonesR[15].Transform.position;

            //RayCast to floor
            RaycastHit hit;
            Vector3 end;
            if (Physics.Raycast(start, handPlane.normal, out hit, Mathf.Infinity)) end = hit.point;
            else end = start + handPlane.normal*10.0f;
            
            DrawLine(start, end);

        
        }
        else if(currentGestureR.name == "Palm" && isExtended)
        {
            Vector3 direction = Vector3.Normalize(handAnchorR.position - skeletonR.transform.position);
            //skeletonR.transform.position = skeletonR.transform.position + direction*speed;
            positionDifference = positionDifference + direction * speed;
            rightHand.transform.position += direction * speed;
        }
        if(currentGestureL.name == "CloseFist")
        {
            //skeletonR.transform.position = handAnchorR.position;
            positionDifference = new Vector3( 0, 0, 0 );
            rightHand.transform.position = handAnchorR.position;
        }
        



        //Text to get a new gesture
        // string test = "";
        // foreach(var bone in fingerBonesR)
        // {
        //     Vector3 currentData = skeletonR.transform.InverseTransformPoint(bone.Transform.position);
        //     test += " " + currentData.ToString("F4") + " - ";
        // }
        // debug2.text = fingerBonesR.Count.ToString() + " start:" + test;
    }

    Gesture isRecognized(OVRSkeleton skeleton,List<OVRBone> fingerBones)
    {
        Gesture currentGesture = new Gesture();
        float minimumSumDistance = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0.0f;
            bool isDiscarded = false;

            for (int bone_it = 0 ; bone_it < fingerBones.Count ; bone_it++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[bone_it].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[bone_it]);
                if(distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            } 

            if(!isDiscarded && sumDistance < minimumSumDistance)
            {
                minimumSumDistance = sumDistance;
                currentGesture = gesture;
            }
        }
        
        return currentGesture;
    }

    void DrawLine(Vector3 start, Vector3 end, float duration = 0.05f)
    {
        GameObject rayLine = new GameObject();
        rayLine.transform.position = start;
        rayLine.AddComponent<LineRenderer>();
        LineRenderer lr = rayLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        //Set a gradient of color
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.blue, 0.5f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.7f, 0.0f), new GradientAlphaKey(0.7f, 1.0f) }
        );
        lr.colorGradient = gradient;        
        lr.SetWidth(0.001f, 0.001f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(rayLine, duration);
    }
}

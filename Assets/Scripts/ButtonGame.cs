using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonGame : MonoBehaviour
{
    public List<GameObject> buttonDots;
    public GameObject goSignal;

    public GameObject copyText;
    public GameObject moveText;
    public GameObject removeText;
    public GameObject index;
    private GameObject middle;
    private GameObject ring;
    public GameObject camera;

    public GameObject pointText;
    public GameObject tooSoonText;
    public GameObject winText;
    public TMP_Text scoreText;
    private bool win = false;
    private bool wereActive = false;
    public GameObject key;

    private float timer;
    public float signalTime = 5.0f;
    public float signalLenght = 2.0f;
    private bool signalActive;
    private int score = 0;
    public int maxScore = 10;

    void Start()
    {
        for(int i=0; i<buttonDots.Count; i++)
        {
            buttonDots[i].SetActive(false);
        }
        signalActive = false;
        winText.SetActive(false);
        timer = 0;
    }

    void Update()
    {
        if (index == null || middle == null || ring == null )
        {
            index = GameObject.Find("OVRHandPrefab_L/Bones/Hand_Start/Hand_Index1/Hand_Index2/Hand_Index3/Hand_IndexTip");
            middle = GameObject.Find("OVRHandPrefab_L/Bones/Hand_Start/Hand_Middle1/Hand_Middle2/Hand_Middle3/Hand_MiddleTip");
            ring = GameObject.Find("OVRHandPrefab_L/Bones/Hand_Start/Hand_Ring1/Hand_Ring2/Hand_Ring3/Hand_RingTip");
        }

        if (index != null)
        {
            copyText.transform.parent = index.transform;
        }
        if (middle != null)
        {
            moveText.transform.parent = middle.transform;
        }
        if (ring != null)
        {
            removeText.transform.parent = ring.transform;
        }

        if (buttonDots[0].active && buttonDots[1].active && buttonDots[2].active && buttonDots[3].active && signalActive)
        {
            if (!wereActive)
            {
                pointText.SetActive(true);
                score += 1;
                scoreText.text = "Score : " + score.ToString() + "/" + maxScore.ToString();
                signalActive = false;
                timer = 0;
            }
            else
            {
                tooSoonText.SetActive(true);
            }
        }
        else
        {
            pointText.SetActive(false);
            tooSoonText.SetActive(false);
        }

        if (score >= maxScore)
        {
            winText.SetActive(true);
            win = true;
            key.SetActive(true);
        }

        // Update timer
        timer += Time.deltaTime;
        if (signalActive && timer > signalLenght)
        {
            timer = 0;
            signalActive = false;
            signalTime = Random.Range(1.0f, 4.0f);
        }
        if (!signalActive && timer > signalTime)
        {
            timer = 0;
            signalActive = true;
            wereActive = (buttonDots[0].active && buttonDots[1].active && buttonDots[2].active && buttonDots[3].active);
        }
        goSignal.SetActive(signalActive);

    }

    public void buttonPressed(int button)
    {
        if (button < buttonDots.Count)
        {
            buttonDots[button].SetActive(true);
        }
    }

    public void buttonReleased(int button)
    {
        if (button < buttonDots.Count)
        {
            buttonDots[button].SetActive(false);
        }
    }

}
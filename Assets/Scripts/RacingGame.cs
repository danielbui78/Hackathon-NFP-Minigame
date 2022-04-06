using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingGame : MonoBehaviour
{
    public float fStopWatchTime = 0.0f;
    public float fCountDownToStart = 3.0f;
    public bool bRaceStarted = false;
    public TMPro.TextMeshProUGUI StopWatchText;
    private TMPro.TextMeshProUGUI ReadySetGoText;
    public GameObject ReadySetGoObject;
    //    public GameObject StopWatchTimerObject;

    public bool bRaceFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        fCountDownToStart = 3.0f;

//        if (StopWatchTimerObject != null)
//            StopWatchText = StopWatchTimerObject.GetComponent<TMPro.TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if (bRaceStarted == false)
        {
            if (fCountDownToStart <= 1.0f)
            {
                if (ReadySetGoText != null)
                {
                    ReadySetGoText.text = "Go!";
                }

            }
            else if (fCountDownToStart <= 2.0f)
            {
                if (ReadySetGoText != null)
                {
                    ReadySetGoText.text = "Set...";
                }

            }
            else if (fCountDownToStart >= 3.0f)
            {
                if (ReadySetGoObject != null)
                {
                    ReadySetGoObject.SetActive(true);
                    ReadySetGoText = ReadySetGoObject.GetComponent<TMPro.TextMeshProUGUI>();
                }
                if (ReadySetGoText != null)
                {
                    ReadySetGoText.text = "Ready...";
                }
            }
            if (fCountDownToStart <= 0)
            {
                bRaceStarted = true;
                if (ReadySetGoObject != null)
                {
                    ReadySetGoObject.SetActive(false);
                }

            }
            fCountDownToStart -= Time.deltaTime;

        }

        if (bRaceStarted)
        {
            fStopWatchTime += Time.deltaTime;
        }

        if (StopWatchText != null)
        {
            string label = "Timer: ";
            StopWatchText.text = label + fStopWatchTime.ToString();
        }

        if (bRaceFinished)
        {
            // DONE!
        }


    }
}

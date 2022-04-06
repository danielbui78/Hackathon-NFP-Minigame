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
    public TapToRunController tapToRunController;

    public bool bRaceFinished = false;

    public GameObject PressStartObject;

    public StarterAssets.StarterAssetsInputs _input;

    // Start is called before the first frame update
    void Start()
    {
        fCountDownToStart = 3.0f;

        if (tapToRunController != null)
        {
            tapToRunController.enabled = false;
        }

        if (StopWatchText != null)
        {
            StopWatchText.enabled = false;
        }

        if (PressStartObject != null)
            PressStartObject.SetActive(true);

        if (ReadySetGoObject != null)
            ReadySetGoObject.SetActive(false);

//        if (StopWatchTimerObject != null)
//            StopWatchText = StopWatchTimerObject.GetComponent<TMPro.TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PressStartObject.active)
        {
            if (_input.startButton)
                PressStartObject.SetActive(false);

            return;
        }

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

        if (bRaceStarted && !bRaceFinished)
        {

            if (StopWatchText != null)
            {
                StopWatchText.enabled = true;
            }

            fStopWatchTime += Time.deltaTime;
            if (tapToRunController != null)
            {
                tapToRunController.enabled = true;
            }

        }

        if (StopWatchText != null)
        {
            string label = "Time: ";
            StopWatchText.text = label + fStopWatchTime.ToString();
        }

        if (bRaceFinished)
        {
            // DONE!
            if (tapToRunController != null)
            {
                tapToRunController.fDragCoefficient = 0.97f;
            }
        }


    }
}

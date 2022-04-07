using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingGame : MonoBehaviour
{
    public float fStopWatchTime = 0.0f;
    public float fCountDownToStart = 3.0f;
    public bool bRaceStarted = false;
    public bool bRaceFinished = false;
    public bool bStartGame = false;

    public TMPro.TextMeshProUGUI StopWatchText;
    private TMPro.TextMeshProUGUI ReadySetGoText;
    public GameObject ReadySetGoObject;
    //    public GameObject StopWatchTimerObject;

    public TapToRunController tapToRunController;
    public StarterAssets.StarterAssetsInputs _input;

    public GameObject startButton;
    public GameObject EndGamePanel;

    public AudioSource RaceMusic;


    public GameObject finishLine;
    public List<GameObject> avatarList;


    public void RestartGame()
    {
        // Reset Timer
        bRaceStarted = false;
        bRaceFinished = false;
        bStartGame = false;

        fStopWatchTime = 0.0f;

        // reset finishLine
        if (finishLine != null)
        {
            finishLine.GetComponent<MeshRenderer>().enabled = true;
        }

        // Reset runner position and state (idle animation)
        int runnerIndex;
        float fLaneWidth = 2.0f;
        for (runnerIndex = 0; runnerIndex < avatarList.Count; runnerIndex++)
        {
            var avatar = avatarList[runnerIndex];
            if (avatar != null)
            {
                avatar.transform.localPosition = Vector3.zero;
                avatar.transform.Translate(runnerIndex * fLaneWidth, 0, 0, Space.Self);
            }
            
        }

        // Reset tapToRunController
        if (tapToRunController != null)
        {
            // reset tapToRunController
            tapToRunController.fCurrentSpeed = 0.0f;
            tapToRunController.fTapRefractoryTimer = 0.0f;
            tapToRunController.fDragCoefficient = 0.995f;
        }

        _input.cursorLocked = true;
        _input.SetCursorState(_input.cursorLocked);

        Start();

    }

    public void StartGame()
    {
        bStartGame = true;

        _input.cursorLocked = true;
        _input.SetCursorState(_input.cursorLocked);

        startButton.SetActive(false);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        RaceMusic.Stop();
        startButton.SetActive(true);

        _input.cursorLocked = false;
        _input.SetCursorState(_input.cursorLocked);

        fCountDownToStart = 3.0f;

        if (EndGamePanel != null)
            EndGamePanel.SetActive(false);

        if (tapToRunController != null)
        {
//            tapToRunController.enabled = false;
            tapToRunController.bDisableMove = true;
        }

        if (StopWatchText != null)
        {
            StopWatchText.enabled = false;
        }

        if (ReadySetGoObject != null)
            ReadySetGoObject.SetActive(false);

//        if (StopWatchTimerObject != null)
//            StopWatchText = StopWatchTimerObject.GetComponent<TMPro.TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if (bStartGame == false)
        {
            return;
        }

        if (bRaceStarted == false)
        {
            if (fCountDownToStart <= 1.0f)
            {
                if (ReadySetGoText != null)
                {
                    ReadySetGoText.text = "Go!";
                    bRaceStarted = true;
                    RaceMusic.Play();
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
        }
        if (fCountDownToStart <= 0)
        {
            if (ReadySetGoObject != null)
            {
                ReadySetGoObject.SetActive(false);
            }
        }
        else
        {
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
//                tapToRunController.enabled = true;
                tapToRunController.bDisableMove = false;
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
            RaceMusic.Stop();
            if (tapToRunController != null)
            {
                tapToRunController.fDragCoefficient = 0.97f;

                _input.cursorLocked = false;
                _input.SetCursorState(_input.cursorLocked);

                if (EndGamePanel != null)
                {
                    if (tapToRunController.fCurrentSpeed <= 0.01f)
                    {
                        EndGamePanel.SetActive(true);
                    }

                }

            }

        }


    }
}

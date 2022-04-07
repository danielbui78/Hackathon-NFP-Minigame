using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HighScoreTable
{
    public float[] score = new float[10];
    public string[] name = new string[10];
    public string[] date = new string[10];
}

public class RacingGame : MonoBehaviour
{
    public float fStopWatchTime = 0.0f;
    public float fCountDownToStart = 3.0f;
    public bool bRaceStarted = false;
    public bool bRaceFinished = false;
    public bool bRaceFinished_RunOnce = false;
    public bool bStartGame = false;

    public TMPro.TextMeshProUGUI StopWatchText;
    private TMPro.TextMeshProUGUI ReadySetGoText;
    public TMPro.TextMeshProUGUI HighScoresText;

    public GameObject ReadySetGoObject;
    //    public GameObject StopWatchTimerObject;

    public TapToRunController tapToRunController;
    public StarterAssets.StarterAssetsInputs _input;

    public GameObject startButton;
    public GameObject EndGamePanel;

    public AudioSource RaceMusic;

    public GameObject finishLine;
    public List<GameObject> avatarList;

     public class runnerTime
    {
        public float fTime;
        public string sName;
        public string sDate;

        public void SetValue(runnerTime other)
        {
            this.fTime = other.fTime;
            this.sName = other.sName;
            this.sDate = other.sDate;
        }
    };
    public List<runnerTime> runTimeSortedList = new List<runnerTime>();

    private bool bLeaderBoardInitialized = false;

    [SerializeField]
    public HighScoreTable highScores;

    public string sUsername = "";

    public GameObject InputNameInputFieldObject;
    public TMPro.TMP_InputField LeaderBoardNameInputField;

    class InsertionCommandContext
    {
        public int index;
        public runnerTime newEntry;
    }
    InsertionCommandContext insertionCommandContext = new InsertionCommandContext();

    public void RequestUserNameAndInsertBestTime(int timeIndex, runnerTime newEntry)
    {
        // save insertion command context
        insertionCommandContext.index = timeIndex;
        insertionCommandContext.newEntry = newEntry;

        // show input field UI
        InputNameInputFieldObject.SetActive(true);

    }

    public void LeaderBoardSubmitName()
    {
        // rebuild insertion command context
        int timeIndex = insertionCommandContext.index;
        runnerTime newEntry = insertionCommandContext.newEntry;

        // perform insertion command
        newEntry.sName = LeaderBoardNameInputField.text;
        runTimeSortedList.Insert(timeIndex, newEntry);

        // hide input field UI
        InputNameInputFieldObject.SetActive(false);

    }

    public void SendBestTimeToDiscord()
    {
        // query for username

        // record best times
        float fBestTime = runTimeSortedList[0].fTime;

        string sMessageToServer = "Best Time for " + sUsername + ": " + fBestTime.ToString();
        GetComponent<DiscordWebhook>().SendMessageToServer(sMessageToServer);
    }

    public void BackSort(int startIndex)
    {

        if (startIndex < 0 || startIndex > runTimeSortedList.Count)
            return;

        for (int timeIndex = startIndex; timeIndex > 0; timeIndex--)
        {
            runnerTime PreviousEntry = new runnerTime();
            PreviousEntry.SetValue(runTimeSortedList[timeIndex - 1]);

            runnerTime CurrentEntry = new runnerTime();
            CurrentEntry.SetValue(runTimeSortedList[timeIndex]);

            if (CurrentEntry.fTime < PreviousEntry.fTime)
            {
                // switch
                runTimeSortedList[timeIndex - 1].SetValue(CurrentEntry);
                runTimeSortedList[timeIndex].SetValue(PreviousEntry);
            }
        }


    }

    public void ForwardSort(int startIndex)
    {
        if (startIndex < 0 || startIndex > runTimeSortedList.Count)
            return;

        for (int timeIndex = startIndex + 1; timeIndex < runTimeSortedList.Count; timeIndex++)
        {
            runnerTime PreviousEntry = new runnerTime();
            PreviousEntry.SetValue(runTimeSortedList[timeIndex - 1]);

            runnerTime CurrentEntry = new runnerTime();
            CurrentEntry.SetValue(runTimeSortedList[timeIndex]);

            if (CurrentEntry.fTime < PreviousEntry.fTime)
            {
                // switch
                runTimeSortedList[timeIndex - 1].SetValue(CurrentEntry);
                runTimeSortedList[timeIndex].SetValue(PreviousEntry);
                BackSort(timeIndex-1);
            }
        }

    }

    public void InsertLastTime()
    {
        runnerTime newEntry = new runnerTime();
        newEntry.fTime = fStopWatchTime;
//      newEntry.sName = sUsername;
        newEntry.sDate = System.DateTime.Now.ToString();

//        runTimeSortedList.Add(newEntry);
        for (int timeIndex = 0; timeIndex < runTimeSortedList.Count;timeIndex++)
        {
            if (fStopWatchTime < runTimeSortedList[timeIndex].fTime)
            {
                // Notify User of High Score and ask for Initials or UserID
                RequestUserNameAndInsertBestTime(timeIndex, newEntry);
//              runTimeSortedList.Insert(timeIndex, newEntry);
                break;
            }
        }

        ForwardSort(0);
        SaveLeaderBoard();
    }

    public void SaveLeaderBoard()
    {
        // save sorted list back to serializable arrays
        for (int timeIndex = 0; timeIndex < runTimeSortedList.Count; timeIndex++)
        {
            if (timeIndex >= highScores.score.Length)
                return;

            highScores.score[timeIndex] = runTimeSortedList[timeIndex].fTime;
            highScores.name[timeIndex] = runTimeSortedList[timeIndex].sName;
            highScores.date[timeIndex] = runTimeSortedList[timeIndex].sDate;
        }

    }

    public void InitializeLeaderBoard()
    {
        // Reeset sorted list
        // Load Serialized Highscore data into sortable list
        // Then sort

        runTimeSortedList.Clear();

        // Copy serialized array to list
        for (int timeIndex = 0; timeIndex < highScores.score.Length; timeIndex++)
        {
            runnerTime newTime = new runnerTime();
            newTime.fTime = highScores.score[timeIndex];
            newTime.sName = highScores.name[timeIndex];
            newTime.sDate = highScores.date[timeIndex];
            runTimeSortedList.Add(newTime);
        }
        // 2. Sort List
        ForwardSort(0);

    }

    public void RestartGame()
    {
        // Reset Timer
        bRaceStarted = false;
        bRaceFinished = false;
        bRaceFinished_RunOnce = false;
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
        InputNameInputFieldObject.SetActive(false);

        if (bLeaderBoardInitialized == false)
        {
            bLeaderBoardInitialized = true;
            InitializeLeaderBoard();
        }
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

        // update stopwatch timer
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

        // update stopwatch UI
        if (StopWatchText != null)
        {
            string label = "Time: ";
            StopWatchText.text = label + fStopWatchTime.ToString();
        }

        // NOTE: Must be run multiple times until tapToRunController.fCurrentSpeed <= 0.01f
        if (bRaceFinished)
        {

            // run only once when RaceFinished
            if (bRaceFinished_RunOnce == false)
            {
                bRaceFinished_RunOnce = true;
                // Caclulate Best Times
                InsertLastTime();
                RaceMusic.Stop();

                if (tapToRunController != null)
                {
                    tapToRunController.fDragCoefficient = 0.97f;

                    _input.cursorLocked = false;
                    _input.SetCursorState(_input.cursorLocked);
                }
            }

            // DONE!
            if (tapToRunController != null && EndGamePanel != null)
            {
                if (tapToRunController.fCurrentSpeed <= 0.01f)
                {
                    // Write High Score Panel
                    HighScoresText.text = "Best Times:\n\n";
                    for (int timeIndex=0; (timeIndex < 5 && timeIndex < runTimeSortedList.Count); timeIndex++)
                    {
                        int scoreIndex = timeIndex + 1;
                        var timeEntry = runTimeSortedList[timeIndex];
                        if (timeEntry.fTime >= 99.9f)
                            break;
                        string BestTimeLine = scoreIndex.ToString() + ". " + timeEntry.fTime.ToString("0.000") + "s\t\t" + timeEntry.sName + "\t\t[" + timeEntry.sDate + "]";
                        HighScoresText.text += BestTimeLine + "\n\n";
                    }
                    EndGamePanel.SetActive(true);
                }
            }

        }


    }
}

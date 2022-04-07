using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCounter : MonoBehaviour
{
    [SerializeField] private int reqScore;
    [SerializeField] private int availableAttempts;
    [SerializeField] private GameRunner gameRunner;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private AudioManager audioManager;
    private int curScore;
    private int turnCount;

    private void Start()
    {
        ResetCounter();
    }

    public void ResetCounter()
    {
        curScore = 0;
        turnCount = 0;
    }

    public void UpdateScore()
    {
        audioManager.PlayGoalSound();
        curScore++;
        if (curScore > reqScore) curScore = reqScore;
        if (curScore == reqScore)
        {
            turnCount++;
            gameRunner.ResetBallPosition();
            
            // Win
            audioManager.PlayWinnerSound();
            gameRunner.StopGame();
            uiManager.LoadEndUi(true);
        }
        else
        {
            UpdateTurns(true);
        }
    }

    public void UpdateTurns(bool noSound = false)
    {
        if (!noSound) audioManager.PlayBlockSound();
        turnCount++;
        var remAttempts = GetAttemptsRemaining();
        if (remAttempts <= 0){
            // Loss
            audioManager.PlayLooserSound();
            gameRunner.StopGame();
            uiManager.LoadEndUi(false);
        }
        gameRunner.ResetBallPosition();
    }

    public int GetScore()
    {
        return curScore;
    }

    public int GetTurnCount()
    {
        return turnCount;
    }

    public int GetAttemptsRemaining()
    {
        return availableAttempts - turnCount;
    }

    public int GetReqScore()
    {
        return reqScore;
    }
}

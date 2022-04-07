using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCounter : MonoBehaviour
{
    [SerializeField] private int reqScore;
    [SerializeField] private int availableAttempts;
    [SerializeField] private GameRunner gameRunner;
    [SerializeField] private UiManager uiManager;
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
        curScore++;
        if (curScore > reqScore) curScore = reqScore;
        if (curScore == reqScore)
        {
            // Win
            gameRunner.StopGame();
            uiManager.LoadEndUi(true);
        }
        UpdateTurns();
    }

    public void UpdateTurns()
    {
        turnCount++;
        var remAttempts = GetAttemptsRemaining();
        if (remAttempts <= 0){
            // Loss
            gameRunner.StopGame();
            uiManager.LoadEndUi(false);
        }
        else
        {
            gameRunner.ResetBallPosition();
        }
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

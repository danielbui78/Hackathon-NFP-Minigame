using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCounter : MonoBehaviour
{
    [SerializeField] private int maxScore;
    [SerializeField] private GameRunner gameRunner;
    [SerializeField] private int curScore;
    [SerializeField] private int turnCount;

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
        if (curScore > maxScore) curScore = maxScore;
        if (curScore == maxScore)
        {
            // Winner
        }
        UpdateTurns();
    }

    public void UpdateTurns()
    {
        turnCount++;
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
}

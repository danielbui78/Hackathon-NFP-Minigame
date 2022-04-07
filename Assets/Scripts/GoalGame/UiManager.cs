using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject mainUi;
    [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject endUi;
    [SerializeField] private TMPro.TMP_Text attemptsText;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private TMPro.TMP_Text targetText;
    [SerializeField] private TMPro.TMP_Text resultsText;
    [SerializeField] private TMPro.TMP_Text statsText;
    [SerializeField] private GoalCounter goalCounter;

    private void Start()
    {
        LoadMainUi();
    }

    private void Update()
    {
        if (gameUi.activeInHierarchy)
        {
            attemptsText.text = goalCounter.GetTurnCount().ToString();
            scoreText.text = goalCounter.GetScore().ToString();
        }
    }

    public void LoadMainUi()
    {
        mainUi.SetActive(true);
        gameUi.SetActive(false);
        endUi.SetActive(false);
    }

    public void LoadGameUi()
    {
        mainUi.SetActive(false);
        gameUi.SetActive(true);
        endUi.SetActive(false);

        targetText.text = goalCounter.GetMaxScore().ToString();
    }

    public void LoadEndUi(bool isWin)
    {
        mainUi.SetActive(false);
        gameUi.SetActive(false);
        endUi.SetActive(true);
        
        resultsText.text = isWin ? "Congratulations!" : "Better luck next time.";
        var winStr = "You have scored all " + goalCounter.GetScore().ToString() + " goals with " + goalCounter.GetAttemptsRemaining().ToString() + " goal attempts remaining.";
        var lossStr = "You have only scored " + goalCounter.GetScore().ToString() + " goals and exhausted all the provided attempts. Required score is " + goalCounter.GetMaxScore().ToString();
        statsText.text = isWin ? winStr : lossStr;
    }
}

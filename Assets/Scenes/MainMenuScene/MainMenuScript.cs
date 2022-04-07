using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RacingMinigame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GoalMinigame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void PlatformerMinigame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

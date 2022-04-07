using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIOptions : MonoBehaviour
{
	public GameObject StartScreen;
    // Start is called before the first frame update
    void Start()
    {
		Time.timeScale = 0;
	}

	public void Pause()
	{
		Time.timeScale = 0;
	}

	public void Resume()
	{
		Time.timeScale = 1;
		StartScreen.SetActive(false);
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ReturnToMain()
	{
		SceneManager.LoadScene(0);
	}
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Scenes")]
    public int sceneIndex = 0;
    public List<Object> sceneList = new List<Object>();

    [Header("UI")]
    public UIController uIContoller;
    
    private bool paused = false;
    public void Quit()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        if (sceneIndex == 0)
        {
            uIContoller.ShowElement("Main Menu");
        }

        SceneManager.UnloadSceneAsync(sceneList[sceneIndex].name);
        sceneIndex = (sceneIndex + 1) % sceneList.Count;
        SceneManager.LoadScene(sceneList[sceneIndex].name);

        if (sceneIndex == 1)
        {
            uIContoller.HideElement("Main Menu");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && sceneIndex != 0)
        {
            TogglePause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0.0f;
        paused = true;
        uIContoller.ShowElement("Pause Menu");
    }

    private void Unpause()
    {
        Time.timeScale = 1.0f;
        paused = false;
        uIContoller.HideElement("Pause Menu");
    }

    public void TogglePause()
    {
        if (paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void Restart()
    {
        sceneIndex = 0;
        SceneManager.LoadScene(sceneList[sceneIndex].name);
        uIContoller.HideAllElemenets();
        uIContoller.ShowElement("Main Menu");
        Unpause();
    }
}

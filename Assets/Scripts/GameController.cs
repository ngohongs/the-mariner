using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [Header("Scenes")]
    public int sceneIndex = 0;
    public List<Object> sceneList = new List<Object>();

    [Header("UI")]
    public UIController uIContoller;
    public bool paused = false;
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            var buildSceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            if (buildSceneName == sceneName)
            {
                sceneIndex = i;
                break;
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && sceneIndex != 0)
        {
            TogglePause();
        }
    }

    public void Pause(bool menuShow = true)
    {
        Time.timeScale = 0.0f;
        paused = true;
        if (menuShow)
        {
            uIContoller.ShowElement("Pause Menu");
        }
    }

    public void Unpause(bool menuShown = true)
    {
        Time.timeScale = 1.0f;
        paused = false;
        if (menuShown)
        {
            uIContoller.HideElement("Pause Menu");
        }
    }

    public void TogglePause(bool showMenu = true)
    {
        if (paused)
        {
            Unpause(showMenu);
        }
        else
        {
            Pause(showMenu);
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

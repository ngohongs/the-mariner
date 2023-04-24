using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [Header("Scenes")]
    public int sceneIndex = 0;

    [Header("UI")]
    public UIController uIContoller;
    public bool paused = false;
    public void Quit()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        sceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(sceneIndex);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        ShowUI();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ShowUI();
    }

    private void ShowUI()
    {
        uIContoller.HideAllElemenets();
        if (sceneIndex == 0)
        {
            uIContoller.ShowElement("Main Menu");
        }
        else
        {
            uIContoller.HideElement("Main Menu");
        }
        if (sceneIndex == 1)
        {
            string tutorial = Resources.Load<TextAsset>("Texts/Tutorial").text;
            uIContoller.DisplayDialogueText(tutorial);
        }
        if (sceneIndex != 0)
        {
            uIContoller.ShowElement("Food");
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
        SceneManager.LoadScene(sceneIndex);
        uIContoller.HideAllElemenets();
        uIContoller.ShowElement("Main Menu");
        Unpause();
    }
}

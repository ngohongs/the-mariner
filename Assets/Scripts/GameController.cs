using DG.Tweening;
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
        DOTween.KillAll();
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
        var name = SceneManager.GetActiveScene().name;

        uIContoller.HideAllElemenets();
        uIContoller.transform.GetChild(3).gameObject.SetActive(false);

        if (sceneIndex == 0)
        {
            uIContoller.ShowElement("Main Menu");
        }
        else
        {
            uIContoller.HideElement("Main Menu");
        }

        bool isLevel1 = name.Equals("Level1");
        bool isLevel = name.ToLower().Contains("level");
        bool isTalk = name.ToLower().Contains("talk");

        if (isLevel && !isLevel1)
        {
            uIContoller.ShowElement("Food");
            uIContoller.ShowElement("Diary Button");
        }
        else if (isTalk)
        {
            uIContoller.transform.GetChild(3).gameObject.SetActive(true);
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
        DOTween.KillAll();
        sceneIndex = 0;
        SceneManager.LoadScene(sceneIndex);
        uIContoller.HideAllElemenets();
        uIContoller.ShowElement("Main Menu");
        Unpause();
    }

    public void RestartLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(sceneIndex);
        Unpause();
    }
}

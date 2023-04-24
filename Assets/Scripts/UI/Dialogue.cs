using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textCompoment;
    public List<string> lines = new List<string>();
    public float textSpeed = 1.0f;
    public int index = 0;
    private GameController gameController;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Awake()
    {  
        gameController = GameController.instance.gameObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 0.0f;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            && !pauseMenu.activeSelf)
        {
            if (textCompoment.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textCompoment.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        textCompoment.text = "";
        StartCoroutine(TypeLines());
    }

    IEnumerator TypeLines()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            while (gameController.paused)
            {
                yield return null;
            }
            textCompoment.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            textCompoment.text = "";
            StartCoroutine(TypeLines());
        }
        else
        {
            gameObject.SetActive(false);
            GameController.instance.uIContoller.HideElement("Screen");
            Time.timeScale = 1.0f;
            
        }
    }

    public void DisplayText(string text)
    {
        GameController.instance.uIContoller.ShowElement("Screen");
        lines = new List<string>(text.Split(new []{ Environment.NewLine, "\n"}, StringSplitOptions.None));
        foreach (string line in lines)
        {
            Debug.Log(line);
        }   
        gameObject.SetActive(true);
        StartDialogue();
    }
}

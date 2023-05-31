using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textCompoment;
    public List<string> lines = new List<string>();
    public float textSpeed = 1.0f;
    public int index = 0;
    public GameObject pauseMenu;
    private bool inDialog = false;


    // ondra
    public AudioSource[] audioSources;
    //




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
        inDialog = true;

        int randomIndex = UnityEngine.Random.Range(0, audioSources.Length);
        audioSources[randomIndex].Play();

        index = 0;
        textCompoment.text = "";
        StartCoroutine(TypeLines());
    }

    IEnumerator TypeLines()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            while (GameController.instance.paused)
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

            int randomIndex = UnityEngine.Random.Range(0, audioSources.Length);
            audioSources[randomIndex].Play();

            index++;
            textCompoment.text = "";
            StartCoroutine(TypeLines());
        }
        else
        {
            gameObject.SetActive(false);
            GameController.instance.uIContoller.HideElement("Screen");
            Time.timeScale = 1.0f;
            inDialog = false;

        }
    }

    public void DisplayText(string text)
    {
        GameController.instance.uIContoller.ShowElement("Screen");
        lines = new List<string>(text.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None));
        gameObject.SetActive(true);
        StartDialogue();
    }

    public bool IsInDialog()
    {
        return inDialog;
    }
}

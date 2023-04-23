using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textCompoment;
    public List<string> lines = new List<string>();
    public float textSpeed = 1.0f;
    private int index = 0;
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = Singleton.instance.gameObject.GetComponent<GameController>();
        textCompoment.text = "";
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 0.0f;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
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
        StartCoroutine(TypeLines());
    }

    IEnumerator TypeLines()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
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
            Time.timeScale = 1.0f;
        }
    }

    public void DisplayText(string text)
    {
        lines = new List<string>(text.Split(Environment.NewLine, StringSplitOptions.None));
        gameObject.SetActive(true);
        StartDialogue();
    }
}

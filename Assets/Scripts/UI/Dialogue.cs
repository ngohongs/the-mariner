using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textCompoment;
    public List<string> lines = new List<string>();
    public float textSpeed = 1.0f;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        textCompoment.text = "";
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
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
            yield return new WaitForSeconds(textSpeed);
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
        }
    }
}

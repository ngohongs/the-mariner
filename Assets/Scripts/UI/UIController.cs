using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using static System.TimeZoneInfo;

public class UIController : MonoBehaviour
{
    public Talk talkManager;
    public void ShowElement(string name)
    {
        GameObject element = GetUIElement(name);
        if (element != null)
        {
            element.SetActive(true);
        }
    }

    public void HideElement(string name)
    {
        GameObject element = GetUIElement(name);
        if (element != null)
        {
            element.SetActive(false);
        }
    }

    public void HideAllElemenets()
    {
        Transform canvas = transform.GetChild(0);
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public GameObject GetUIElement(string name)
    {
        GameObject element = null;
        Transform canvas = transform.GetChild(0);
        for (int i = 0; i < canvas.childCount; i++)
        {
            var child = canvas.transform.GetChild(i);
            if (child.name == name)
            {
                element = child.gameObject;
                break;
            }
        }
        return element;
    }

    public void DisplayDialogueText(string text)
    {
        var element = GetUIElement("Dialogue Box");
        if (element != null)
        {
            element.GetComponent<Dialogue>().DisplayText(text);
        }
    }

    public void DisplayCharacterDialogueText(string text, bool pause = true, Action action = null) {
        talkManager.gameObject.SetActive(true);
        talkManager.ShowBackground = false;

        List<string> formated = new List<string>();

        string[] blocks = text.Split(new string[] { "--\n", "--\r\n", "--\r", "--\n\r" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var block in blocks)
        {
            Debug.Log(block);
            formated.Add(block.Trim());
        }

        var formattedBlocks = formated.ToArray();
        StartCoroutine(StartTalkDialog(formattedBlocks, pause, action));

    }

    IEnumerator StartTalkDialog(string[] script, bool pause = true, Action action = null) {
        foreach (string block in script)
        {
            if (block[0] == 'P')
            {
                talkManager.enemy.SetActive(false);
                talkManager.player.SetActive(true);
                Debug.Log("Player " + block.Substring(2));
                talkManager.PlayerSpeach(block.Substring(2).Trim(), pause);
            }
            else if (block[0] == 'E')
            {
                talkManager.enemy.SetActive(true);
                talkManager.player.SetActive(false);
                Debug.Log("Enemy " + block.Substring(2));
                talkManager.EnemySpeach(block.Substring(2).Trim(), pause);
            }
            yield return new WaitUntil(() => !talkManager.talking);
        }

        talkManager.ShowBackground = true;
        talkManager.enemy.SetActive(true);
        talkManager.player.SetActive(true);
        talkManager.gameObject.SetActive(false);
        
        action?.Invoke();
    }
}

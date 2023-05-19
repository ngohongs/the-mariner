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
}

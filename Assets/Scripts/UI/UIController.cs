using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Serializable]
    public struct UIElement
    {
        public string name;
        public GameObject element;
    }

    public List<UIElement> uIElements;

    public void ShowElement(string name)
    {
        var element = uIElements.FirstOrDefault(e => e.name == name);
        if (element.element != null)
        {
            element.element.SetActive(true);
        }
    }

    public void HideElement(string name)
    {
        var element = uIElements.FirstOrDefault(e => e.name == name);
        if (element.element != null)
        {
            element.element.SetActive(false);
        }
    }   

    public void HideAllElemenets()
    {
        foreach (var element in uIElements)
        {
            if (element.element != null)
            {
                element.element.SetActive(false);
            }
        }
    }

    public void DisplayDialogueText(string text)
    {
        var element = uIElements.FirstOrDefault(e => e.name == "Dialogue Box");
        Debug.Log("search " + element.name);
        if (element.element != null)
        {
            element.element.GetComponent<Dialogue>().DisplayText(text);
        }
    }
}

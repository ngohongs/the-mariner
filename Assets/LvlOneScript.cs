using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlOneScript : MonoBehaviour
{
    public TextAsset text;
    void Start()
    {
        if (text == null)
            return;

        GameController.instance.uIContoller.DisplayCharacterDialogueText(text.text); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

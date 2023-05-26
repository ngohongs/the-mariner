using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTalk : MonoBehaviour
{
    public TextAsset text;

    public void StartTalk()
    {
        GameController.instance.uIContoller.DisplayCharacterDialogueText(text.text);
    }
}

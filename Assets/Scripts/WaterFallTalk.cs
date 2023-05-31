using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterFallTalk : MonoBehaviour
{
    public TextAsset text;

    private CanvasGroup canvasGroup;
    private TextMeshProUGUI textfield;

    void Start()
    {

        canvasGroup = GameController.instance.uIContoller.transform.GetChild(2).GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        textfield = canvasGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        if (text == null)
            return;

        GameController.instance.uIContoller.DisplayCharacterDialogueText(text.text, false, () =>
        {
            StartCoroutine(WaitEnd());
        });
    }

    IEnumerator WaitEnd()
    {
        textfield.text = "";
        canvasGroup.DOFade(1f, 2f);

        yield return new WaitForSeconds(3);

        GameController.instance.NextScene();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;



public class GoToLevelTwo : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;

    [SerializeField] CanvasGroup canvasGroup;
    
    [SerializeField] float delayBeforeFade = 0.1f;


    private void Start()
    {
        levelSelect = GameObject.Find("LeveSelectScreen");
        canvasGroup = levelSelect.transform.GetChild(0).gameObject.GetComponent<CanvasGroup>();
        levelSelect.SetActive(false);
    }


    public void GoToLevelTwoScene()
    {
        StartCoroutine(FadeOutCanvasWithDelay());
        SceneManager.LoadScene("Talk1"); // Replace "Level2" with the name of your level 2 scene
    }
    public void GoToLevelThreeScene()
    {
        StartCoroutine(FadeOutCanvasWithDelay());
        SceneManager.LoadScene("Talk2");
    }
    public void GoToMainMenu()
    { 
        levelSelect.SetActive(false);
    }
    public void GoToLevelSelect()
    {
       levelSelect.SetActive(true);
    }



    private IEnumerator FadeOutCanvasWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        canvasGroup.DOFade(1f, 1f);
        canvasGroup.blocksRaycasts = false;

        yield return new WaitForSeconds(1f);

        if (levelSelect.activeSelf)
            levelSelect.SetActive(false);
        else
            levelSelect.SetActive(true);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StartTalk : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset script;

    Talk talkManager;
    void Start()
    {
        if (GameController.instance == null)
        {
            var gameControllerPrefab = (GameObject)Resources.Load("Game Manager");
            var instance = Instantiate(gameControllerPrefab);
            instance.name = "Game Manager";
        }

        talkManager = GameController.instance.uIContoller.transform.GetChild(3).GetComponent<Talk>();

        StartCoroutine(StartTalkDialog(ParseScript(script)));
    }

    public string[] ParseScript(TextAsset script)
    {
        List<string> formated = new List<string>();

        string[] blocks = script.text.Split(new string[] { "--\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var block in blocks)
        {
            formated.Add(block.Trim());
        }

        return formated.ToArray();
    }

    IEnumerator StartTalkDialog(string[] script)
    {
        foreach (string block in script)
        {
            if (block[0] == 'P')
            {
                Debug.Log("Player " + block.Substring(2));
                talkManager.PlayerSpeach(block.Substring(2));
            }
            else if (block[0] == 'E')
            {
                Debug.Log("Enemy " + block.Substring(2));
                talkManager.EnemySpeach(block.Substring(2));
            }
            yield return new WaitUntil(() => !talkManager.talking);
        }

        GameController.instance.NextScene();
    }

}

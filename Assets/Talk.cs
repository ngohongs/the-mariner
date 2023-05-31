using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



using UnityEngine.Audio;

public class Talk : MonoBehaviour
{
    

    public GameObject player;
    public GameObject enemy;

    public Dialogue dialogue;
    public RawImage background;
    private bool _showBackground = true;
    public bool ShowBackground
    {
        get { return _showBackground; }
        set { background.enabled = value; } 
    }

    public bool talking { get; private set; } = false;

    private void Update()
    {
       // Stopped talking
       if (talking != dialogue.IsInDialog() && talking)
       {
            talking = false;
            player.transform.DOKill();
            enemy.transform.DOKill();
            Debug.Log("TALK END");
        }
    }

    public void EnemySpeach(string text, bool pause = true)
    {
        Debug.Log("ENEMY TALK START");
        talking = true;
        dialogue.DisplayText(text, pause);
        enemy.transform.DOShakePosition(2, new Vector3(0.0f, 10.0f, 0.0f), 10, 90, true, false).SetUpdate(true).SetLoops(-1);


       
    }


    public void PlayerSpeach(string text, bool pause = true)
    {
        Debug.Log("PLAYER TALK START");
        talking = true;
        dialogue.DisplayText(text, pause);
        player.transform.DOShakePosition(2, new Vector3(0.0f, 10.0f, 0.0f), 10, 90, true, false).SetUpdate(true).SetLoops(-1);



      
    }
}

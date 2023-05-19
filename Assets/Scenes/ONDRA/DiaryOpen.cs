using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryOpen : MonoBehaviour
{
    [SerializeField] private GameObject diary;
    private bool OpenClose;

    private void Start()
    {
       
    }

    public void OnDiaryClick()
    {
        OpenClose = !OpenClose;
        diary.gameObject.SetActive(OpenClose);
    }
}

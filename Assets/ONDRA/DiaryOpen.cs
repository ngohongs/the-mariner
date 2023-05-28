using UnityEngine;

public class DiaryOpen : MonoBehaviour
{
    [SerializeField] private GameObject diary;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceTwo;


    private bool OpenClose;

    private void Start()
    {

    }

    public void OnDiaryClick()
    {
        OpenClose = !OpenClose;

        if (!OpenClose)
        {
            audioSource.Play();
        }
        if (OpenClose)
        {
            audioSourceTwo.Play();
        }

        diary.gameObject.SetActive(OpenClose);





    }
}

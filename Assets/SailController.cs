using UnityEngine;

public class SailController : MonoBehaviour
{
    public AudioSource[] audioSources;
   
    public void PlaySailSound()
    {
        if (audioSources.Length > 0)
        {
            int randomSourceIndex = Random.Range(0, audioSources.Length);
            AudioSource selectedSource = audioSources[randomSourceIndex];
            selectedSource.Play();
        }
    }
}
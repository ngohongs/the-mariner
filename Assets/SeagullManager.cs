using UnityEngine;

public class SeagullManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    public float minInterval;
    public float maxInterval;

    private float nextSoundTime = 0f;

    private void Start()
    {
        nextSoundTime = GetRandomTime();
    }

    private void Update()
    {
        if (Time.time >= nextSoundTime)
        {
            PlayRandomSound();
            nextSoundTime = Time.time + GetRandomTime();
        }
    }

    private void PlayRandomSound()
    {
        if (audioSources.Length > 0)
        {
            int randomSourceIndex = Random.Range(0, audioSources.Length);
            AudioSource selectedSource = audioSources[randomSourceIndex];
            selectedSource.Play();
        }
    }

    private float GetRandomTime()
    {
        return Random.Range(minInterval, maxInterval);
    }
}
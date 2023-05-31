using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dubControl : MonoBehaviour
{
 
 public GameObject[] soundObjects;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        int randomIndex = Random.Range(0, soundObjects.Length);
        GameObject randomSoundObject = soundObjects[randomIndex];
        AudioClip randomClip = randomSoundObject.GetComponent<AudioSource>().clip;
        audioSource.clip = randomClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
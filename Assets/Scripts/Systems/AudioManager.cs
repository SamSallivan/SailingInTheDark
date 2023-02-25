using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource RadioPlayer;
    public AudioSource DialoguePlayer;

    private void Start()
    {
        instance = this;
    }

    public void playRecording(AudioClip clip)
    {
        StartCoroutine(PlayAudioCo(clip, RadioPlayer));
    }

    public void playDialogue(AudioClip clip)
    {
        StartCoroutine(PlayAudioCo(clip, DialoguePlayer));
    }

    IEnumerator PlayAudioCo(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
        yield return null;
    }
}

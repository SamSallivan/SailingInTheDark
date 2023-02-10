using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource RadioPlayer;

    private void Start()
    {
        instance = this;
    }

    public void playRecording(AudioClip tempClip)
    {
        StartCoroutine(PlayAudioCo(tempClip));
    }

    IEnumerator PlayAudioCo(AudioClip tempClip)
    {
        //yield return new WaitForSeconds(1f);
        RadioPlayer.clip = tempClip;
        RadioPlayer.Play();
        yield return null;
    }

    public void StopRadio()
    {
        RadioPlayer.Stop();
    }
}

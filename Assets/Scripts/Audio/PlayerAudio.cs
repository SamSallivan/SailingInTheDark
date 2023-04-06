using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class PlayerAudio : MonoBehaviour
{
    [Foldout("AudioSources", true)]
    public AudioSource walk;
    public AudioSource jump;

    [Foldout("AudioClips", true)]
    public AudioClip sfx_walk;
    public AudioClip sfx_jump;

    private bool isDelayed = false;
    private bool isJump = false;
    private float startVolume;

    public float footStepInterval;

    private void Start()
    {
        startVolume = walk.volume;
    }
    public IEnumerator PlayWalkSound()
    {
        // Debug.Log(!audioSource.isPlaying);
        // Debug.Log(!isDelayed);
        if (!walk.isPlaying && !isDelayed)
        {
            walk.volume = startVolume;
            walk.clip = sfx_walk;
            walk.loop = false;
            walk.Play();
            isDelayed = true;
            yield return new WaitForSeconds(walk.clip.length);
            yield return new WaitForSeconds(footStepInterval);
            isDelayed = false;
        }

        else if (isDelayed)
        {
            yield break;
        }
    }

    public IEnumerator StopWalkSound()
    {
        while (walk.volume > 0f)
        {
            walk.volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        walk.Stop();
    }

    public void PlayJumpSound()
    {
        jump.clip = sfx_jump;
        jump.Play();
    }
}

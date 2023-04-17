using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ObjectiveAudio : MonoBehaviour
{
    [Foldout("AudioSources", true)]
    public AudioSource objectiveSound;

    [Foldout("AudioClips", true)]
    public AudioClip sfx_get_objective;
    public AudioClip sfx_complete_objective;

    public void PlayGetObjective()
    {
        objectiveSound.clip = sfx_get_objective;
        objectiveSound.Play();
    }

    public void PlayCompleteObjective()
    {
        objectiveSound.clip = sfx_complete_objective;
        objectiveSound.Play();
    }
}

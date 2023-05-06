using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

//place on timeline animated objects to call these functions.
public class CutsceneActions : MonoBehaviour
{
    public CinemachineImpulseSource shakeSource;
    public DialogueData dialogueData;

    public void Shake()
    {
        shakeSource.GenerateImpulse();
    }

    public void PlayAudio()
    {
        DialogueManager.instance.OverrideDialogue(dialogueData);
        UIManager.instance.boatUI.SetActive(false);
    }
    public void TitleScreen()
    {
        SceneManager.LoadScene(0);
    }
}

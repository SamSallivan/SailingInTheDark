using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public List<Recording> RecordingWaitList;
    public bool radioPaused;
    private void Start()
    {
        instance = this;
    }

    public void EnterDialogue(Recording tempRecording)
    {
        StartCoroutine(SubtitleSequence(tempRecording));
    }

    IEnumerator SubtitleSequence(Recording tempRecording)
    {
        for (int i = 0; i < tempRecording.lines.Count; i++)
        {
            UIManager.instance.DisplaySubtitle(tempRecording.lines[i].subtitle);
            AudioManager.instance.playRecording(tempRecording.lines[i].audioClip);
            float timeLength = tempRecording.lines[i].audioClip.length;
            //Debug.Log(timeLength);
            //yield return new WaitForSeconds(timeLength);
            yield return new WaitWhile(() => AudioManager.instance.RadioPlayer.isPlaying || radioPaused);
        }
        UIManager.instance.DisplaySubtitle(" ");
        yield return null;
    }

    public void StopCurrentClip()
    {
        UIManager.instance.DisplaySubtitle(" ");
        AudioManager.instance.RadioPlayer.Stop(); 
    }

    public void AddWaitAudio(Recording tempRecording)
    {
        RecordingWaitList.Add(tempRecording);
    }

    public void PlayWaitlistDialogue()
    {
        if (RecordingWaitList.Count == 0)
            return;
        StartCoroutine(WaitedClipSequence());
    }

    IEnumerator WaitedClipSequence()
    {
        for (int i = 0; i < RecordingWaitList.Count; i++)
        {
            EnterDialogue(RecordingWaitList[i]);
            float timeLength = 0;
            for (int j = 0; j < RecordingWaitList[i].lines.Count; j++)
            {
                timeLength += RecordingWaitList[i].lines[i].audioClip.length;
            }
            yield return new WaitForSeconds(timeLength);
        }
    }

    public void PauseRadio(){
        radioPaused = true;
        AudioManager.instance.RadioPlayer.Pause();
    }

    public void UnpauseRadio(){
        radioPaused = false;
        AudioManager.instance.RadioPlayer.Play();
    }

}

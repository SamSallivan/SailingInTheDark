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
        List<AudioClip> listOfClips = tempRecording.audioClips;
        List<string> listOfSubtitle = tempRecording.subtitles;
        for (int i = 0; i < listOfClips.Count; i++)
        {
            UIManager.instance.DisplaySubtitle(listOfSubtitle[i]);
            AudioManager.instance.playRecording(listOfClips[i]);
            float timeLength = listOfClips[i].length;
            //Debug.Log(timeLength);
            yield return new WaitWhile(() => AudioManager.instance.RadioPlayer.isPlaying || radioPaused);
            //yield return new WaitForSeconds(timeLength);
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
            for (int j = 0; j < RecordingWaitList[i].audioClips.Count; j++)
            {
                timeLength += RecordingWaitList[i].audioClips[i].length;
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

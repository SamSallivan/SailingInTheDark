using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class RecordingManager : MonoBehaviour
{
    public static RecordingManager instance;

    public I_TapeRecorder recorder;

    [ReadOnly]
    public int currentIndex;
    [ReadOnly]
    public int interuptedIndex;
    [ReadOnly]
    public DialogueData currentRecording;
    [ReadOnly]
    public List<DialogueData> recordingWaitList;
    [ReadOnly]
    public Line currentLine;
    [ReadOnly]
    public Coroutine currentCoroutine;

    [ReadOnly]
    public bool radioPaused = true;
    [ReadOnly]
    public bool radioInBound;
    [ReadOnly]
    public bool autoUnpause;

    private void Start()
    {
        instance = this;
    }

    public void ReplaceRecording(DialogueData tempRecording, bool autoUnpause = false)
    {
        //Stop current recording and eject tape
        if (currentRecording != null)
        {
            StopCurrentLine();
        }
        
        currentCoroutine = StartCoroutine(PlayRecording(tempRecording, 0));
        
    }

    IEnumerator PlayRecording(DialogueData tempRecording, int fromLine)
    {
        yield return new WaitWhile(() => radioPaused);
        currentRecording = tempRecording;
        for (int i = fromLine; i < tempRecording.lines.Count; i++)
        {
            currentIndex = i;
            interuptedIndex = i;
            currentLine = tempRecording.lines[i];

            yield return new WaitForSeconds(tempRecording.lines[i].intervalBefore);
            UIManager.instance.FadeInSubtitle(tempRecording.lines[i].speaker, tempRecording.lines[i].subtitle, UIManager.SubtitleType.Radio);
            yield return new WaitForSeconds(0.25f);
            AudioManager.instance.playRecording(tempRecording.lines[i].audioClip);
            yield return new WaitWhile(() => (AudioManager.instance.RadioPlayer.isPlaying || radioPaused));
            UIManager.instance.FadeOutSubtitle(UIManager.SubtitleType.Radio);
            yield return new WaitForSeconds(0.25f);
            if (i != tempRecording.lines.Count - 1)
            {
                UIManager.instance.radioSubtitleUI.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(tempRecording.lines[i].intervalAfter);

        }
        recorder.EjectTape();

        currentIndex = 0;
        currentLine = new Line();
        currentRecording = null;
        UIManager.instance.ClearSubtitle(UIManager.SubtitleType.Radio);
        AudioManager.instance.RadioPlayer.clip = null;

        yield return null;
    }

    public void StopCurrentLine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        interuptedIndex = currentIndex;
        Debug.Log(interuptedIndex);
        currentIndex = 0;
        currentLine = new Line();
        UIManager.instance.ClearSubtitle(UIManager.SubtitleType.Radio);
        AudioManager.instance.RadioPlayer.Stop();
        AudioManager.instance.RadioPlayer.clip = null;
        //currentDialogue = null;
    }

    public void PauseRadio(){
        radioPaused = true;
        UIManager.instance.ClearSubtitle(UIManager.SubtitleType.Radio);
        AudioManager.instance.RadioPlayer.Pause();
    }

    public void UnpauseRadio(){
        if (!radioPaused)
            return;

        radioPaused = false;
        if(currentRecording != null)
        {
            UIManager.instance.FadeInSubtitle(currentRecording.lines[currentIndex].speaker, currentRecording.lines[currentIndex].subtitle, UIManager.SubtitleType.Radio);
        }
        AudioManager.instance.RadioPlayer.UnPause();
    }

    public void ExitRadioBound()
    {
        radioInBound = false;
        if (!radioPaused && currentRecording != null)
        {
            StopCurrentLine();
        }
    }

    public void EnterRadioBound()
    {
        radioInBound = true;

        if (!radioPaused)
        {
            if (recorder.tapeInserted != null)
            {
                DialogueData recording = recorder.tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData.recording;
                currentCoroutine = StartCoroutine(PlayRecording(recording, interuptedIndex));
            }
        }
        else if (autoUnpause)
        {
            UnpauseRadio();
            autoUnpause = false;
        }
    }
}

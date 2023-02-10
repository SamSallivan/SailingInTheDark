using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public int currentIndex;
    public int interuptedIndex;
    public Line currentLine;
    public DialogueData currentRecording;
    public List<DialogueData> recordingWaitList;
    public bool radioPaused = true;
    public bool radioInBound;
    public Coroutine currentCoroutine;
    private void Start()
    {
        instance = this;
    }

    public void OverrideDialogue(DialogueData tempRecording)
    {
        if (currentRecording == null)
        {
        }
        else if (currentRecording != null)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                StopCurrentClip();
            }
            recordingWaitList.Insert(0, currentRecording);
            interuptedIndex = currentIndex;
        }

        if (radioInBound)
        {
            if (!radioPaused)
            {
                currentCoroutine = StartCoroutine(SubtitleSequence(tempRecording, 0));
            }
            else
            {
                currentRecording = null;
                recordingWaitList.Insert(0, tempRecording);
            }
        }
        else
        {
            if (!radioPaused)
            {
                currentRecording = tempRecording;
            }
            else
            {
                currentRecording = null;
                recordingWaitList.Insert(0, tempRecording);
            }
        }
    }
    public void WaitlistDialogue(DialogueData tempRecording)
    {
        recordingWaitList.Add(tempRecording);
        if (currentRecording == null && radioInBound && !radioPaused)
        {
            currentCoroutine = StartCoroutine(SubtitleSequence(tempRecording, 0));
        }
    }

    IEnumerator SubtitleSequence(DialogueData tempRecording, int fromLine)
    {
        yield return new WaitWhile(() => radioPaused);
        currentRecording = tempRecording;
        for (int i = fromLine; i < tempRecording.lines.Count; i++)
        {
            currentIndex = i;
            currentLine = tempRecording.lines[i];
            yield return new WaitForSeconds(tempRecording.lines[i].intervalBefore);
            UIManager.instance.FadeInSubtitle(tempRecording.lines[i].speaker, tempRecording.lines[i].subtitle);
            yield return new WaitForSeconds(0.5f);
            AudioManager.instance.playRecording(tempRecording.lines[i].audioClip);
            float timeLength = tempRecording.lines[i].audioClip.length;
            //Debug.Log(timeLength);
            //yield return new WaitForSeconds(timeLength);
            yield return new WaitWhile(() => (AudioManager.instance.RadioPlayer.isPlaying || radioPaused));
            UIManager.instance.FadeOutSubtitle();
            yield return new WaitForSeconds(tempRecording.lines[i].intervalAfter);

        }
        currentIndex = 0;
        currentLine = new Line();
        currentRecording = null;
        UIManager.instance.ClearSubtitle();
        AudioManager.instance.RadioPlayer.clip = null;
        //recordingWaitList.RemoveAt(0);

        if (recordingWaitList.Count != 0)
        {
            currentCoroutine = StartCoroutine(SubtitleSequence(recordingWaitList[0], interuptedIndex));
            recordingWaitList.RemoveAt(0);
            interuptedIndex = 0;
        }
        else
        {
            radioPaused = true;
        }

        yield return null;
    }

    public void StopCurrentClip()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        interuptedIndex = currentIndex;
        currentIndex = 0;
        currentLine = new Line();
        //currenRecording = null;
        UIManager.instance.ClearSubtitle();
        AudioManager.instance.RadioPlayer.Stop();
        AudioManager.instance.RadioPlayer.clip = null;
        //interuptedIndex = currentIndex;
    }

    public void ClearAllClip()
    {
    }

    public void SkipCurrentClip()
    {
    }

    public void PauseRadio(){
        radioPaused = true;
        UIManager.instance.ClearSubtitle();
        AudioManager.instance.RadioPlayer.Pause();
    }

    public void UnpauseRadio(){
        radioPaused = false;
        if(currentRecording != null)
        {
            UIManager.instance.FadeInSubtitle(currentRecording.lines[currentIndex].speaker, currentRecording.lines[currentIndex].subtitle);
        }
        else if (recordingWaitList.Count != 0)
        {
            currentCoroutine = StartCoroutine(SubtitleSequence(recordingWaitList[0], 0));
        }
        AudioManager.instance.RadioPlayer.UnPause();
    }

    public void ExitRadioBound()
    {
        radioInBound = false;
        if (!radioPaused && currentRecording != null) {
            StopCoroutine(currentCoroutine);
            interuptedIndex = currentIndex;
            StopCurrentClip();
        }
    }

    public void EnterRadioBound()
    {
        radioInBound = true;

        if (!radioPaused)
        {
            if (currentRecording != null)
            {
                currentCoroutine = StartCoroutine(SubtitleSequence(currentRecording, interuptedIndex));
            }
            else if (recordingWaitList.Count != 0)
            {
                currentCoroutine = StartCoroutine(SubtitleSequence(recordingWaitList[0], 0));
            }
        }
    }
}

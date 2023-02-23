using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public I_TapeRecorder recorder;
    public int currentIndex;
    public int interuptedIndex;
    public Line currentLine;
    public ItemData generateTape;
    public DialogueData currentRecording;
    public List<DialogueData> recordingWaitList;
    public bool radioPaused = true;
    public bool radioInBound;
    public bool autoUnpause;
    public Coroutine currentCoroutine;
    private void Start()
    {
        instance = this;
    }
    public void ReplaceRecording(DialogueData tempRecording, bool autoUnpause = false)
    {
        //Push the current one back in waitlist
        if (currentRecording != null)
        {
            StopCurrentRecordingLine();
            recorder.EjectTape();
        }

        if (radioInBound)
        {
            if (!radioPaused)
            {
                currentCoroutine = StartCoroutine(PlayRecording(tempRecording, 0));
            }
            else
            {
                currentRecording = null;
                recordingWaitList.Insert(0, tempRecording);
                if (autoUnpause)
                {
                    UnpauseRadio();
                }
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
                if (autoUnpause)
                {
                    this.autoUnpause = true;
                }
            }
        }
    }

    public void ReplaceAndGenerate(ItemData data, bool autoUnpause = false)
    {
        ReplaceRecording(data.recording, autoUnpause);
        generateTape = data;
    }

    public void OverrideRecording(DialogueData tempRecording, bool autoUnpause = false)
    {
        //Push the current one back in waitlist
        if (currentRecording != null)
        {
            StopCurrentRecordingLine();
            recordingWaitList.Insert(0, currentRecording);
        }

        if (radioInBound)
        {
            if (!radioPaused)
            {
                currentCoroutine = StartCoroutine(PlayRecording(tempRecording, 0));
            }
            else
            {
                currentRecording = null;
                recordingWaitList.Insert(0, tempRecording);
                if (autoUnpause)
                {
                    UnpauseRadio();
                }
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
                if (autoUnpause)
                {
                    this.autoUnpause = true;
                }
            }
        }
    }

    public void WaitlistRecording(DialogueData tempRecording)
    {
        if (currentRecording == null && radioInBound && !radioPaused)
        {
            currentCoroutine = StartCoroutine(PlayRecording(tempRecording, 0));
        }
        else
        {
            recordingWaitList.Add(tempRecording);
        }
    }

    IEnumerator PlayRecording(DialogueData tempRecording, int fromLine)
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
        
        //recorder.EjectTape();

        currentIndex = 0;
        currentLine = new Line();
        currentRecording = null;
        UIManager.instance.ClearSubtitle();
        AudioManager.instance.RadioPlayer.clip = null;
        //recordingWaitList.RemoveAt(0);

        if (recordingWaitList.Count != 0)
        {
            currentCoroutine = StartCoroutine(PlayRecording(recordingWaitList[0], interuptedIndex));
            recordingWaitList.RemoveAt(0);
            interuptedIndex = 0;
        }
        else
        {
            //radioPaused = true;
        }

        yield return null;
    }

    public void PlayNext()
    {
        if (recordingWaitList.Count != 0)
        {
            currentCoroutine = StartCoroutine(PlayRecording(recordingWaitList[0], interuptedIndex));
            recordingWaitList.RemoveAt(0);
            interuptedIndex = 0;
        }
    }

    public void StopCurrentRecordingLine()
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

    public void PauseRadio()
    {
        radioPaused = true;
        UIManager.instance.ClearSubtitle();
        AudioManager.instance.RadioPlayer.Pause();
    }

    public void UnpauseRadio()
    {
        if (!radioPaused)
            return;

        radioPaused = false;
        if (currentRecording != null)
        {
            UIManager.instance.FadeInSubtitle(currentRecording.lines[currentIndex].speaker, currentRecording.lines[currentIndex].subtitle);
        }
        else if (recordingWaitList.Count != 0)
        {
            currentCoroutine = StartCoroutine(PlayRecording(recordingWaitList[0], 0));
            recordingWaitList.RemoveAt(0);
        }
        AudioManager.instance.RadioPlayer.UnPause();
    }

    public void ExitRadioBound()
    {
        radioInBound = false;
        if (!radioPaused && currentRecording != null)
        {
            StopCurrentRecordingLine();
        }
    }

    public void EnterRadioBound()
    {
        radioInBound = true;

        if (!radioPaused)
        {
            if (currentRecording != null)
            {
                currentCoroutine = StartCoroutine(PlayRecording(currentRecording, interuptedIndex));
            }
            else if (recordingWaitList.Count != 0)
            {
                currentCoroutine = StartCoroutine(PlayRecording(recordingWaitList[0], 0));
                recordingWaitList.RemoveAt(0);
            }
        }
        else if (autoUnpause)
        {
            UnpauseRadio();
            autoUnpause = false;
        }
    }
}

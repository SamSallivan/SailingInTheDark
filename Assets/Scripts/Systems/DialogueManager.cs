using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    public int currentIndex;
    public int interuptedIndex;
    public Line currentLine;
    public DialogueData currentDialogue;
    public List<DialogueData> dialogueWaitList;
    public bool dialoguePaused = true;
    public bool dialogueInBound;
    public bool autoUnpause;
    public Coroutine currentCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OverrideDialogue(DialogueData tempDialogue, bool autoUnpause = false)
    {
        //Push the current one back in waitlist
        if (currentDialogue != null¡¡&& currentDialogue != tempDialogue)
        {
            StopCurrentRecordingLine();
            //if this one is also important
            //dialogueWaitList.Insert(0, currentDialogue);
        }

        if (dialogueInBound && currentDialogue != tempDialogue)
        {
            currentCoroutine = StartCoroutine(PlayRecording(tempDialogue, 0));
        }
        else
        {
            currentDialogue = tempDialogue;
        }
    }

    public void WaitlistRecording(DialogueData tempDialogue)
    {
        if (currentDialogue == null && dialogueInBound)
        {
            currentCoroutine = StartCoroutine(PlayRecording(tempDialogue, 0));
        }
        else
        {
            dialogueWaitList.Add(tempDialogue);
        }
    }

    IEnumerator PlayRecording(DialogueData tempRecording, int fromLine)
    {
        yield return new WaitWhile(() => dialoguePaused);
        currentDialogue = tempRecording;
        for (int i = fromLine; i < tempRecording.lines.Count; i++)
        {
            currentIndex = i;
            currentLine = tempRecording.lines[i];

            yield return new WaitForSeconds(tempRecording.lines[i].intervalBefore);
            UIManager.instance.FadeInSubtitle(tempRecording.lines[i].speaker, tempRecording.lines[i].subtitle, UIManager.SubtitleType.Dialogue);
            yield return new WaitForSeconds(0.25f);
            AudioManager.instance.playDialogue(tempRecording.lines[i].audioClip);
            yield return new WaitWhile(() => (AudioManager.instance.DialoguePlayer.isPlaying));
            UIManager.instance.FadeOutSubtitle(UIManager.SubtitleType.Dialogue);
            yield return new WaitForSeconds(0.25f);
            if (i != tempRecording.lines.Count - 1)
            {
                UIManager.instance.dialogueSubtitleUI.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(tempRecording.lines[i].intervalAfter);

        }

        currentIndex = 0;
        currentLine = new Line();
        currentDialogue = null;
        UIManager.instance.ClearSubtitle(UIManager.SubtitleType.Dialogue);
        AudioManager.instance.DialoguePlayer.clip = null;
        //dialogueWaitList.RemoveAt(0);

        if (dialogueWaitList.Count != 0)
        {
            currentCoroutine = StartCoroutine(PlayRecording(dialogueWaitList[0], interuptedIndex));
            dialogueWaitList.RemoveAt(0);
            interuptedIndex = 0;
        }
        else
        {
        }

        yield return null;
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
        UIManager.instance.ClearSubtitle(UIManager.SubtitleType.Dialogue);
        AudioManager.instance.DialoguePlayer.Stop();
        AudioManager.instance.DialoguePlayer.clip = null;
    }

    public void ExitDialogueBound()
    {
        dialogueInBound = false;
        if (currentDialogue != null)
        {
            StopCurrentRecordingLine();
        }
    }

    public void EnterDialogueBound()
    {
        dialogueInBound = true;

        if (currentDialogue != null)
        {
            currentCoroutine = StartCoroutine(PlayRecording(currentDialogue, interuptedIndex));
        }
        else if (dialogueWaitList.Count != 0)
        {
            currentCoroutine = StartCoroutine(PlayRecording(dialogueWaitList[0], 0));
            dialogueWaitList.RemoveAt(0);
        }
        
    }
}

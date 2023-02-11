using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingButton : MonoBehaviour
{
    public DialogueData recording;

    public void playRecording()
    {
        //DialogueManager.instance.WaitlistDialogue(recording);
        DialogueManager.instance.OverrideRecording(recording, true);
        DialogueManager.instance.UnpauseRadio();
    }

}

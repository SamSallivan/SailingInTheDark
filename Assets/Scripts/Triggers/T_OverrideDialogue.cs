using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_OverrideDialogue : Trigger
{
    public DialogueData dialogueData;
    public override IEnumerator TriggerEvent()
    {
        // DialogueManager.instance.OverrideRecording(dialogueData);
        yield return null;
    }
}

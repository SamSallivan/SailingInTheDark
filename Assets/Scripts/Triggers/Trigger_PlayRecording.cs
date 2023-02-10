using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_PlayRecording : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, the recording clip attached to this will be played immediately.
     */
    public override IEnumerator TriggerEvent()
    {
        DialogueManager.instance.OverrideDialogue(recording);
        yield return null;
    }
}

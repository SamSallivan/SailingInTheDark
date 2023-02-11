using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_OverrideRecording : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, the recording clip attached to this will be played immediately.
     */
    public DialogueData recording;
    public bool autoStartPlaying;
    public override IEnumerator TriggerEvent()
    {
        DialogueManager.instance.OverrideRecording(recording, autoStartPlaying);
        yield return null;
    }
}

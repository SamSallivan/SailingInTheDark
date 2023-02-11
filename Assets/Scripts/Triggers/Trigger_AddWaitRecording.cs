using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_AddWaitRecording : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, add the recording clip attached to this to the audio waitlist.
     */

    public DialogueData RecordingClip;
    public override IEnumerator TriggerEvent()
    {
        DialogueManager.instance.WaitlistRecording(RecordingClip);
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_WaitlistRecording : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, add the recording clip attached to this to the audio waitlist.
     */
    public DialogueData recording;

    public override IEnumerator TriggerEvent()
    {
        DialogueManager.instance.WaitlistRecording(recording);
        yield return null;
    }
}

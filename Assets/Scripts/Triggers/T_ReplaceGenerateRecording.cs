using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ReplaceGenerateRecording : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, the recording clip attached to this will be played immediately.
     */
    public ItemData recording;
    public bool autoStartPlaying;
    public override IEnumerator TriggerEvent()
    {
        DialogueManager.instance.ReplaceGenerate(recording, autoStartPlaying);
        yield return null;
    }
}

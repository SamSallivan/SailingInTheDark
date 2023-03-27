using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ReceiveRadio : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, the recording clip attached to this will be played immediately.
     */
    public GameObject generateTape;
    public bool autoPlay;
    public override IEnumerator TriggerEvent()
    {
        //RecordingManager.instance.ReplaceAndGenerate(generateTape, autoStartPlaying);

        RecordingManager.instance.tapePlayer.ReceiveRadio(generateTape);
        //RecordingManager.instance.recorder.radioIndicator.SetActive(true);
        //RecordingManager.instance.recorder.radioTapes.Add(generateTape);
        yield return null;
    }
}

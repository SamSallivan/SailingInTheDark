using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ReplaceGenerateRecording : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, the recording clip attached to this will be played immediately.
     */
    public GameObject tapeGenerated;
    public override IEnumerator TriggerEvent()
    {
        //RecordingManager.instance.ReplaceAndGenerate(tapeGenerated, autoStartPlaying);

        RecordingManager.instance.recorder.ReceiveRadio(tapeGenerated);
        //RecordingManager.instance.recorder.radioIndicator.SetActive(true);
        //RecordingManager.instance.recorder.radioTapes.Add(tapeGenerated);
        yield return null;
    }
}

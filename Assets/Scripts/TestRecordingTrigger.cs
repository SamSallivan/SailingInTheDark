using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRecordingTrigger : Trigger
{
    public override IEnumerator TriggerEvent()
    {
        UIManager.instance.interactionName.text = recording.subtitles[0];
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRecordingTrigger : Trigger
{
    public override IEnumerator TriggerEvent()
    {
        UIManager.instance.PlaySubtitle(recording);
        yield return null;
    }
}

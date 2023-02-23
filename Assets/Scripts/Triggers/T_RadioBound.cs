using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_RadioBound : Trigger
{
    public override IEnumerator TriggerEvent()
    {
        RecordingManager.instance.EnterRadioBound();
        yield return null;
    }

    public void OnTriggerExit(Collider other)
    {
        RecordingManager.instance.ExitRadioBound();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_PauseRadio : Interactable
{
    public override IEnumerator InteractionEvent()
    {
        if (RecordingManager.instance.radioPaused)
        {
            RecordingManager.instance.UnpauseRadio();
            activated = false;
        }
        else
        {
            RecordingManager.instance.PauseRadio();
            activated = true;
        }
        yield return null;
    }
}

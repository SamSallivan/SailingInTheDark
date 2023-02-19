using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_PauseRadio : Interactable
{
    public override IEnumerator InteractionEvent()
    {
        if (DialogueManager.instance.radioPaused)
        {
            DialogueManager.instance.UnpauseRadio();
            activated = false;
        }
        else
        {
            DialogueManager.instance.PauseRadio();
            activated = true;
        }
        yield return null;
    }
}

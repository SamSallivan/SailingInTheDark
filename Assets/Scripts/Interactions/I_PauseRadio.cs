using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_PauseRadio : Interactable
{
    public override IEnumerator InteractionEvent()
    {
        if (!activated)
        {
            activated = true;
            DialogueManager.instance.UnpauseRadio();
        }
        else
        {
            activated = false;
            DialogueManager.instance.PauseRadio();
        }
        yield return null;
    }
}

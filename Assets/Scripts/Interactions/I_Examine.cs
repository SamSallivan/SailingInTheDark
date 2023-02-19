using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class I_Examine : Interactable
{

    public override IEnumerator InteractionEvent()
    {
        if (itemData != null)
        {
            UIManager.instance.Examine(examineText);
            //ObjectiveManager.instance.CompleteObjetive("Read Paper");

        }
        yield return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class I_Examine : Interactable
{

    public override IEnumerator InteractionEvent()
    {
        UIManager.instance.Examine(examineText);

        if (itemData != null)
        {
            //ObjectiveManager.instance.CompleteObjetive("Read Paper");

        }
        yield return null;
    }

}

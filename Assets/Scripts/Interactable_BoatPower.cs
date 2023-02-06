using NWH.DWP2.ShipController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_BoatPower : Interactable
{
    public AdvancedShipController boat;
    public override IEnumerator InteractionEvent()
    {
        if (boat.input.Throttle == 0)
            boat.input.Throttle = 1;
        else
            boat.input.Throttle = 0;

        textPrompt = boat.input.Throttle==1 ? "Turn Off" : "Turn On";
        UI.instance.interactionPrompt.text = "[E] " + textPrompt;

        ObjectiveManager.instance.CompleteObjetive("Turn on boat engine");
        yield return null;
    }
}

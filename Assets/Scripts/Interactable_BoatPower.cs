using NWH.DWP2.ShipController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_BoatPower : Interactable
{
    public AdvancedShipController boat;
    public Interactable_BoatThruster thruster;
    public bool activated;
    public override IEnumerator InteractionEvent()
    {
        if (!activated){
            activated = true;
            boat.input.Throttle = 1;
            PlayerController.instance.TetherToBoat(10, 7.5f);
            textPrompt = "Turn Off";
        }
        else{
            activated = false;
            boat.input.Throttle = 0;
            PlayerController.instance.UntetherFromBoat();
            textPrompt = "Turn On";
        }

        UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;

        ObjectiveManager.instance.CompleteObjetive("Turn on boat engine");
        yield return null;
    }
}

using NWH.DWP2.ShipController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_BoatThruster : Interactable
{
    public AdvancedShipController boat;
    public Interactable_BoatPower power;
    public bool activated;
    public override IEnumerator InteractionEvent()
    {
        if (!activated & power.activated){
            activated = true;
            boat.input.Throttle = 1;
            PlayerController.instance.TetherToBoat(20, 15);
            textPrompt = "Turn Off";
        }
        else if (activated){
            activated = false;
            boat.input.Throttle = 0;
            //boat.engine.
            PlayerController.instance.TetherToBoat(10, 7.5f);
            textPrompt = "Turn On";
        }
        UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;
        yield return null;
    }

    void Update(){
        Debug.Log(boat.Anchor.Dropped);
    }
}

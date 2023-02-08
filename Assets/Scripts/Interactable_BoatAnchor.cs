using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.DWP2.ShipController;

public class Interactable_BoatAnchor : Interactable
{
    public AdvancedShipController boat;
    public bool activated;

    public override IEnumerator InteractionEvent()
    {
        if (!activated){
            activated = true;
            boat.Anchor.Drop();
            textPrompt = "Weigh";
        }
        else if (activated){
            activated = false;
            boat.Anchor.Weigh();
            textPrompt = "Drop";
        }
        UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;
        yield return null;
    }
    void Update(){
        Debug.Log(boat.Anchor.Dropped);
    }

}

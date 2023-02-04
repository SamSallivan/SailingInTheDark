using NWH.DWP2.ShipController;
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

        yield return null;
    }
}

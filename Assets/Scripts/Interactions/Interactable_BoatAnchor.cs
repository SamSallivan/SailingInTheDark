using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.DWP2.ShipController;
using NWH.DWP2.WaterObjects;

public class Interactable_BoatAnchor : Interactable
{
    public AdvancedShipController boat;
    public WaterObject boatHull;
    public bool dockable;
    public T_DockingZone dockingZone;
    public float dockingTimer;

    public override IEnumerator InteractionEvent()
    {
        if (!activated){
            activated = true;
            boat.Anchor.Drop();
            textPrompt = "Weigh";

            if (dockable)
            {
                textPrompt = "Undock";

            }
        }
        else if (activated){
            activated = false;
            boat.Anchor.Weigh();
            textPrompt = "Drop";

            if (dockable)
            {
                textPrompt = "Dock";
                PlayerController.instance.gameObject.transform.SetParent(boat.transform, true);

            }
        }
        UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;
        yield return null;
    }
    void Update(){
        if (dockable && dockingZone != null && activated)
        {
            //boatHull.calculateWaterHeights = false;

            if (dockingTimer <= 2)
            {
                dockingTimer += Time.fixedDeltaTime;
                Vector3 targetpos = new Vector3(dockingZone.dockingPos.position.x, boat.transform.position.y, dockingZone.dockingPos.position.z);
                boat.transform.position = Vector3.Lerp(boat.transform.position, targetpos, dockingTimer*2);
                boat.transform.rotation = Quaternion.Lerp(boat.transform.rotation, dockingZone.dockingPos.rotation, Time.fixedDeltaTime);
                boat.Anchor.AnchorPosition = boat.Anchor.AnchorPoint;
            }
            else
            {
                //PlayerController.instance.gameObject.transform.parent = null;
            }
        }
        else
        {
            dockingTimer = 0;
            boatHull.calculateWaterHeights = true;
        }
    }

}

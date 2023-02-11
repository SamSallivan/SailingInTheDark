using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.DWP2.ShipController;
using NWH.DWP2.WaterObjects;

public class I_BoatAnchor : Interactable
{
    public AdvancedShipController boat;
    public WaterObject boatHull;
    public bool dockable;
    public T_DockingZone dockingZone;
    public float dockingTimer;
    public Vector3 playerPos;

    public override IEnumerator InteractionEvent()
    {
        if (!activated){
            activated = true;
            boat.Anchor.Drop();
            textPrompt = "Weigh";

            if (dockable)
            {
                textPrompt = "Undock";
                playerPos = PlayerController.instance.transform.localPosition;
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
    void FixedUpdate(){
        if (dockable && dockingZone != null && activated)
        {
            //boatHull.calculateWaterHeights = false;

            if (dockingTimer <= 2.5f)
            {
                dockingTimer += Time.fixedDeltaTime;
                Vector3 targetpos = new Vector3(dockingZone.dockingPos.position.x, boat.transform.position.y, dockingZone.dockingPos.position.z);
                boat.transform.position = Vector3.Lerp(boat.transform.position, targetpos, dockingTimer / 2);
                boat.transform.rotation = Quaternion.Lerp(boat.transform.rotation, dockingZone.dockingPos.rotation, Time.fixedDeltaTime * 1.5f);
                boat.Anchor.AnchorPosition = boat.Anchor.AnchorPoint;

                if (dockingTimer <= 1f && PlayerController.instance.transform.parent == BoatController.instance.transform)
                {
                    PlayerController.instance.enableMovement = false;
                    PlayerController.instance.transform.localPosition = playerPos;
                }
                else
                {
                    PlayerController.instance.enableMovement = true;
                }
            }
            else
            {
                PlayerController.instance.enableMovement = true;
            }
        }
        else
        {
            dockingTimer = 0;
            boatHull.calculateWaterHeights = true;
        }
    }

}

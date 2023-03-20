using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.DWP2.ShipController;
using NWH.DWP2.WaterObjects;
using MyBox;

public class I_Anchor : Interactable
{
    [Foldout("Docking", true)]
    public AdvancedShipController boat;
    public WaterObject boatHull;
    [ReadOnly]
    public bool dockable;
    [ReadOnly]
    public T_DockingZone dockingZone;
    [ReadOnly]
    public float dockingTimer;

    [Foldout("Boat Component", true)]
    public float wattConsumption = 10;

    public bool componentActivated = false;

    public override IEnumerator InteractionEvent()
    {
        boat = BoatController.instance.boat;
        activated = !activated;
        Target();

        if (activated){
            boat.Anchor.Drop();
            BoatController.instance.helm.ShutDown();
            textPromptActivated = "Weigh";

            if (dockable)
            {
                textPromptActivated = "Undock";
            }
        }
        else if (!activated)
        {
            boat.Anchor.Weigh();
            textPrompt = "Drop";

            if (dockable)
            {
                textPrompt = "Dock";
            }
        }
        yield return null;
    }
    void FixedUpdate(){

        if (!activated)
        {
            if (dockable && dockingZone != null)
            {
                highlightTarget.GetComponent<Renderer>().material.color = Color.green;
            }
            else if(!dockable)
            {
                highlightTarget.GetComponent<Renderer>().material.color = Color.grey;
            }
        }
        else
        {
            highlightTarget.GetComponent<Renderer>().material.color = Color.red;
        }

        if (dockable && dockingZone != null && activated)
        {
            //boatHull.calculateWaterHeights = false;
            Quaternion rotation1 = dockingZone.dockingPos.rotation;
            Quaternion rotation2 = Quaternion.Euler(rotation1.eulerAngles.x, rotation1.eulerAngles.y + 180, rotation1.eulerAngles.z);
            Quaternion rotation;

            //Debug.Log("1:" + Quaternion.Angle(rotation1, boat.transform.rotation));
            //Debug.Log("2:" + Quaternion.Angle(rotation2, boat.transform.rotation));

            if (Quaternion.Angle(rotation1, boat.transform.rotation) < Quaternion.Angle(rotation2, boat.transform.rotation))
            {
                rotation = rotation1;
            }
            else
            {
                rotation = rotation2;
            }

            if (dockingTimer <= 2.5f)
            {
                //dockingTimer += Time.fixedDeltaTime;
                Vector3 targetpos = new Vector3(dockingZone.dockingPos.position.x, boat.transform.position.y,
                    dockingZone.dockingPos.position.z);
                boat.transform.position = Vector3.Lerp(boat.transform.position, targetpos, dockingTimer / 2);
                boat.transform.rotation = Quaternion.Lerp(boat.transform.rotation, rotation,Time.fixedDeltaTime * 1.5f);
                //boat.transform.position = Vector3.MoveTowards(boat.transform.position, targetpos, dockingTimer / 2);
                //boat.transform.rotation = Quaternion.Lerp(boat.transform.rotation, rotation,Time.fixedDeltaTime * 1.5f);
                boat.Anchor.AnchorPosition = boat.Anchor.AnchorPoint;
            }
        }
        else
        {
            dockingTimer = 0;
        }
    }

}

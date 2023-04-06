using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.DWP2.ShipController;
using NWH.DWP2.WaterObjects;
using MyBox;

public class I_Anchor : Interactable
{
    [Foldout("Docking", true)]
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
        AnchorSwitch();
        Target();

        yield return null;
    }

    void FixedUpdate()
    {

        if (!activated)
        {
            if (dockable && dockingZone != null)
            {
                highlightTarget.GetComponent<Renderer>().material.color = Color.green;
            }
            else if (!dockable)
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

            if (Quaternion.Angle(rotation1, BoatController.instance.transform.rotation) < Quaternion.Angle(rotation2, BoatController.instance.transform.rotation))
            {
                rotation = rotation1;
            }
            else
            {
                rotation = rotation2;
            }

            if (dockingTimer <= 10f)
            {
                dockingTimer += Time.fixedDeltaTime;
                Vector3 targetpos = new Vector3(dockingZone.dockingPos.position.x, BoatController.instance.transform.position.y,
                    dockingZone.dockingPos.position.z);
                BoatController.instance.transform.position = Vector3.Lerp(BoatController.instance.transform.position, targetpos, Time.fixedDeltaTime * 1.5f);
                BoatController.instance.transform.rotation = Quaternion.Lerp(BoatController.instance.transform.rotation, rotation, Time.fixedDeltaTime * 1);
                //boat.transform.position = Vector3.MoveTowards(boat.transform.position, targetpos, dockingTimer / 2);
                //boat.transform.rotation = Quaternion.Lerp(boat.transform.rotation, rotation,Time.fixedDeltaTime * 1.5f);
                BoatController.instance.boat.Anchor.AnchorPosition = BoatController.instance.boat.Anchor.AnchorPoint;
            }
        }
        else
        {
            dockingTimer = 0;
        }
    }

    public void AnchorSwitch()
    {
        activated = !activated;

        if (activated)
        {
            BoatController.instance.boatAudio.PlayAnchorDownSound();

            BoatController.instance.boat.Anchor.Drop();
            BoatController.instance.helm.ShutDown();
            textPromptActivated = "Weigh";
            UIManager.instance.anchorText.text = "[X] Weigh Anchor";

            if (dockable)
            {
                textPromptActivated = "Undock";
                UIManager.instance.anchorText.text = "[X] Undock Boat";
                StartCoroutine(SaveGame());
            }

            UIManager.instance.anchorImage.SetActive(true);
        }
        else if (!activated)
        {
            BoatController.instance.boatAudio.PlayAnchorUpSound();

            BoatController.instance.boat.Anchor.Weigh();
            textPrompt = "Drop";
            UIManager.instance.anchorText.text = "[X] Drop Anchor";

            if (dockable)
            {
                textPrompt = "Dock";
                UIManager.instance.anchorText.text = "[X] Dock Boat";
            }

            UIManager.instance.anchorImage.SetActive(false);
        }
    }

    public void Initialize(bool anchored)
    {
        activated = anchored;

        if (anchored)
        {
            BoatController.instance.boat.Anchor.Drop();
            BoatController.instance.helm.ShutDown();
            textPromptActivated = "Weigh";
            UIManager.instance.anchorText.text = "[X] Weigh Anchor";

            if (dockable)
            {
                textPromptActivated = "Undock";
                UIManager.instance.anchorText.text = "[X] Undock Boat";
            }

            UIManager.instance.anchorImage.SetActive(true);
        }
        else if (!anchored)
        {
            BoatController.instance.boat.Anchor.Weigh();
            textPrompt = "Drop";
            UIManager.instance.anchorText.text = "[X] Drop Anchor";

            if (dockable)
            {
                textPrompt = "Dock";
                UIManager.instance.anchorText.text = "[X] Dock Boat";
            }

            UIManager.instance.anchorImage.SetActive(false);
        }
    }

    public IEnumerator SaveGame()
    {
        yield return new WaitForSeconds(1f);
        SaveManager.instance.Save();
    }

}

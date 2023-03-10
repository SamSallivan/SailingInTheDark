using NWH.DWP2.ShipController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Wheel : Interactable
{
    public Transform playerTargetPos;
    public Transform wheel;
    public float wheelAngle;
    public float hTemp;
    public override IEnumerator InteractionEvent()
    {
        activated = !activated;
        PlayerController.instance.enableMovement = !PlayerController.instance.enableMovement;
        textPrompt = activated ? "Exit" : "Use";
        UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;
        yield return null;
    }
    void Update()
    {
        if (activated)
        {
            PlayerController.instance.transform.position = Vector3.Lerp(PlayerController.instance.transform.position, playerTargetPos.position, Time.deltaTime * 5f);

            BoatController.instance.boat.input.Steering = hTemp;
            //wheelAngle = Mathf.MoveTowards(wheelAngle, -hTemp * 90, 1.5f);
            wheelAngle = Mathf.Lerp(wheelAngle, -hTemp * 90, Time.deltaTime * 5);
        }
        else
        {
            hTemp = 0;
            BoatController.instance.boat.input.Steering = 0;
            wheelAngle = Mathf.Lerp(wheelAngle, -hTemp * 90, Time.deltaTime * 5);
        }
        wheel.localEulerAngles = new Vector3(0, 0, wheelAngle);
    }
}

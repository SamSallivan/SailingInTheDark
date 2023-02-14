using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_WheelThrottle : Interactable
{
    [Header("Boat Movements")]
    public float speed;
    public float currentGear = 0;
    public int currentMaxGear = 2;
    public int totalGearNumber = 5;
    public float vTemp;
    public float hTemp;

    [Header("Handle")]
    public float handleSpeed = 1;
    public float handleAngle;
    public Transform handle;

    [Header("Wheel")]
    public float wheelSpeed;
    public float wheelAngle;
    public Transform wheel;
    public Vector3 playerPos;

    public override IEnumerator InteractionEvent()
    {
        activated = !activated;
        playerPos = PlayerController.instance.transform.localPosition;
        PlayerController.instance.enableMovement = !PlayerController.instance.enableMovement;
        textPrompt = activated ? "Exit" : "Use";
        UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;
        yield return null;
    }

    void FixedUpdate()
    {
        if (activated)
        {
            PlayerController.instance.transform.localPosition = playerPos;


            hTemp = 0f;
            hTemp += (Input.GetKey(KeyCode.A) ? (-1) : 0);
            hTemp += (Input.GetKey(KeyCode.D) ? 1 : 0);
            hTemp = Mathf.Clamp(hTemp, -1, 1);
            wheelAngle = Mathf.Lerp(wheelAngle, -hTemp * 90, Time.deltaTime * 5);
            BoatController.instance.boat.input.Steering = hTemp;


            int speedPerGear = 100 / totalGearNumber;

            vTemp += (Input.GetKey(KeyCode.S) ? (-handleSpeed) : 0);
            vTemp += (Input.GetKey(KeyCode.W) ? handleSpeed : 0);
            vTemp = Mathf.Clamp(vTemp, 0, currentMaxGear * speedPerGear);

            //round to product of speed per gear
            currentGear = Mathf.Round(vTemp / speedPerGear);
            speed = currentGear * speedPerGear;
            BoatController.instance.boat.input.Throttle = speed / 100;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
            {
                handleAngle = vTemp / 100 * 180;
            }
            else
            {
                vTemp = speed;
                handleAngle = Mathf.Lerp(handleAngle, speed / 100 * 180, Time.deltaTime * 10);
            }
        }
        else
        {
            hTemp = 0;
            BoatController.instance.boat.input.Steering = 0;
            wheelAngle = Mathf.Lerp(wheelAngle, -hTemp * 90, Time.deltaTime * 5);
        }

        wheel.localEulerAngles = new Vector3(0, 0, wheelAngle);
        handle.localEulerAngles = new Vector3(handleAngle, 0, 0);

        if (PlayerController.instance.transform.parent == BoatController.instance.transform)
        {
            PlayerController.instance.TetherToBoat(BoatController.instance.boat.Speed * 0.75f, 0);
        }
        else
        {
            PlayerController.instance.UntetherFromBoat();
        }

        //Debug.Log(BoatController.instance.boat.Speed);
    }
}

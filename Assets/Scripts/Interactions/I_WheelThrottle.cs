using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoatComponent))]
public class I_WheelThrottle : Interactable
{

    [Foldout("Controls", true)]
    [Header("Boat Movements")]
    private float speed;
    [ReadOnly]
    public float currentGear = 0;
    public int currentMaxGear = 2;
    public int totalGearNumber = 5;
    private float vTemp;
    private float hTemp;

    [Header("Handle")]
    public float handleSpeed = 1;
    private float handleAngle;
    public Transform handle;

    [Header("Wheel")]
    public float wheelSpeed = 1;
    private float wheelAngle;
    public Transform wheel;
    [ReadOnly]
    public Vector3 playerPos;

    public override IEnumerator InteractionEvent()
    {
        playerPos = PlayerController.instance.transform.localPosition;
        if (activated){
            PlayerController.instance.LockMovement(true);
        }
        else{
            PlayerController.instance.LockMovement(false);
        }
        yield return null;
    }

    void FixedUpdate()
    {
        if (activated)
        {
            PlayerController.instance.transform.localPosition = playerPos;


            hTemp = 0f;
            hTemp += (Input.GetKey(KeyCode.A) ? (-wheelSpeed) : 0);
            hTemp += (Input.GetKey(KeyCode.D) ? wheelSpeed : 0);
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
            if (speed >= 0)
            {
                GetComponent<BoatComponent>().componentActivated = true;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
            {
                handleAngle = vTemp / 100 * 180;
            }
            else
            {
                vTemp = speed;
                //handleAngle = Mathf.Lerp(handleAngle, speed / 100 * 180, Time.deltaTime * 10);
            }
        }
        else
        {
            hTemp = 0;
            BoatController.instance.boat.input.Steering = 0;
            wheelAngle = Mathf.Lerp(wheelAngle, -hTemp * 90, Time.deltaTime * 5);
        }

        wheel.localEulerAngles = new Vector3(0, 0, wheelAngle);
        handleAngle = Mathf.Lerp(handleAngle, speed / 100 * 180, Time.deltaTime * 10);
        handle.localEulerAngles = new Vector3(handleAngle, 0, 0);


        if (BoatController.instance.boat.input.Throttle != 0)
        {
            GetComponent<BoatComponent>().componentActivated = true;
        }
        else
        {
            GetComponent<BoatComponent>().componentActivated = false;
        }

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

    public override void ShutDown()
    {
        activated = false;
        GetComponent<BoatComponent>().componentActivated = false;
        PlayerController.instance.LockMovement(false);
        BoatController.instance.boat.input.Throttle = 0; 
        vTemp = 0f;
        currentGear = 0;
        speed = 0;
        UIManager.instance.interactionPrompt.text = "[E] ";
        UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;

    }
}

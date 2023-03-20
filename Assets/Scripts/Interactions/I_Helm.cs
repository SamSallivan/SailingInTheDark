using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoatComponent))]
public class I_Helm : Interactable
{

    [Foldout("Controls", true)]
    [Header("Boat Movements")]
    [ReadOnly]
    public float speed;
    [ReadOnly]
    public float currentGear = 0;
    public int currentMaxGear = 2;
    public int totalGearNumber = 5;
    [ReadOnly]
    private float vTemp;
    [ReadOnly]
    private float hTemp;

    [Header("Handle")]
    public float handleSpeed = 1;
    [ReadOnly]
    private float handleAngle;
    public Transform handle;

    [Header("Wheel")]
    public float wheelSpeed = 1;
    [ReadOnly]
    private float wheelAngle;
    public Transform wheel;

    [Header("Enter & Exit")]
    [ReadOnly]
    public bool inControl;

    [ReadOnly] public float enterTimer;
    public Transform playerPos;
    public TMP_Text GearText;

    public override IEnumerator InteractionEvent()
    {

        if (!activated && !inControl)
        {
            activated = true;
            Target();
        }
        yield return null;
    }

    void FixedUpdate()
    {
        if (activated && !inControl)
        {
            Highlight();
            UIManager.instance.generalTips.SetActive(false);
            UIManager.instance.helmTips.SetActive(true);
            PlayerController.instance.LockMovement(true);
            if (enterTimer < 0.5f)
            {
                enterTimer += Time.deltaTime;

                Vector3 targetPos = new Vector3(playerPos.localPosition.x, PlayerController.instance.transform.localPosition.y, playerPos.localPosition.z);
                PlayerController.instance.transform.localPosition = Vector3.Lerp(PlayerController.instance.transform.localPosition, playerPos.localPosition, Time.deltaTime * 5);
                //PlayerController.instance.transform.localPosition = Vector3.Lerp(PlayerController.instance.transform.localPosition, targetPos, Time.deltaTime * 5);
                //PlayerController.instance.headPosition.Slide(0.25f);

                PlayerController.instance.LockCamera(true);
                PlayerController.instance.transform.localRotation = Quaternion.Lerp(PlayerController.instance.transform.localRotation, Quaternion.identity, Time.deltaTime * 5);
                PlayerController.instance.tHead.localRotation = Quaternion.Lerp(PlayerController.instance.tHead.localRotation, Quaternion.identity, Time.deltaTime * 5);
            }
            else
            {
                inControl = true;
                enterTimer = 0;

                PlayerController.instance.GetComponent<MouseLook>().Reset();
                PlayerController.instance.tHead.GetComponent<MouseLook>().Reset();
                PlayerController.instance.GetComponent<MouseLook>().SetClamp(-120, 120, -60, 60);
                PlayerController.instance.tHead.GetComponent<MouseLook>().SetClamp(-120, 120, -60, 60);
                PlayerController.instance.LockCamera(false);
            }
        }
        else if (!activated && inControl)
        {
            inControl = false;
            UIManager.instance.generalTips.SetActive(true);
            UIManager.instance.helmTips.SetActive(false);
            //PlayerController.instance.headPosition.Slide(0.75f);
            PlayerController.instance.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);
            PlayerController.instance.tHead.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);
            PlayerController.instance.LockMovement(false);

        }

        if (inControl)
        {
            Highlight();

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
            {
                activated = false;
                InventoryManager.instance.inputDelay = 0;
            }

            hTemp = 0f;
            hTemp += (Input.GetKey(KeyCode.A) ? (-wheelSpeed) : 0);
            hTemp += (Input.GetKey(KeyCode.D) ? wheelSpeed : 0);
            hTemp = Mathf.Clamp(hTemp, -1, 1);
            wheelAngle = Mathf.Lerp(wheelAngle, -hTemp * 90, Time.deltaTime * 5);
            BoatController.instance.boat.input.Steering = hTemp;
            

            int speedPerGear = 100 / totalGearNumber;

            vTemp += (Input.GetKey(KeyCode.S) ? (-handleSpeed) : 0);
            vTemp += (Input.GetKey(KeyCode.W) ? handleSpeed : 0);
            vTemp = Mathf.Clamp(vTemp, -currentMaxGear * speedPerGear, currentMaxGear * speedPerGear);

            //round to product of speed per gear
            currentGear = Mathf.Round(vTemp / speedPerGear);
            speed = currentGear * speedPerGear;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
            {
                handleAngle = vTemp / 100 * 90;
            }
            else
            {
                vTemp = speed;
            }
        }
        else
        {
            hTemp = 0;
            BoatController.instance.boat.input.Steering = 0;
            wheelAngle = Mathf.Lerp(wheelAngle, -hTemp * 90, Time.deltaTime * 5);
        }

        /*if (BoatController.instance.engine.activated)
        {
            BoatController.instance.boat.input.Throttle = speed / 100;
        }
        else
        {
            BoatController.instance.boat.input.Throttle = 0;
        }*/

        BoatController.instance.boat.input.Throttle = speed / 100;

        wheel.localEulerAngles = new Vector3(0, 0, wheelAngle);
        handleAngle = Mathf.Lerp(handleAngle, speed / 100 * 90, Time.deltaTime * 10);
        handle.localEulerAngles = new Vector3(handleAngle, 0, 0);

        if (BoatController.instance.boat.input.Throttle != 0)
        {
            GetComponent<BoatComponent>().componentActivated = true;
        }
        else
        {
            GetComponent<BoatComponent>().componentActivated = false;
        }

        GearText.text = "Gear: " + currentGear;
    }

    public override void ShutDown()
    {
        vTemp = 0f;
        currentGear = 0;
        speed = 0;

        GetComponent<BoatComponent>().componentActivated = false;
        BoatController.instance.boat.input.Throttle = 0; 

        if (PlayerController.instance.targetInteractable == this)
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
        }

        if (activated)
        {
            PlayerController.instance.exclusiveInteractable = null;
        }

    }

    public void Highlight()
    {
        if (highlightTarget != null)
        {
            if (!highlightTarget.GetComponent<OutlineRenderer>())
            {
                OutlineRenderer outline = highlightTarget.AddComponent<OutlineRenderer>();
                outline.OutlineMode = OutlineRenderer.Mode.OutlineVisible;
                outline.OutlineWidth = 10;
            }
        }
    }

    public override void Target()
    {
        if (highlightTarget != null && !highlightTarget.GetComponent<OutlineRenderer>())
        {
            OutlineRenderer outline = highlightTarget.AddComponent<OutlineRenderer>();
            outline.OutlineMode = OutlineRenderer.Mode.OutlineVisible;
            outline.OutlineWidth = 10;
        }

        /*if (!BoatController.instance.engine.activated)
        {
            UIManager.instance.interactionPrompt.text = "Engine is off";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }

        else if (BoatController.instance.anchor.activated)
        {
            UIManager.instance.interactionPrompt.text = "Anchor is dropped";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }
        else
        {
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }*/

        if (activated)
        {
            UIManager.instance.interactionName.text = "";
            UIManager.instance.interactionPrompt.text = "";
        }
        else if (!activated)
        {
            UIManager.instance.interactionName.text = textName;
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
        }

    }
}

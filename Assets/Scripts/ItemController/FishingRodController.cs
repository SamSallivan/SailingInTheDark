using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    public GameObject floatPrefab;
    public GameObject floatObject;
    public ItemData fishHooked;

    public Transform CastPos;
    public Vector2 CastForce = new Vector2(1, 5);
    public float castCharger;
    public float waitTimer = 0;
    public float reactTimer = 0;
    public float reelCharger;

    public List<ItemData> fishList = new List<ItemData>();
    public Vector2 WaitInterval = new Vector2(2.5f, 5);
    public float waitTimeFrame = 0;
    public float reactTimeFrame = 1;
    public float reelIncreaseCoefficient = 1;
    public float reelDecreaseCoefficient = 1;

    public enum FishingState
    {
        Idle,
        Casting,
        Waiting,
        Reacting,
        Nibbling,
        Reeling,
    }
    public FishingState fishingState;

    void Update()
    {

        if (transform.IsChildOf(PlayerController.instance.transform) && !UIManager.instance.inventoryUI.activeInHierarchy)
        {
            KeyCode key;

            if (transform.IsChildOf(PlayerController.instance.equippedTransformLeft))
            {
                key = KeyCode.Mouse0;
            }
            else
            {
                key = KeyCode.Mouse1;
            }

            Quaternion targetRotation = Quaternion.Euler(30, 0, 0);

            switch (fishingState)
            {
                case FishingState.Idle:

                    UIManager.instance.fishingUI.SetActive(false);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10f);
                    if (Input.GetKeyDown(key))
                    {
                        fishingState = FishingState.Casting;
                    }
                    break;

                case FishingState.Casting:

                    if (Input.GetKey(key))
                    {
                        //castCharger = Mathf.MoveTowards(castCharger, 1, Time.deltaTime);
                        castCharger = Mathf.Lerp(castCharger, 1, Time.deltaTime);
                        //Quaternion targetRotation = Quaternion.Euler(Mathf.Lerp(30, -30, castCharger), 0, 0);
                        Quaternion startRotation = Quaternion.Euler(30, 0, 0);
                        targetRotation = Quaternion.Euler(-15, 0, 0);
                        transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, castCharger);
                    }

                    if (Input.GetKeyUp(key))
                    {
                        Cast();
                        fishingState = FishingState.Waiting;
                    }
                    break;

                case FishingState.Waiting:

                    targetRotation = Quaternion.Euler(45, 0, 0);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10f);
                    waitTimer = Mathf.MoveTowards(waitTimer, waitTimeFrame, Time.deltaTime);
                    if (Input.GetKeyDown(key))
                    {
                        Reset();
                        fishingState = FishingState.Idle;
                    }
                    if (waitTimer >= waitTimeFrame) //if (Random.Range(0, averageWaitSeconds / Time.fixedDeltaTime) < 1)
                    {
                        fishHooked = fishList[Random.Range(0, fishList.Count)];
                        //fishingState = FishingState.Reacting;
                        fishingState = FishingState.Reeling;
                        reelCharger = 0.99f;
                    }
                    break;

                /*case FishingState.Reacting:

                    reactTimer  = Mathf.MoveTowards(reactTimer, reactTimeFrame, Time.fixedDeltaTime);
                    UIManager.instance.fishingUI.SetActive(true);
                    UIManager.instance.reelSlider.value = 1 - (reactTimer / reactTimeFrame);

                    if (Input.GetKeyDown(key))
                    {
                        reelCharger = 1 - (reactTimer / reactTimeFrame);
                        fishingState = FishingState.Reeling; 
                    }


                    if (reactTimer >= reactTimeFrame)
                    {
                        fishHooked = null;
                        waitTimer = 0;
                        reactTimer = 0;
                        waitTimeFrame = Random.Range(WaitInterval.x, WaitInterval.y);
                    }
                    break;*/

                case FishingState.Reeling:

                    UIManager.instance.fishingUI.SetActive(true);
                    if (reelCharger <= 0)
                    {
                        Reset();
                    }
                    else if (reelCharger < 1)
                    {
                        reelCharger = Mathf.MoveTowards(reelCharger, 0, Time.fixedDeltaTime * reelDecreaseCoefficient);
                        targetRotation = Quaternion.Euler(60, 0, 0);
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime);

                        if (Input.GetKeyDown(key))
                        {
                            reelCharger = Mathf.MoveTowards(reelCharger, 1, Time.fixedDeltaTime * reelIncreaseCoefficient);
                            targetRotation = Quaternion.Euler(0, 0, 0);
                            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10);
                        }

                        UIManager.instance.reelSlider.value = reelCharger;
                    }
                    else if (reelCharger >= 1)
                    {
                        InventoryItem newItem = InventoryManager.instance.AddItem(fishHooked, new ItemStatus(1, 1));
                        if (newItem != null)
                        {
                            //InventoryManager.instance.EquipItem(newItem);
                            InventoryManager.instance.OpenInventory();
                            InventoryManager.instance.selectedPosition = InventoryManager.instance.GetGridPosition(newItem.slot.GetIndex());
                        }
                        Reset();
                    }

                    break;

            }

            if (floatObject != null)
            {
                LineRenderer line = GetComponent<LineRenderer>();
                line.SetPosition(0, CastPos.position);
                line.SetPosition(1, floatObject.transform.position);
            }
        }

    }

    public void Cast()
    {
        floatObject = Instantiate(floatPrefab, CastPos.position, Quaternion.identity,null);
        floatObject.GetComponent<Rigidbody>().AddForce(PlayerController.instance.tHead.forward * Mathf.Lerp(CastForce.x, CastForce.y, castCharger), ForceMode.Impulse);
        floatObject.GetComponent<SpringJoint>().connectedBody = GetComponent<Rigidbody>();
        
        LineRenderer line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, CastPos.position);
        line.SetPosition(1, floatObject.transform.position);

        castCharger = 0;
        waitTimer = 0;

        waitTimeFrame = Random.Range(WaitInterval.x, WaitInterval.y);
    }
    public void Reset()
    {
        UIManager.instance.fishingUI.SetActive(false);
        GetComponent<LineRenderer>().positionCount = 0;
        Destroy(floatObject);
        floatObject = null;
        fishHooked = null;
        waitTimer = 0;
        reactTimer = 0;
        reelCharger = 0;

        fishingState = FishingState.Idle;
        waitTimeFrame = Random.Range(WaitInterval.x, WaitInterval.y);
    }

    public void OnDisable() {
        Reset();
    }
}

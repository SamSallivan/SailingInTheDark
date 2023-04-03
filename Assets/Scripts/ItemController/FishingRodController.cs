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
    public float reelCharger;

    public List<ItemData> fishList = new List<ItemData>();
    public float averageWaitSeconds = 5;
    public float reactTimeFrame = 1;
    public float reactTimer = 0;
    public bool reacted = false;
    public float reelTimeFrame = 5;
    public float reelTimer = 0;
    public float reelIncreaseCoefficient = 1;
    public float reelDecreaseCoefficient = 1;

    void Update()
    {

        if (transform.IsChildOf(PlayerController.instance.transform) &&
            !UIManager.instance.inventoryUI.activeInHierarchy)
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

            if (floatObject == null)
            {
                if (Input.GetKey(key))
                {
                    castCharger = Mathf.MoveTowards(castCharger, 1, Time.fixedDeltaTime);
                }

                if (Input.GetKeyUp(key))
                {
                    Cast();
                }
            }
            else
            {
                if (fishHooked == null)
                {
                    UIManager.instance.fishingUI.SetActive(false);

                    if (Input.GetKeyDown(key))
                    {
                        Reel();
                    }
                    else if (Random.Range(0, averageWaitSeconds / Time.fixedDeltaTime) < 1)
                    {
                        fishHooked = fishList[Random.Range(0, fishList.Count)];
                    }

                }
                else
                {
                    UIManager.instance.fishingUI.SetActive(true);

                    if (!reacted)
                    {
                        if (reactTimer < reactTimeFrame)
                        {
                            reactTimer += Time.fixedDeltaTime;
                            UIManager.instance.reelSlider.value = 1 - (reactTimer / reactTimeFrame);

                            if (Input.GetKeyDown(key))
                            {
                                reacted = true;
                                reelCharger = 1 - (reactTimer / reactTimeFrame);
                            }
                        }
                        else if (reactTimer >= reactTimeFrame)
                        {
                            fishHooked = null;
                            reactTimer = 0;
                            reacted = false;
                        }
                    }
                    else if (reacted)
                    {
                        if (reelCharger < 1)
                        {
                            if (reelTimer < reelTimeFrame)
                            {
                                reelTimer += Time.fixedDeltaTime;

                                reelCharger = Mathf.MoveTowards(reelCharger, 0,
                                    Time.fixedDeltaTime * reelDecreaseCoefficient);

                                if (Input.GetKeyDown(key))
                                {
                                    reelCharger = Mathf.MoveTowards(reelCharger, 1,
                                        Time.fixedDeltaTime * reelIncreaseCoefficient);
                                }

                                UIManager.instance.reelSlider.value = reelCharger;
                            }
                            else
                            {
                                fishHooked = null;
                                reactTimer = 0;
                                reacted = false;
                                reelTimer = 0;

                            }
                        }
                        else if (reelCharger >= 1)
                        {
                            Reel();
                            InventoryItem newItem = InventoryManager.instance.AddItem(fishHooked, new ItemStatus(1, 1));
                            if (newItem != null)
                            {
                                InventoryManager.instance.EquipItem(newItem);
                                //InventoryManager.instance.OpenInventory();
                                //InventoryManager.instance.selectedPosition = InventoryManager.instance.GetGridPosition(newItem.slot.GetIndex());
                            }

                            fishHooked = null;
                        }
                    }
                }
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
    }
    public void Reel()
    {
        Destroy(floatObject);
        floatObject = null;
        LineRenderer line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        reelCharger = 0;
        UIManager.instance.fishingUI.SetActive(false);
    }
}

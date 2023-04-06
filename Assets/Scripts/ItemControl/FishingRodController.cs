using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using NWH.DWP2.WaterObjects;
using UnityEngine.InputSystem;

public class FishingRodController : MonoBehaviour
{
    public GameObject floatPrefab;
    public GameObject floatObject;

    [ReadOnly()]
    public T_FishingZone fishingZone;
    public List<FishData> fishListDefault;

    public Transform CastPos;
    public Vector2 CastForce = new Vector2(1, 5);
    [ReadOnly()]
    public float castCharger;

    public Vector2 waitTimeInterval;
    [ReadOnly()]
    public float waitTime;
    [ReadOnly()]
    public float waitTimer = 0;

    public Vector2 nibbleCountInterval;
    public Vector2 nibbleTimeInterval;
    [ReadOnly()]
    public int nibbleCount = 0;
    public float nibbleForce = 1;

    [ReadOnly()]
    public ItemData fishHooked;
    public float reactTimeDefault = 1;
    [ReadOnly()]
    public float reactTimer = 0;

    public Vector3 shakeDefaultPos;
    [ReadOnly()]
    public Vector3 shakeTargetPos;
    public Vector2 shakeRandomX;
    public Vector2 shakeRandomY;
    public Vector2 shakeRandomZ;
    [ReadOnly()]
    public float shakeTimer = 0;
    public float shakeFrequency = 0;
    public float shakeSpeed = 1;

    public float reelIncreaseCoefficient = 1;
    [ReadOnly()]
    public float reelDecreaseCoefficient;
    [ReadOnly()]
    public float reelCharger;

    public FishingAudio fishingAudio;
    public bool lureTouchWater = false;


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

        if (transform.IsChildOf(PlayerController.instance.transform) && PlayerController.instance.enableMovement)
        {
            KeyCode key;
            KeyCode key1 = KeyCode.Mouse0; ;
            KeyCode key2 = KeyCode.Mouse0;

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
                    if (Input.GetKeyDown(key) || Input.GetKeyDown(key2))
                    {
                        fishingAudio.PlayReadyRod();
                        fishingState = FishingState.Casting;
                    }
                    break;

                case FishingState.Casting:

                    if (Input.GetKey(key) || Input.GetKey(key2))
                    {
                        //castCharger = Mathf.MoveTowards(castCharger, 1, Time.deltaTime);
                        //castCharger = Mathf.Lerp(castCharger, 0.5f, Time.deltaTime);
                        castCharger += Time.deltaTime;
                        //Quaternion targetRotation = Quaternion.Euler(Mathf.Lerp(30, -30, castCharger), 0, 0);
                        Quaternion startRotation = Quaternion.Euler(30, 0, 0);
                        targetRotation = Quaternion.Euler(-15, 0, 0);
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, castCharger);
                    }

                    if (Input.GetKeyUp(key) || Input.GetKeyUp(key2))
                    {
                        Cast();
                        fishingState = FishingState.Waiting;
                    }
                    break;

                case FishingState.Waiting:
                    targetRotation = Quaternion.Euler(45, 0, 0);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10f);
                    if (floatObject.transform.GetChild(0).GetComponent<WaterObject>().IsTouchingWater())
                    {
                        if (lureTouchWater == false)
                        {
                            lureTouchWater = true;
                            fishingAudio.PlayLureLand();
                        }

                        //waitTimer = Mathf.MoveTowards(waitTimer, waitTime, Time.deltaTime);
                        waitTimer += Time.deltaTime;
                    }

                    if (Input.GetKeyDown(key) || Input.GetKeyDown(key2))
                    {
                        Reset();
                        StartCoroutine(fishingAudio.PlayEarlyPull());
                    }


                    if (waitTimer >= waitTime) //if (Random.Range(0, averageWaitSeconds / Time.fixedDeltaTime) < 1)
                    {
                        if (fishingZone != null)
                        {
                            fishHooked = RandomFish(fishingZone.fishList);
                        }
                        else
                        {
                            fishHooked = RandomFish(fishListDefault);
                        }

                        if (nibbleCount > 0)
                        {
                            //fishingState = FishingState.Nibbling;
                            floatObject.GetComponent<Rigidbody>().AddForce(Vector3.down * nibbleForce, ForceMode.Impulse);
                            waitTimer -= Random.Range(nibbleTimeInterval.x, nibbleTimeInterval.y);
                            nibbleCount--;
                            transform.localRotation = Quaternion.Euler(50, 0, 0);
                            fishingAudio.PlayFakeBite();
                        }
                        else
                        {
                            fishingState = FishingState.Reacting;
                            StartCoroutine(fishingAudio.PlayRealBite());
                        }
                    }
                    break;

                case FishingState.Nibbling:

                    floatObject.GetComponent<Rigidbody>().AddForce(Vector3.down * nibbleForce, ForceMode.Impulse);
                    break;

                case FishingState.Reacting:

                    if (shakeTimer < shakeFrequency)
                    {
                        shakeTimer += Time.deltaTime;
                    }
                    else
                    {
                        shakeTimer = 0;
                        shakeTargetPos = shakeDefaultPos + new Vector3(Random.Range(shakeRandomX.x, shakeRandomX.y), Random.Range(shakeRandomY.x, shakeRandomY.y), Random.Range(shakeRandomZ.x, shakeRandomZ.y));
                    }
                    floatObject.transform.GetChild(1).localPosition = Vector3.Lerp(floatObject.transform.GetChild(1).localPosition, shakeTargetPos, Time.deltaTime * shakeSpeed);
                    targetRotation = Quaternion.Euler(60 + Random.Range(shakeRandomX.x, shakeRandomX.y) * 2.5f, 0, Random.Range(shakeRandomZ.x, shakeRandomZ.y) * 1f);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * shakeSpeed);

                    //reactTimer  = Mathf.MoveTowards(reactTimer, reactTimeDefault, Time.fixedDeltaTime);
                    reactTimer += Time.deltaTime;
                    //UIManager.instance.fishingUI.SetActive(true);
                    //UIManager.instance.reelSlider.value = 1 - (reactTimer / reactTimeDefault);

                    if (Input.GetKeyDown(key) || Input.GetKeyDown(key2))
                    {
                        //reelCharger = 1 - (reactTimer / reactTimeDefault);
                        reelCharger = 0.5f;
                        fishingState = FishingState.Reeling;
                        fishingAudio.StartReeling();
                    }

                    if (reactTimer >= reactTimeDefault)
                    {
                        Reset();
                        StartCoroutine(fishingAudio.PlayEarlyPull());
                    }
                    break;

                case FishingState.Reeling:

                    //floatObject.transform.GetChild(1).localPosition = new Vector3(0, 0, 0);

                    if (shakeTimer < shakeFrequency)
                    {
                        shakeTimer += Time.deltaTime;
                    }
                    else
                    {
                        shakeTimer = 0;
                        shakeTargetPos = shakeDefaultPos + new Vector3(Random.Range(shakeRandomX.x, shakeRandomX.y), Random.Range(shakeRandomY.x, shakeRandomY.y), Random.Range(shakeRandomZ.x, shakeRandomZ.y));
                    }
                    floatObject.transform.GetChild(1).localPosition = Vector3.Lerp(floatObject.transform.GetChild(1).localPosition, shakeTargetPos, Time.deltaTime * shakeSpeed);

                    UIManager.instance.fishingUI.SetActive(true);
                    if (reelCharger <= 0)
                    {
                        Reset();
                        StartCoroutine(fishingAudio.PlayFishingFailure());
                    }
                    else
                    if (reelCharger < 1)
                    {
                        reelCharger = Mathf.MoveTowards(reelCharger, 0, Time.fixedDeltaTime * reelDecreaseCoefficient);
                        targetRotation = Quaternion.Euler(50 + Random.Range(shakeRandomX.x, shakeRandomX.y) * 2.5f, 0, Random.Range(shakeRandomZ.x, shakeRandomZ.y) * 1f);
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * shakeSpeed);

                        if (Input.GetKeyDown(key) || Input.GetKeyDown(key2))
                        {
                            reelCharger = Mathf.MoveTowards(reelCharger, 1, Time.fixedDeltaTime * reelIncreaseCoefficient);
                            targetRotation = Quaternion.Euler(40, 0, 0);
                            //transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10);
                            transform.localRotation = targetRotation;
                            StartCoroutine(fishingAudio.PlayPlayerPull());
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

                        if (fishingZone != null)
                        {
                            fishingZone.fishAmount--;
                            if (fishingZone.fishAmount <= 0)
                            {
                                //StartCoroutine(fishingZone.Delete());
                                Destroy(fishingZone.gameObject);
                                fishingZone = null;
                            }
                        }

                        Reset();
                        fishingAudio.PlayFishingSuccess();
                    }

                    break;

            }

            if (floatObject != null)
            {
                LineRenderer line = GetComponent<LineRenderer>();
                line.positionCount = 2;
                line.SetPosition(0, CastPos.position);
                line.SetPosition(1, floatObject.transform.GetChild(1).position);
            }
        }

    }

    public void Cast()
    {
        floatObject = Instantiate(floatPrefab, CastPos.position, Quaternion.identity, null);
        floatObject.GetComponent<Rigidbody>().AddForce(PlayerController.instance.tHead.forward * Mathf.Lerp(CastForce.x, CastForce.y, castCharger), ForceMode.Impulse);
        floatObject.GetComponent<SpringJoint>().connectedBody = GetComponent<Rigidbody>();

        LineRenderer line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, CastPos.position);
        line.SetPosition(1, floatObject.transform.GetChild(1).position);

        castCharger = 0;
        waitTimer = 0;
        waitTime = Random.Range(waitTimeInterval.x, waitTimeInterval.y);
        nibbleCount = Random.Range((int)nibbleCountInterval.x, (int)nibbleCountInterval.y);

        StartCoroutine(fishingAudio.PlayThrowLure());
        fishingAudio.StartFishingAmbiance();

    }

    public ItemData RandomFish(List<FishData> fishList)
    {
        float random = Random.Range(0f, 1f);
        float chance = 0;

        Debug.Log(random);

        foreach (FishData fish in fishList)
        {
            if (random <= chance)
            {
                return fish.fishItemData;
            }
            chance += fish.chance;
        }
        return fishList[fishList.Count - 1].fishItemData;
    }

    public void Reset()
    {
        fishingAudio.EndFishingAmbiance();
        //reset lure status
        lureTouchWater = false;

        UIManager.instance.fishingUI.SetActive(false);
        Destroy(floatObject);
        GetComponent<LineRenderer>().positionCount = 0;
        floatObject = null;
        fishHooked = null;
        fishingZone = null;
        waitTimer = 0;
        reactTimer = 0;
        reelCharger = 0;

        fishingState = FishingState.Idle;
        //waitTime = Random.Range(WaitTimeInterval.x, WaitTimeInterval.y);
    }

    public void OnDisable()
    {
        //Load();
        UIManager.instance.fishingUI.SetActive(false);
        Destroy(floatObject);
    }
}

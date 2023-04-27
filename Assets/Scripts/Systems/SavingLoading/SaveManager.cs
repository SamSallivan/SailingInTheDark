using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[System.Serializable]
public class SaveData
{
    public bool docked;
    public float currWatt;

    public int fuelLevel = 1;
    public int armorLevel = 1;
    public int lightLevel = 1;
    public int gearLevel = 1;

    public float[] boatPosition = new float[3];
    public float[] boatRotation = new float[3];

    public List<InventoryItem> inventoryItems;
    public List<SavedObjective> objectives;

    public bool[] collectedItems = new bool[0];
    public List<Trigger> oneTimeTriggers;

    public float[] playerPosition = new float[3];
    public float[] playerRotation = new float[3];

    public bool[] mapFragments = new bool[2];
    public bool[] doorOpen = new bool[3];

    public SaveData()
    {
    }
}

[System.Serializable]
public struct SavedObjective
{
    public GameObject prefabRef;
    public string text;
    public SavedObjective(GameObject prefabRef, string text)
    {
        this.prefabRef = prefabRef;
        this.text = text;
    }
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public AudioMixerGroup mixerOutput;
    public I_MapUpdate[] allFragments;
    public List<Trigger> eventsPending = new List<Trigger>();
    [Unity.Collections.ReadOnly] public I_InventoryItem[] allItems;
    [Unity.Collections.ReadOnly] public I_Door[] allDoors;

    public SaveData initialSaveData;
    public SaveData saveData = new SaveData();

    public bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        allFragments = FindObjectsOfType<I_MapUpdate>();
        allItems = FindObjectsOfType<I_InventoryItem>();
        allDoors = FindObjectsOfType<I_Door>();

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < allAudioSources.Length; i++)
            allAudioSources[i].outputAudioMixerGroup = mixerOutput;

        //SaveData readData = SaveLoader.Read();
        SaveData readData = ES3.Load<SaveData>("saveData", initialSaveData);
        if (readData != null)
        {
            saveData = readData;
            StartCoroutine(Load(readData));
        }
        else
        {
            StartCoroutine(Load(initialSaveData));
        }
    }

    public void Save()
    {
        saveData.currWatt = BoatController.instance.curWattHour;
        saveData.fuelLevel = BoatController.instance.fuelLevel;
        saveData.armorLevel = BoatController.instance.armorLevel;
        saveData.lightLevel = BoatController.instance.lightLevel;
        saveData.gearLevel = BoatController.instance.gearLevel;
        saveData.docked = BoatController.instance.anchor.activated;

        saveData.boatPosition[0] = BoatController.instance.transform.position.x;
        saveData.boatPosition[1] = BoatController.instance.transform.position.y;
        saveData.boatPosition[2] = BoatController.instance.transform.position.z;
        saveData.boatRotation[0] = BoatController.instance.transform.eulerAngles.x;
        saveData.boatRotation[1] = BoatController.instance.transform.eulerAngles.y;
        saveData.boatRotation[2] = BoatController.instance.transform.eulerAngles.z;

        saveData.playerPosition[0] = PlayerController.instance.transform.position.x;
        saveData.playerPosition[1] = PlayerController.instance.transform.position.y;
        saveData.playerPosition[2] = PlayerController.instance.transform.position.z;
        saveData.playerRotation[0] = PlayerController.instance.transform.eulerAngles.x;
        saveData.playerRotation[1] = PlayerController.instance.transform.eulerAngles.y;
        saveData.playerRotation[2] = PlayerController.instance.transform.eulerAngles.z;

        for (int i = 0; i < eventsPending.Count; i++)
            saveData.oneTimeTriggers.Add(eventsPending[i]);
        eventsPending.Clear();

        for (int i = 0; i < allDoors.Length; i++)
            saveData.doorOpen[i] = allDoors[i].activated;

        saveData.inventoryItems = InventoryManager.instance.inventoryItemList;
        saveData.objectives.Clear();
        foreach (Objective objective in ObjectiveManager.instance.ObjectiveList)
            saveData.objectives.Add(new SavedObjective(objective.prefabRef, objective.gameObject.GetComponent<TMP_Text>().text));

        saveData.collectedItems = new bool[allItems.Length];
        for (int i = 0; i<allItems.Length; i++)
            saveData.collectedItems[i] = allItems[i] == null;

        for (int i = 0; i < saveData.mapFragments.Length; i++)
            saveData.mapFragments[i] = (allFragments[i] == null);

        //SaveLoader.Write(saveData); 
        ES3.Save("saveData", saveData);
        isGameOver = false;
    }

    void Update()
    {   
        //testing
        if (Input.GetKeyDown(KeyCode.P))
            BoatController.instance.TakeDamage(100);
    }

    public IEnumerator Load(SaveData saveData)
    {
        for (int i = 0; i < saveData.oneTimeTriggers.Count; i++)
            saveData.oneTimeTriggers[i].triggeredOnce = true;
        for (int i = 0; i < eventsPending.Count; i++)
            eventsPending[i].triggeredOnce = false;
        this.eventsPending.Clear();

        for (int i = 0; i < saveData.doorOpen.Length; i++)
        {
            if (saveData.doorOpen[i])
            {
                allDoors[i].locked = false;
                StartCoroutine(allDoors[i].InteractionEvent());
                allDoors[i].UnTarget();
            }
        }

        while (InventoryManager.instance.inventoryItemList.Count > 0)
            InventoryManager.instance.RemoveItem(InventoryManager.instance.inventoryItemList[0]);
        foreach (InventoryItem item in saveData.inventoryItems)
            InventoryManager.instance.AddItem(item.data, item.status);

        while (ObjectiveManager.instance.ObjectiveList.Count > 0)
        {
            Destroy(ObjectiveManager.instance.ObjectiveList[0].gameObject);
            ObjectiveManager.instance.ObjectiveList.RemoveAt(0);
        }
        foreach (SavedObjective savedObjective in saveData.objectives)
        {
            if (savedObjective.prefabRef != null)
                ObjectiveManager.instance.AssignObejctive(savedObjective.prefabRef);
            else
                ObjectiveManager.instance.AssignObejctive(savedObjective.text);
        }
        for (int i = 0; i<saveData.mapFragments.Length; i++)
        {
            if (saveData.mapFragments[i])
            {
                StartCoroutine(allFragments[i].InteractionEvent());
                Destroy(allFragments[i]);
            }
        }
        for (int i = 0; i < saveData.collectedItems.Length; i++)
        {
            if (saveData.collectedItems[i] && allItems[i] != null)
            {
                Destroy(allItems[i].gameObject);
            }
        }

        BoatController.instance.fuelLevel = saveData.fuelLevel;
        BoatController.instance.maxWattHour = 100 + (saveData.fuelLevel-1)*25;
        BoatController.instance.curWattHour = Mathf.Clamp(saveData.currWatt, 0, BoatController.instance.maxWattHour);

        BoatController.instance.armorLevel = saveData.armorLevel;
        BoatController.instance.boatArmor = saveData.armorLevel * 0.5f;

        BoatController.instance.lightLevel = saveData.lightLevel;
        BoatController.instance.lightLeft.lightObject.GetComponent<Light>().intensity = 25 + (saveData.lightLevel * 25);
        BoatController.instance.lightRight.lightObject.GetComponent<Light>().intensity = 25 + (saveData.lightLevel * 25);

        BoatController.instance.gearLevel = saveData.gearLevel;
        BoatController.instance.helm.currentMaxGear = saveData.gearLevel + 1;

        BoatController.instance.ignoreConsumption = false;

        Rigidbody boatRB = BoatController.instance.GetComponent<Rigidbody>();
        boatRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        boatRB.interpolation = RigidbodyInterpolation.None;
        boatRB.isKinematic = true;

        BoatController.instance.transform.position = new Vector3(saveData.boatPosition[0], saveData.boatPosition[1], saveData.boatPosition[2]);
        BoatController.instance.transform.eulerAngles = new Vector3(saveData.boatRotation[0], saveData.boatRotation[1], saveData.boatRotation[2]);

        Rigidbody playerRB = PlayerController.instance.GetComponent<Rigidbody>();
        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            playerRB.interpolation = RigidbodyInterpolation.None;
            playerRB.isKinematic = true;
        }

        PlayerController.instance.transform.SetParent(null);
        PlayerController.instance.transform.position = new Vector3(saveData.playerPosition[0], saveData.playerPosition[1], saveData.playerPosition[2]);
        PlayerController.instance.transform.localEulerAngles = new Vector3(saveData.playerRotation[0], saveData.playerRotation[1], saveData.playerRotation[2]);

        PlayerController.instance.AttachToBoat(BoatController.instance.transform);

        yield return new WaitForSeconds(2);

        PlayerController.instance.DetachFromBoat();

        boatRB.constraints = RigidbodyConstraints.None;
        boatRB.interpolation = RigidbodyInterpolation.Interpolate;
        boatRB.isKinematic = false;
        BoatController.instance.helm.topView = false;
        
        BoatController.instance.anchor.Initialize(saveData.docked);

        if (playerRB)
        {
            playerRB.constraints = RigidbodyConstraints.FreezeRotation;
            playerRB.interpolation = RigidbodyInterpolation.Interpolate;
            playerRB.isKinematic = false;
        }

        //exit top view!!
        //PlayerController.instance.headPosition.Slide(0.75f + headHeightOffset1);
        PlayerController.instance.bob.GetComponentsInChildren<CinemachineVirtualCamera>(true)[0].gameObject.SetActive(true);
        PlayerController.instance.bob.GetComponentsInChildren<CinemachineVirtualCamera>(true)[1].gameObject.SetActive(false);

        PlayerController.instance.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);
        PlayerController.instance.tHead.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);
        PlayerController.instance.GetComponent<MouseLook>().Reset();

        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        InventoryManager.instance.CloseInventory();
        UpgradeManager.instance.CloseMenu();

        UIManager.instance.gameOverUI.SetActive(false);
        UIManager.instance.lockMouse.LockCursor(true);
        Time.timeScale = 1;
        isGameOver = false;
    }

    public void Die(string deadText)
    {
        if (!isGameOver)
        {
            isGameOver = true;
            UIManager.instance.gameOverUI.SetActive(true);
            UIManager.instance.lockMouse.LockCursor(false);

            PlayerController.instance.LockMovement(true);
            PlayerController.instance.LockCamera(true);

            BoatController.instance.GetComponent<Rigidbody>().isKinematic = true;
            BoatController.instance.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            BoatController.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            BoatController.instance.ShutDown();
            BoatController.instance.helm.activated = false;

            UIManager.instance.deathText.text = deadText;
            //MistCollision.instance.StopCoroutine(MistCollision.instance.DecreaseSlider());
        }
    }

    public void StartFromCheckPoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

[System.Serializable]
public class SaveData
{
    public float currWatt;
    public float maxWatt;
    public bool boatDocked;

    public Vector3 boatPosition;
    public Vector3 boatRotation;

    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public List<InventoryItem> inventoryItems;
    public List<Objective> objectives;

    public SaveData()
    {
    }
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public SaveData initialSaveData;
    private SaveData saveData = new SaveData();
    public bool isGameOver = false;
    public bool enableSave = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(Load(initialSaveData));

        /*if (!enableSave)
        {
            StartCoroutine(Load(initialSaveData));
        }
        else
        {
            SaveData readData = SaveLoader.Read();
            if (readData != null)
            {
                StartCoroutine(Load(readData));
                //SaveLoader.DeleteSaveData();
                //Save();
            }
            else
            {
                StartCoroutine(Load(initialSaveData));
            }
        }*/
    }

    public void Save()
    {
        saveData.currWatt = BoatController.instance.curWattHour;
        saveData.maxWatt = BoatController.instance.maxWattHour;
        saveData.boatDocked = BoatController.instance.anchor.activated;
        saveData.boatPosition = BoatController.instance.transform.position;
        saveData.boatRotation = BoatController.instance.transform.eulerAngles;

        saveData.playerPosition = PlayerController.instance.transform.position;
        saveData.playerRotation = PlayerController.instance.transform.eulerAngles;
        saveData.inventoryItems = InventoryManager.instance.inventoryItemList;
        saveData.objectives = ObjectiveManager.instance.ObjectiveList;

        SaveLoader.Write(saveData);
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
        //SaveData saveData = SaveLoader.Read();

        InventoryManager.instance.inventoryItemList.Clear();
        foreach (InventoryItem item in saveData.inventoryItems)
        {
            InventoryManager.instance.AddItem(item.data, item.status);
        }

        ObjectiveManager.instance.ObjectiveList.Clear();
        foreach (Objective objective in saveData.objectives)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        BoatController.instance.curWattHour = saveData.currWatt;
        BoatController.instance.maxWattHour = saveData.maxWatt;
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

        yield return new WaitForSeconds(1);

        boatRB.constraints = RigidbodyConstraints.None;
        boatRB.interpolation = RigidbodyInterpolation.Interpolate;
        boatRB.isKinematic = false;
        BoatController.instance.helm.topView = false;
        
        BoatController.instance.anchor.Initialize(saveData.boatDocked);

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
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);
        Time.timeScale = 1;
        isGameOver = false;
    }

    public void Die(string deadText)
    {
        if (!isGameOver)
        {
            isGameOver = true;
            UIManager.instance.gameOverUI.SetActive(true);
            UIManager.instance.GetComponent<LockMouse>().LockCursor(false);

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
    
}

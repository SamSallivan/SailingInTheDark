using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public bool UseSave;

    private Transform playerTransform;
    [HideInInspector] public Vector3 playerPos;
    [HideInInspector] public Vector3 playerRot;
    [HideInInspector] public List<InventoryItem> initialInventory;
    [HideInInspector] public List<Objective> initialObjective;
    [HideInInspector] public List<InventoryItem> playerInventory = new List<InventoryItem>();

    [HideInInspector] public Transform boatTransform;
    [HideInInspector] public Vector3 boatPos;
    [HideInInspector] public Vector3 boatRot;
    [HideInInspector] public bool boatDocked;
    I_Anchor anchor;
    //public TMP_Text deathText;
    [HideInInspector] public bool alive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            anchor = GameObject.Find("AnchorSwitch").GetComponent<I_Anchor>();
        }
        //deathText = GameObject.Find("Death Text").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        foreach (InventoryItem item in initialInventory)
        {
            InventoryManager.instance.AddItem(item.data, item.status);
        }

        foreach (Objective objective in initialObjective)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        Debug.Log($"find your save file here: {Application.persistentDataPath}");

        if (UseSave)
        {
            DataFile data = SaveLoader.LoadData();
            if (data == null)
                Save();
            else
                LoadCheckPoint();
        }
        else
        {
            SaveLoader.DeleteSaveData();
            Save();
        }
    }

    public void Save()
    {
        this.boatDocked = anchor.activated;
        playerPos = PlayerController.instance.transform.position;
        playerRot = PlayerController.instance.transform.eulerAngles;

        boatPos = BoatController.instance.transform.position;
        boatRot = BoatController.instance.transform.eulerAngles;

        SaveLoader.SaveData(this, BoatController.instance.curWattHour, BoatController.instance.maxWattHour);
        alive = true;
    }

    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.P))
            BoatController.instance.TakeDamage(100);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Save();
    }

    public void LoadCheckPoint()
    {
        InventoryManager.instance.inventoryItemList.Clear();
        foreach (InventoryItem item in playerInventory)
        {
            InventoryManager.instance.AddItem(item.data, item.status);
        }

        ObjectiveManager.instance.ObjectiveList.Clear();
        foreach (Objective objective in initialObjective)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        StartCoroutine(Reset());
    }

    public IEnumerator Reset()
    {
        DataFile data = SaveLoader.LoadData();

        BoatController.instance.curWattHour = data.currWatt;
        BoatController.instance.maxWattHour = data.maxWatt;
        BoatController.instance.ignoreConsumption = false;

        Rigidbody boatRB = BoatController.instance.GetComponent<Rigidbody>();
        boatRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        boatRB.interpolation = RigidbodyInterpolation.None;
        boatRB.isKinematic = true;

        BoatController.instance.transform.position = new Vector3(data.boatPosition[0], data.boatPosition[1], data.boatPosition[2]);
        BoatController.instance.transform.eulerAngles = new Vector3(data.boatRotation[0], data.boatRotation[1], data.boatRotation[2]);

        Rigidbody playerRB = PlayerController.instance.GetComponent<Rigidbody>();
        playerRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        playerRB.interpolation = RigidbodyInterpolation.None;
        playerRB.isKinematic = true;

        PlayerController.instance.transform.SetParent(null);
        PlayerController.instance.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        PlayerController.instance.transform.eulerAngles = new Vector3(data.playerRotation[0], data.playerRotation[1], data.playerRotation[2]);

        if (anchor.activated != data.boatDocked)
            anchor.AnchorSwitch();

        yield return new WaitForSeconds(1);

        boatRB.constraints = RigidbodyConstraints.None;
        boatRB.interpolation = RigidbodyInterpolation.Interpolate;
        boatRB.isKinematic = false;

        playerRB.constraints = RigidbodyConstraints.None;
        playerRB.interpolation = RigidbodyInterpolation.Interpolate;
        playerRB.isKinematic = false;

        //exit top view!!
        BoatController.instance.helm.topView = false;
        //PlayerController.instance.headPosition.Slide(0.75f + headHeightOffset1);
        PlayerController.instance.bob.GetComponentsInChildren<CinemachineVirtualCamera>(true)[0].gameObject.SetActive(true);
        PlayerController.instance.bob.GetComponentsInChildren<CinemachineVirtualCamera>(true)[1].gameObject.SetActive(false);

        PlayerController.instance.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);
        PlayerController.instance.tHead.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);

        PlayerController.instance.transform.localRotation = Quaternion.identity;
        PlayerController.instance.GetComponent<MouseLook>().Reset();

        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        InventoryManager.instance.CloseInventory();
        UpgradeManager.instance.CloseMenu();

        UIManager.instance.gameOverUI.SetActive(false);
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);
        Time.timeScale = 1;
        alive = true;
    }

    public void Die(string deadText)
    {
        if (alive)
        {
            alive = false;
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

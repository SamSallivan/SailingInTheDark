using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private Transform playerTransform;
    [HideInInspector] public Vector3 playerPos;
    [HideInInspector] public Quaternion playerRot;
    [HideInInspector] public List<InventoryItem> initialInventory;
    [HideInInspector] public List<Objective> initialObjective;
    [HideInInspector] public List<InventoryItem> playerInventory = new List<InventoryItem>();

    [HideInInspector] public Transform boatTransform;
    [HideInInspector] public Vector3 boatPos;
    [HideInInspector] public Quaternion boatRot;
    [HideInInspector] public float boatWattHour;
    public bool boatDocked;
    public I_Anchor anchor;
    //public TMP_Text deathText;
    public bool alive = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        Save();
    }

    public void Save()
    {
        this.boatDocked = anchor.activated;
        playerTransform = PlayerController.instance.transform;
        playerPos = playerTransform.position;
        playerRot = playerTransform.rotation;

        boatTransform = BoatController.instance.transform;
        boatPos = boatTransform.position;
        boatRot = boatTransform.rotation;
        boatWattHour = BoatController.instance.curWattHour;
    }

    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.P))
            BoatController.instance.TakeDamage(100);
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
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);

        BoatController.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        BoatController.instance.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
        BoatController.instance.GetComponent<Rigidbody>().isKinematic = true;

        BoatController.instance.transform.position = boatPos;
        BoatController.instance.transform.rotation = boatRot;

        BoatController.instance.curWattHour = boatWattHour;
        BoatController.instance.ignoreConsumption = false;

        if (anchor.activated != this.boatDocked)
            anchor.AnchorSwitch();

        yield return new WaitForSeconds(1);

        BoatController.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        BoatController.instance.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        BoatController.instance.GetComponent<Rigidbody>().isKinematic = false;
        
        //exit top view!!
        BoatController.instance.helm.topView = false;
        //PlayerController.instance.headPosition.Slide(0.75f + headHeightOffset1);
        PlayerController.instance.bob.GetComponentsInChildren<CinemachineVirtualCamera>(true)[0].gameObject.SetActive(true);
        PlayerController.instance.bob.GetComponentsInChildren<CinemachineVirtualCamera>(true)[1].gameObject.SetActive(false);

        PlayerController.instance.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);
        PlayerController.instance.tHead.GetComponent<MouseLook>().SetClamp(-360, 360, -85, 85);

        PlayerController.instance.transform.localRotation = Quaternion.identity;
        PlayerController.instance.GetComponent<MouseLook>().Reset();

        PlayerController.instance.transform.position = playerPos;
        PlayerController.instance.transform.rotation = playerRot;
        PlayerController.instance.transform.SetParent(null);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public TMP_Text deathText;
    public bool alive = true;

    private Transform playerTransform;
    private Vector3 playerPos;
    private Quaternion playerRot;
    public List<InventoryItem> initialInventory;
    public List<Objective> initialObjective;
    private List<InventoryItem> playerInventory;

    private Transform boatTransform;
    private Vector3 boatPos;
    private Quaternion boatRot;
    private float boatWattHour;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        deathText = GameObject.Find("Death Text").GetComponent<TMP_Text>();
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

    public void Die(string dead)
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

            deathText.text = dead;
            MistCollision.instance.StopCoroutine(MistCollision.instance.DecreaseSlider());
        }
    }

    public void LoadCheckPoint()
    {
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);
        UIManager.instance.gameOverUI.SetActive(false);

        PlayerController.instance.transform.SetParent(null);
        PlayerController.instance.transform.position = playerPos;
        PlayerController.instance.transform.rotation = playerRot;
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        BoatController.instance.transform.position = boatPos;
        BoatController.instance.transform.rotation = boatRot;

        BoatController.instance.curWattHour = this.boatWattHour;
        BoatController.instance.ignoreConsumption = false;

        Rigidbody rb = BoatController.instance.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        alive = true;

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
    }

}

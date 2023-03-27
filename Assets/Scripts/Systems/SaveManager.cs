using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private Transform playerTransform;
    private Vector3 playerPos;
    private Quaternion playerRot;
    public List<InventoryItem> initialInventory;
    public List<Objective> initialObjective;
    private List<InventoryItem> playerInventory = new List<InventoryItem>();

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

    public IEnumerator Reset() {
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);

        BoatController.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        BoatController.instance.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
        BoatController.instance.GetComponent<Rigidbody>().isKinematic = true;

        BoatController.instance.transform.position = boatPos;
        BoatController.instance.transform.rotation = boatRot;

        BoatController.instance.curWattHour = boatWattHour;
        BoatController.instance.ignoreConsumption = false;

        yield return new WaitForSeconds(1);

        BoatController.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        BoatController.instance.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        BoatController.instance.GetComponent<Rigidbody>().isKinematic = false;

        PlayerController.instance.transform.position = playerPos;
        PlayerController.instance.transform.rotation = playerRot;
        PlayerController.instance.transform.SetParent(null);
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        UIManager.instance.gameOverUI.SetActive(false);
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);
        Time.timeScale = 1;

    }
    
}

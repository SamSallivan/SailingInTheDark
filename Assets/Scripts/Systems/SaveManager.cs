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
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);

        PlayerController.instance.transform.SetParent(null);
        PlayerController.instance.transform.position = playerPos;
        PlayerController.instance.transform.rotation = playerRot;
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        BoatController.instance.transform.position = boatPos;
        BoatController.instance.transform.rotation = boatRot;
        BoatController.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        BoatController.instance.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;

        BoatController.instance.curWattHour = boatWattHour;
        BoatController.instance.ignoreConsumption = false;
        
        UIManager.instance.gameOverUI.SetActive(false);
    }
    
}

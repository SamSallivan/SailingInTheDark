using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public BoatController boatController;
    public Rigidbody boatRB;
    public PlayerController playerController;
    public RectTransform image;

    public Vector3 boatStart;
    public Vector3 playerStart;
    public Vector3 boatRot;
    public Vector3 playerRot;

    public bool gameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        image.anchoredPosition = new Vector3(0, 2000, 0);
        boatStart = boatController.transform.position;
        playerStart = playerController.transform.position;

        boatRB.interpolation = RigidbodyInterpolation.None;
        boatRot = boatController.transform.eulerAngles;
        playerRot = playerController.transform.eulerAngles;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            boatController.TakeDamage(100);
    }

    public IEnumerator TriggerDeath()
    {
        UIManager.instance.GetComponent<LockMouse>().LockCursor(false);
        gameOver = true;

        int position = 2000;
        while (position > 0)
        {
            position-=30;
            image.anchoredPosition = new Vector3(0, position, 0);
            yield return new WaitForSeconds(0.01f);
        }

        boatController.batteryInUse = false;
        playerController.enableMovement = false;
        boatRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
    }

    public void ResetScene()
    {
        StartCoroutine(SceneReset());
    }

    IEnumerator SceneReset()
    { 
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);
        gameOver = false;

        playerController.transform.SetParent(null);
        playerController.transform.eulerAngles = playerRot;
        playerController.transform.position = playerStart;
        playerController.enableMovement = true;

        playerController.transform.SetParent(null);
        boatController.transform.position = boatStart;
        boatController.transform.eulerAngles = boatRot;
        boatRB.constraints = RigidbodyConstraints.None;

        boatController.curWattHour = boatController.maxWattHour;
        boatController.batteryInUse = true;

        int position = 0;
        while (position < 2000)
        {
            position += 30;
            image.anchoredPosition = new Vector3(0, position, 0);
            yield return new WaitForSeconds(0.01f);
        }

    }
}

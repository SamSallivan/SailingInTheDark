using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public PlayerController playerMovement;
    public BoatController boatMovement;
    public Rigidbody boatRB;
    public GameObject GameCameras;
    //public List<>

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public IEnumerator PlayCutscene()
    {
        playerMovement.enableMovement = false;
        boatMovement.batteryInUse = false;

        yield return null;
        /*
        playerMovement.enableMovement = true;
        boatMovement.batteryInUse = true;
        */
    }
}

using NWH.DWP2.ShipController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public static BoatController instance;
    public AdvancedShipController boat;
    public bool activated;
    public Transform playerTargetPos;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            PlayerController.instance.transform.position = Vector3.Lerp(PlayerController.instance.transform.position, playerTargetPos.position, Time.deltaTime*5);
        }
        
    }
}

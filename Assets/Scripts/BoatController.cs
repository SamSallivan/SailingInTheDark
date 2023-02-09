using NWH.DWP2.ShipController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public static BoatController instance;
    public AdvancedShipController boat;
    void Awake()
    {
        instance = this;
    }
}

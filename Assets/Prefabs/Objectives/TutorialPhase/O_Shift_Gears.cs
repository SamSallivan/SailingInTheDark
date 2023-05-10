using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Shift_Gears : Objective
{
    public override bool CheckFinished()
    {
        Debug.Log(BoatController.instance.helm.currentGear);
        if (BoatController.instance.helm.currentGear >= 2)
        {
            return true;
        }
        return false;
    }
}

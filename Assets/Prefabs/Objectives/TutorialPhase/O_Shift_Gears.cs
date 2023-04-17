using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Shift_Gears : Objective
{
    public override bool CheckFinished()
    {
        if (Mathf.Round(BoatController.instance.boat.Speed) >= 4)
        {
            return true;
        }
        return false;
    }
}

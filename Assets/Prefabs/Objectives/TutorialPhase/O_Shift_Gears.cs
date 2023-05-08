using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Shift_Gears : Objective
{
    public override bool CheckFinished()
    {
        if (BoatController.instance.gearLevel == 2)
        {
            return true;
        }
        return false;
    }
}

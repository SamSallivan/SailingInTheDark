using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Lights : Objective
{
    public override bool CheckFinished()
    {
        if (BoatController.instance.lightLeft.activated && BoatController.instance.lightRight.activated)
        {
            return true;
        }
        return false;
    }
}

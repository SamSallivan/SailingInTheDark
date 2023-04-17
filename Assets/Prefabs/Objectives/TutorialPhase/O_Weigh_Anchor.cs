using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Weigh_Anchor : Objective
{
    public override bool CheckFinished()
    {
        if (!BoatController.instance.anchor.activated)
        {
            return true;
        }
        return false;
    }
}

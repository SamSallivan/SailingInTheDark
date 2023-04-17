using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class O_Dock_Boat : Objective
{

    public override bool CheckFinished()
    {
        if (BoatController.instance.anchor.activated && BoatController.instance.anchor.dockable)
        {
            return true;
        }
        return false;
    }

}

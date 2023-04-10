using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class O_Flash_Light : Objective
{

    public override bool CheckFinished()
    {
        if (InventoryManager.instance.equippedItemLeft != null && InventoryManager.instance.equippedItemLeft.data != null)
        {
            if (InventoryManager.instance.equippedItemLeft.data.title == "Flash Light")
                return true;
        }
        return false;
    }

}

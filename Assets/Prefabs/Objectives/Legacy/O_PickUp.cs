using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_PickUp : Objective
{
    public ItemData desiredItem;
    public void OnEnable()
    {
        InventoryManager.OnPickUp += OnPickUp;
    }

    public void OnDisable()
    {
        InventoryManager.OnPickUp -= OnPickUp;
    }

    public void OnPickUp(ItemData itemData)
    {
        if(itemData == desiredItem){
            Finish();
        }
    }
}

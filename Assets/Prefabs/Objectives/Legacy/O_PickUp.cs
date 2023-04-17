using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_PickUp : Objective
{
    //comment for github
    public List<ItemData> desiredItems;
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
        if (ItemIsRequired(itemData))
        {
            if (desiredItems.Count == 0)
            {
                Finish();
            }
        }
    }

    bool ItemIsRequired(ItemData itemData)
    {
        if (desiredItems.Contains(itemData))
        {
            desiredItems.Remove(itemData);
            return true;
        }
        return false;
    }
}

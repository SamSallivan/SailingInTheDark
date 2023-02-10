using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_InventoryItem : Interactable
{
    
    public override IEnumerator InteractionEvent()
    {
        if(itemData != null)
        {
            InventoryManager.instance.AddItem(itemData, itemStatus);
            gameObject.SetActive(false);
        }
        yield return null; 
    }
}

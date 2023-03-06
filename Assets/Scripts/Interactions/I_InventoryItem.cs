using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_InventoryItem : Interactable
{
    public enum ExamineType
    {
        None,
        Paper,
        Object,
        Upgrade
    }


    //[ConditionalField(nameof(examineType), false, ExamineType.Object, ExamineType.Paper)]
    //public bool canPickUp;

    public override IEnumerator InteractionEvent()
    {
        
        if(itemData != null)
        {
            InventoryItem newItem = InventoryManager.instance.AddItem(itemData, itemStatus);
            Destroy(transform.gameObject);
            
            if (equipOnPickUp && itemData.isEquippable)
            {
                InventoryManager.instance.EquipItem(newItem);
            }

            if (openInventoryOnPickUp)
            {
                InventoryManager.instance.OpenInventory();
                InventoryManager.instance.selectedPosition = InventoryManager.instance.GetGridPosition(newItem.slot.GetIndex());
            }

            UnTarget();
        }
        yield return null; 
    }

    public IEnumerator PickUp()
    {
        yield return null;
    }
    public IEnumerator Examine()
    {
        yield return null;
    }



}

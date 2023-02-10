using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



[System.Serializable]
public struct ItemStatus
{
    public int amount;
}

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    public TMP_Text descriptiontext;

}

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public ItemStatus status;
    public InventorySlot slot;

    public InventoryItem()
    {
    }
    public InventoryItem(ItemData data, ItemStatus status)
    {
        this.data = data;
        this.status = status;
    }
    public InventoryItem(ItemData data, ItemStatus status, InventorySlot slot)
    {
        this.data = data;
        this.status = status;
        this.slot = slot;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventoryItem> inventoryItemList;

    public GameObject slotPrefab;
    public GameObject inventoryUI;


    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //inventoryUI.SetActive(Input.GetKeyDown(KeyCode.Tab));
    }

    public void AddItem(ItemData itemData, ItemStatus itemStatus)
    {
        //if exists in inventory
        //inventoryitemlist[i].itemstatus.amount++
        //elseif new to inventory
            //InventorySlot slot = Instantiate(slotPrefab, Slots[i].transform); ;
            inventoryItemList.Add(new InventoryItem(itemData, itemStatus));
    }
}

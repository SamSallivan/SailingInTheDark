using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject slot;

    public InventoryItem()
    {
    }
    public InventoryItem(ItemData data, ItemStatus status, GameObject slot)
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
        inventoryUI.SetActive(Input.GetKey(KeyCode.Tab));

        if (Input.GetKeyDown(KeyCode.P) && inventoryItemList.Count > 0)
            DropItem(inventoryItemList[0]);
    }

    public void AddItem(ItemData itemData, ItemStatus itemStatus)
    {
        //if exists in inventory
        //inventoryitemlist[i].itemstatus.amount++
        //elseif new to inventory
        //InventorySlot slot = Instantiate(slotPrefab, Slots[i].transform); ;

        GameObject newSlot = Instantiate(slotPrefab, inventoryUI.transform);
        newSlot.GetComponentInChildren<Image>().sprite = itemData.sprite;
        inventoryItemList.Add(new InventoryItem(itemData, itemStatus, newSlot));
    }

    public void DropItem(InventoryItem removeMe)
    {
        inventoryItemList.Remove(removeMe);
        Destroy(removeMe.slot.gameObject);
        
        GameObject droppdeObject = Instantiate(removeMe.data.dropObject);
        droppdeObject.GetComponent<I_InventoryItem>().itemStatus = removeMe.status;
        droppdeObject.transform.position = PlayerController.instance.tHead.transform.position + Vector3.forward;
    }

}

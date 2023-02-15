using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public ItemStatus status;
    public InventorySlot slot;

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
    public bool activated;

    public InventorySlot slotPrefab;

    public List<InventoryItem> inventoryItemList;
    public int selectedIndex;
    public int2 selectedPosition;
    public InventoryItem selectedItem;
    public InventoryItem equippedItem;
    public Transform holdHeldObject;

    public int slotPerRow = 8;
    public int slotPerColumn = 4;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activated = !activated;
            UIManager.instance.inventoryUI.SetActive(activated);
            PlayerController.instance.enableMovement = !activated;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (inventoryItemList.Count > 0)
            {
                Destroy(holdHeldObject.GetChild(0).gameObject);
                GameObject newObject = Instantiate(inventoryItemList[0].data.dropObject, holdHeldObject);
                newObject.name = inventoryItemList[0].data.dropObject.name;
                newObject.transform.localPosition = new Vector3(0, 0, 0);
                newObject.transform.localEulerAngles = new Vector3(0, 0, 0);

                Destroy(newObject.transform.GetChild(0).gameObject);
                Destroy(newObject.GetComponent<Collider>());
                Destroy(newObject.GetComponent<Rigidbody>());
            }
        }

        if (activated)
        {
            selectedPosition.x += Input.GetKeyDown(KeyCode.D) ? 1 : 0;
            selectedPosition.x -= Input.GetKeyDown(KeyCode.A) ? 1 : 0;
            if(selectedPosition.x < 0)
            {
                selectedPosition.x = slotPerRow-1;
            }
            if (selectedPosition.x > slotPerRow-1)
            {
                selectedPosition.x = 0;
            }
            selectedPosition.y += Input.GetKeyDown(KeyCode.S) ? 1 : 0;
            selectedPosition.y -= Input.GetKeyDown(KeyCode.W) ? 1 : 0;
            if (selectedPosition.y < 0)
            {
                selectedPosition.y = slotPerColumn - 1;
            }
            if (selectedPosition.y > slotPerColumn - 1)
            {
                selectedPosition.y = 0;
            }
            selectedIndex = GetGridIndex(selectedPosition);

            for (int i = 0; i < UIManager.instance.inventoryBackGrid.transform.childCount; i++)
            {
                UIManager.instance.inventoryBackGrid.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            UIManager.instance.inventoryBackGrid.transform.GetChild(selectedIndex).GetComponent<Image>().color = Color.red;

            if (UIManager.instance.inventoryItemGrid.transform.childCount >= selectedIndex)
            {
                selectedItem = UIManager.instance.inventoryItemGrid.transform.GetChild(selectedIndex).GetComponent<InventorySlot>().inventoryItem;
                if (Input.GetKeyDown(KeyCode.P) && inventoryItemList.Count > 0)
                    DropItem(selectedItem);
            }


        }

    }

    public void AddItem(ItemData itemData, ItemStatus itemStatus)
    {
        //if exists in inventory and stackable
            //if max amount not reached
                //inventoryitemlist[i].itemstatus.amount++
            //else

        InventorySlot newSlot = Instantiate(slotPrefab, UIManager.instance.inventoryItemGrid.transform);
        InventoryItem newItem = new InventoryItem(itemData, itemStatus, newSlot);
        inventoryItemList.Add(newItem);
        newSlot.inventoryItem = newItem;
        newSlot.image.sprite = itemData.sprite;
        newSlot.name.text = itemData.name;
        newSlot.amount.text = $"{itemStatus.amount}";
    }

    public void DropItem(InventoryItem inventoryItem)
    {
        if (inventoryItem.data.itemToggles.isStackable)
        {
            if (inventoryItem.status.amount > 1)
            {
                inventoryItem.status.amount--;
                inventoryItem.slot.amount.text = "" + inventoryItem.status.amount;
                GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + Vector3.forward, PlayerController.instance.tHead.transform.rotation);
            }
            else
            {
                inventoryItemList.Remove(inventoryItem);
                Destroy(inventoryItem.slot.gameObject);
                GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + Vector3.forward, PlayerController.instance.tHead.transform.rotation);
            }
        }
        else
        {
            inventoryItemList.Remove(inventoryItem);
            Destroy(inventoryItem.slot.gameObject);
            GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + Vector3.forward, PlayerController.instance.tHead.transform.rotation);
            droppdeObject.GetComponentInChildren<I_InventoryItem>().itemStatus = inventoryItem.status;
        }
    }

    public int2 GetGridPosition(int index)
    {
        return(new int2(index/ slotPerRow + 1, index% slotPerRow));
    }
    public int GetGridIndex(int2 position)
    {
        return ((position.y) * slotPerRow + position.x);
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering;
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

    public int slotPerRow = 8;
    public int slotPerColumn = 4;

    void Awake()
    {
        instance = this;
        equippedItem = null;
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!activated)
            {
                if (!UIManager.instance.examineUI.activeInHierarchy)
                {
                    OpenInventory();
                }
            }
            else
            {
                CloseInventory();
            }
        }


        if (activated)
        {
            SelectItem();

            if (inventoryItemList.Count - 1 >= selectedIndex)
            {
                selectedItem = UIManager.instance.inventoryItemGrid.transform.GetChild(selectedIndex).GetComponent<InventorySlot>().inventoryItem;

                UpdateDetailObject();
                RotateDetailObject();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    EquipItem(selectedItem);
                }
                if (Input.GetKeyDown(KeyCode.Q) )
                {
                    DropItem(selectedItem);
                }
            }
            else
            {
                selectedItem = null;
                if (UIManager.instance.detailObjectPivot.childCount > 0)
                {
                    Destroy(UIManager.instance.detailObjectPivot.GetChild(0).gameObject);
                }
                UIManager.instance.detailName.text = "";
                UIManager.instance.detailDescription.text = "";
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseInventory();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q) && equippedItem != null)
            {
                DropItem(equippedItem);
            }
        }

    }

    public void OpenInventory()
    {
        activated = true;
        //Time.timeScale = activated ? 0.0f : 1.0f;
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);

        UIManager.instance.inventoryUI.SetActive(true);
        UIManager.instance.gameplayUI.SetActive(false);

        PlayerController.instance.tHead.GetComponent<LockMouse>().LockCursor(false);

        if (equippedItem != null)
        {
            selectedPosition = GetGridPosition(equippedItem.slot.GetIndex());
        }
        else
        {
            selectedPosition = 0;
        }

        //play the fade in effect
        UIManager.instance.inventoryAnimation.Play("Basic Fade-in");
    }
    public void CloseInventory()
    {
        activated = false;
        //Time.timeScale = activated ? 0.0f : 1.0f;
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        //UIManager.instance.inventoryUI.SetActive(false);
        UIManager.instance.gameplayUI.SetActive(true);

        PlayerController.instance.tHead.GetComponent<LockMouse>().LockCursor(true);

        //play the fade out effect
        UIManager.instance.inventoryAnimation.Play("Basic Fade-out");
    }

    public void SelectItem()
    {
        selectedPosition.x += Input.GetKeyDown(KeyCode.D) ? 1 : 0;
        selectedPosition.x -= Input.GetKeyDown(KeyCode.A) ? 1 : 0;
        if (selectedPosition.x < 0)
        {
            selectedPosition.x = slotPerRow - 1;
        }
        if (selectedPosition.x > slotPerRow - 1)
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
            UIManager.instance.inventoryBackGrid.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
        }
        UIManager.instance.inventoryBackGrid.transform.GetChild(selectedIndex).GetComponent<Image>().color = Color.white;
    }

    public InventoryItem AddItem(ItemData itemData, ItemStatus itemStatus)
    {
        //if exists in inventory and stackable
            //if max amount not reached
                //inventoryitemlist[i].itemstatus.amount++
            //else
        
        foreach(InventoryItem item in inventoryItemList)
        {
            if (item.data == itemData)
            {
                if (itemData.isStackable && item.status.amount < itemData.maxStackAmount)
                {
                    item.status.amount++;
                    item.slot.amount.text = "" + item.status.amount;
                    return item;
                }
            }
        }

        InventorySlot newSlot = Instantiate(slotPrefab, UIManager.instance.inventoryItemGrid.transform);
        InventoryItem newItem = new InventoryItem(itemData, itemStatus, newSlot);
        inventoryItemList.Add(newItem);
        newSlot.inventoryItem = newItem;
        newSlot.image.sprite = itemData.sprite;
        newSlot.name.text = itemData.name;
        newSlot.amount.text = $"{itemStatus.amount}";

        return newItem;
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        if (inventoryItem.data.isStackable)
        {
            if (inventoryItem.status.amount > 1)
            {
                inventoryItem.status.amount--;
                inventoryItem.slot.amount.text = "" + inventoryItem.status.amount;
                
            }
            else
            {
                inventoryItemList.Remove(inventoryItem);
                Destroy(inventoryItem.slot.gameObject);
                if (equippedItem == inventoryItem)
                {
                    UnequipItem();
                }
            }
        }
        else
        {
            inventoryItemList.Remove(inventoryItem);
            Destroy(inventoryItem.slot.gameObject);
            if (equippedItem == inventoryItem)
            {
                UnequipItem();
            }
        }

        //loop through items and organize inventory.
        //perhaps a separate functions for this?
    }

    public void DropItem(InventoryItem inventoryItem)
    {
        if (inventoryItem.data.isStackable)
        {
            if (inventoryItem.status.amount > 1)
            {
                inventoryItem.status.amount--;
                inventoryItem.slot.amount.text = "" + inventoryItem.status.amount;
                GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + PlayerController.instance.tHead.transform.forward, PlayerController.instance.tHead.transform.rotation);

            }
            else
            {
                inventoryItemList.Remove(inventoryItem);
                Destroy(inventoryItem.slot.gameObject);
                GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + PlayerController.instance.tHead.transform.forward, PlayerController.instance.tHead.transform.rotation);
                if (equippedItem == inventoryItem)
                {
                    UnequipItem();
                }
            }
        }
        else
        {
            inventoryItemList.Remove(inventoryItem);
            Destroy(inventoryItem.slot.gameObject);
            GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + PlayerController.instance.tHead.transform.forward, PlayerController.instance.tHead.transform.rotation);
            droppdeObject.GetComponentInChildren<I_InventoryItem>().itemStatus = inventoryItem.status;
            if (equippedItem == inventoryItem)
            {
                UnequipItem();
            }
        }

        //loop through items and organize inventory.
        //perhaps a separate functions for this?
    }

    public void EquipItem(InventoryItem inventoryItem)
    {
        if (equippedItem == inventoryItem)
        {
            UnequipItem();
        }
        else
        {
            if (PlayerController.instance.equippedTransform.childCount > 0)
            {
                Destroy(PlayerController.instance.equippedTransform.GetChild(0).gameObject);
            }
            equippedItem = inventoryItem;
            GameObject newObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.equippedTransform);
            newObject.name = inventoryItem.data.dropObject.name + " Equipped";
            newObject.transform.localPosition = new Vector3(0, 0, 0);
           // newObject.transform.localEulerAngles = new Vector3(0, 0, 0);

            //Destroy(newObject.transform.GetChild(0).gameObject);
            Destroy(newObject.transform.GetComponentInChildren<Interactable>());
            foreach (Collider collider in newObject.GetComponents<Collider>())
            {
                Destroy(collider);
            }
            Destroy(newObject.GetComponent<Rigidbody>());
        }
    }

    public void UnequipItem()
    {
        equippedItem = null;
        if (PlayerController.instance.equippedTransform.childCount > 0)
        {
            Destroy(PlayerController.instance.equippedTransform.GetChild(0).gameObject);
        }
    }
    
    private void UpdateDetailObject()
    {
        if (UIManager.instance.detailName.text != selectedItem.data.name)
        {
            UIManager.instance.detailName.text = selectedItem.data.name;
            UIManager.instance.detailDescription.text = selectedItem.data.description;

            if (UIManager.instance.detailObjectPivot.childCount > 0)
            {
                Destroy(UIManager.instance.detailObjectPivot.GetChild(0).gameObject);
            }

            GameObject detailGameObject = Instantiate(selectedItem.data.dropObject, UIManager.instance.detailObjectPivot);
            detailGameObject.transform.localScale *= 1200;
            detailGameObject.transform.localRotation = selectedItem.data.examineRotation;
            foreach (Transform child in UIManager.instance.detailObjectPivot.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 6;
            }

            Destroy(detailGameObject.transform.GetComponentInChildren<Interactable>());
            Destroy(detailGameObject.GetComponent<Rigidbody>());
            foreach (Collider collider in detailGameObject.GetComponents<Collider>())
            {
                Destroy(collider);
            }
        }
    }

    public void RotateDetailObject()
    {
        Vector2 lookVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 rotateValue = new Vector2();

        if(Input.GetMouseButton(0)){
            rotateValue.x = -(lookVector.x * 2.5f);
            rotateValue.y = lookVector.y * 2.5f;
            UIManager.instance.detailObjectPivot.GetChild(0).transform.Rotate(PlayerController.instance.tHead.transform.up, rotateValue.x, Space.World);
            UIManager.instance.detailObjectPivot.GetChild(0).transform.Rotate(PlayerController.instance.tHead.transform.right, rotateValue.y, Space.World);
        }
        else
        {
            //Transform target = new RectTransform();
            //target.localRotation = selectedItem.data.examineRotation;
            //target.Rotate(PlayerController.instance.tHead.transform.up, 1, Space.World);

            UIManager.instance.detailObjectPivot.GetChild(0).transform.Rotate(PlayerController.instance.tHead.transform.up, 1, Space.World);
            //UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation = Quaternion.Lerp(UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation, new Quaternion(selectedItem.data.examineRotation.x, UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation.y, selectedItem.data.examineRotation.z, selectedItem.data.examineRotation.w), Time.fixedDeltaTime * 5);
            //UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation = Quaternion.Lerp(UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation, selectedItem.data.examineRotation, Time.fixedDeltaTime);
            //UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation = selectedItem.data.examineRotation;
        }
    }

    public int2 GetGridPosition(int index)
    {
        return(new int2(index% slotPerRow, index / slotPerRow));
    }
    public int GetGridIndex(int2 position)
    {
        return ((position.y) * slotPerRow + position.x);
    }

}

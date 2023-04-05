using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public bool activated;

    public InventorySlot slotPrefab;

    public List<InventoryItem> inventoryItemList;
    public int selectedIndex;
    public int2 selectedPosition;
    public InventoryItem selectedItem;
    public InventoryItem equippedItem = null;
    public InventoryItem equippedItemLeft = null;
    public InventoryItem equippedItemRight = null;
    public InventoryItem equippedItemCenter = null;

    public int slotPerRow = 8;
    public int slotPerColumn = 4;

    private bool detailRotationFix;
    public bool detailObjectDrag;
    public float inputDelay;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputDelay < 1)
        {
            inputDelay+=Time.fixedDeltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!activated)
            {
                if (!UIManager.instance.examineUI.activeInHierarchy && !BoatController.instance.helm.activated && !UIManager.instance.upgradeUI.activeInHierarchy)
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

                if (Input.GetKeyDown(KeyCode.E) && inputDelay >= 0.1f)
                {
                    if (selectedItem.data.isEquippable)
                    {
                        EquipItem(selectedItem);
                    }
                }
                if (Input.GetKeyDown(KeyCode.G) && selectedItem != null && inputDelay >= 0.1f)
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

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseInventory();
            }
        }

    }

    public void OpenInventory()
    {
        activated = true;
        inputDelay = 0;

        //Time.timeScale = activated ? 0.0f : 1.0f;
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);

        UIManager.instance.inventoryUI.SetActive(true);
        UIManager.instance.gameplayUI.SetActive(false);

        UIManager.instance.GetComponent<LockMouse>().LockCursor(false);

        selectedPosition = 0;

        /*if (equippedItem != null && equippedItem.data != null)
        {
            selectedPosition = GetGridPosition(equippedItem.slot.GetIndex());
        }*/

        //play the fade in effect
        //UIManager.instance.inventoryAnimation.Play("Basic Fade-in");

        for (int i = 0; i < UIManager.instance.inventoryBackGrid.transform.childCount; i++)
        {
            UIManager.instance.inventoryBackGrid.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
    public void CloseInventory()
    {
        activated = false;
        //Time.timeScale = activated ? 0.0f : 1.0f;
        if (!BoatController.instance.helm.activated)
        {
            PlayerController.instance.LockMovement(false);
        }

        PlayerController.instance.LockCamera(false);

        UIManager.instance.inventoryUI.SetActive(false);
        UIManager.instance.gameplayUI.SetActive(true);

        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);

        //play the fade out effect
        //UIManager.instance.inventoryAnimation.Play("Basic Fade-out");
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
            UIManager.instance.inventoryBackGrid.transform.GetChild(i).GetComponent<Image>().color = new Color(0.085f, 0.085f, 0.085f, 0.5f);
        }
        UIManager.instance.inventoryBackGrid.transform.GetChild(selectedIndex).GetComponent<Image>().color = new Color(0.85f, 0.85f, 0.85f, 0.5f);
    }

    public InventoryItem AddItem(ItemData itemData, ItemStatus itemStatus)
    {
        //if exists in inventory and stackable
        //if max amount not reached
        //inventoryitemlist[i].itemstatus.amount++
        //else

        int temp = itemStatus.amount;

        foreach (InventoryItem item in inventoryItemList)
        {
            if (item.data == itemData && item.data.isStackable)
            {
                while(item.status.amount < item.data.maxStackAmount && temp > 0)
                {
                    item.status.amount++;
                    temp--;
                    item.slot.amount.text = "" + item.status.amount;
                }

                if (temp <= 0){
                    return item;
                }
            }
        }

        if (temp > 0)
        {
            InventorySlot newSlot1 = Instantiate(slotPrefab, UIManager.instance.inventoryItemGrid.transform);
            InventoryItem newItem1 = new InventoryItem(itemData, new ItemStatus(temp, 1), newSlot1);
            inventoryItemList.Add(newItem1);
            newSlot1.inventoryItem = newItem1;
            newSlot1.image.sprite = itemData.sprite;
            newSlot1.name.text = itemData.name;
            newSlot1.amount.text = $"{temp}";

            if (inventoryItemList.Count > slotPerRow * slotPerColumn)
            {
                
                DropItem(newItem1, itemStatus.amount);
                return null;
            }
            return newItem1;
        }
        return null;

        /*InventorySlot newSlot = Instantiate(slotPrefab, UIManager.instance.inventoryItemGrid.transform);
        InventoryItem newItem = new InventoryItem(itemData, itemStatus, newSlot);
        inventoryItemList.Add(newItem);
        newSlot.inventoryItem = newItem;
        newSlot.image.sprite = itemData.sprite;
        newSlot.name.text = itemData.name;
        newSlot.amount.text = $"{itemStatus.amount}";

        if (inventoryItemList.Count > slotPerRow * slotPerColumn)
        {
            DropItem(newItem);
            return null;
        }
        return newItem;*/

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
                if (equippedItemLeft == inventoryItem || equippedItemRight == inventoryItem || equippedItemCenter == inventoryItem)
                {
                    UnequipItem(inventoryItem.data.equipType);
                }
            }
        }
        else
        {
            inventoryItemList.Remove(inventoryItem);
            Destroy(inventoryItem.slot.gameObject);
            if (equippedItemLeft == inventoryItem || equippedItemRight == inventoryItem || equippedItemCenter == inventoryItem)
            {
                UnequipItem(inventoryItem.data.equipType);
            }
        }

        //loop through items and organize inventory.
        //perhaps a separate functions for this?
    }

    public void DropItem(InventoryItem inventoryItem, int amount = 1)
    {
        if (inventoryItem.data.isStackable)
        {
            if (inventoryItem.status.amount > amount)
            {
                inventoryItem.status.amount -= amount;
                inventoryItem.slot.amount.text = "" + inventoryItem.status.amount;
                GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + PlayerController.instance.tHead.transform.forward, PlayerController.instance.tHead.transform.rotation);
                droppdeObject.GetComponent<I_InventoryItem>().itemStatus.amount = amount;

            }
            else
            {
                inventoryItemList.Remove(inventoryItem);
                Destroy(inventoryItem.slot.gameObject);
                GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + PlayerController.instance.tHead.transform.forward, PlayerController.instance.tHead.transform.rotation);
                droppdeObject.GetComponent<I_InventoryItem>().itemStatus.amount = amount;
                if (equippedItemLeft == inventoryItem || equippedItemRight == inventoryItem || equippedItemCenter == inventoryItem)
                {
                    UnequipItem(inventoryItem.data.equipType);
                }
            }
        }
        else
        {
            inventoryItemList.Remove(inventoryItem);
            Destroy(inventoryItem.slot.gameObject);
            GameObject droppdeObject = Instantiate(inventoryItem.data.dropObject, PlayerController.instance.tHead.transform.position + PlayerController.instance.tHead.transform.forward, PlayerController.instance.tHead.transform.rotation);
            droppdeObject.GetComponentInChildren<I_InventoryItem>().itemStatus.durability = inventoryItem.status.durability;
            droppdeObject.GetComponent<I_InventoryItem>().itemStatus.amount = amount;
            if (equippedItemLeft == inventoryItem || equippedItemRight == inventoryItem || equippedItemCenter == inventoryItem)
            {
                UnequipItem(inventoryItem.data.equipType);
            }
        }

        //loop through items and organize inventory.
        //perhaps a separate functions for this?
    }

    public void DropItem(ItemData itemData, int amount)
    {
        //materialCount -= x;
        
        int temp = amount;

        foreach (InventoryItem item in inventoryItemList)
        {
            if (item.data == itemData && item.data.isStackable)
            {
                while(item.status.amount > 0 && temp > 0)
                {
                    item.status.amount --;
                    temp--;
                    item.slot.amount.text = "" + item.status.amount;

                    //remove from inventory if <= 0
                }
                
                if (temp <= 0){
                    return;
                }

                if (item.status.amount <= 0)
                {
                    inventoryItemList.Remove(item);
                    Destroy(item.slot.gameObject);
                    if (equippedItemLeft == item || equippedItemRight == item || equippedItemCenter == item)
                    {
                        UnequipItem(item.data.equipType);
                    }
                }
                break;
            }
        }
    }

    public void EquipItem(InventoryItem item)
    {


        if (equippedItemLeft == item || equippedItemRight == item || equippedItemCenter == item)
        {
            UnequipItem(item.data.equipType);
        }
        else
        {
            UnequipItem(item.data.equipType);
            Transform equipPivot;

            switch (item.data.equipType)
            {
                case ItemData.EquipType.Left:
                    equippedItemLeft = item;
                    equippedItemLeft.slot.leftHandIcon.enabled = true;
                    equipPivot = PlayerController.instance.equippedTransformLeft;
                    break;

                case ItemData.EquipType.Right:
                    equippedItemRight = item;
                    equippedItemRight.slot.rightHandIcon.enabled = true;
                    equipPivot = PlayerController.instance.equippedTransformRight;
                    break;

                case ItemData.EquipType.Both:
                    equippedItemCenter = item;
                    equippedItemCenter.slot.leftHandIcon.enabled = true;
                    equippedItemCenter.slot.rightHandIcon.enabled = true;
                    equipPivot = PlayerController.instance.equippedTransformCenter;
                    break;

                default:
                    equipPivot = PlayerController.instance.equippedTransformRight;
                    break;
            }
            
            GameObject newObject = Instantiate(item.data.dropObject, equipPivot);
            newObject.name = item.data.dropObject.name + " Equipped";
            //newObject.transform.localPosition = new Vector3(0, 0, 0);
            newObject.transform.localPosition = item.data.equipPosition;
            //newObject.transform.localRotation = item.data.equipRotation;

            //Destroy(newObject.transform.GetChild(0).gameObject);
            //newObject.transform.GetComponentInChildren<Interactable>().enabled = false;
            foreach (Collider collider in newObject.GetComponents<Collider>())
            {
                Destroy(collider);
            }
            //Destroy(newObject.GetComponent<Rigidbody>());
            newObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (PlayerController.instance.targetInteractable != null)
        {
            PlayerController.instance.targetInteractable.Target();
        }
    }
    
    public void UnequipItem(ItemData.EquipType type)
    {
        switch (type)
        {
            case ItemData.EquipType.Left:

                if (equippedItemLeft != null && equippedItemLeft.slot != null)
                {
                    equippedItemLeft.slot.leftHandIcon.enabled = false;
                    if (PlayerController.instance.equippedTransformLeft.childCount > 0)
                    {
                        Destroy(PlayerController.instance.equippedTransformLeft.GetChild(0).gameObject);
                    }
                }
                equippedItemLeft = null;

                if (equippedItemCenter != null && equippedItemCenter.slot != null)
                {
                    equippedItemCenter.slot.leftHandIcon.enabled = false;
                    equippedItemCenter.slot.rightHandIcon.enabled = false;
                    Debug.Log("1");
                    if (PlayerController.instance.equippedTransformCenter.childCount > 0)
                    {
                        Debug.Log("2");
                        Destroy(PlayerController.instance.equippedTransformCenter.GetChild(0).gameObject);
                    }
                }
                equippedItemCenter = null;
                break;

            case ItemData.EquipType.Right:

                if (equippedItemRight != null && equippedItemRight.slot != null)
                {
                    equippedItemRight.slot.rightHandIcon.enabled = false;
                    if (PlayerController.instance.equippedTransformRight.childCount > 0)
                    {
                        Destroy(PlayerController.instance.equippedTransformRight.GetChild(0).gameObject);
                    }
                }
                equippedItemRight = null;

                if (equippedItemCenter != null && equippedItemCenter.slot != null)
                {
                    equippedItemCenter.slot.leftHandIcon.enabled = false;
                    equippedItemCenter.slot.rightHandIcon.enabled = false;
                    if (PlayerController.instance.equippedTransformCenter.childCount > 0)
                    {
                        Destroy(PlayerController.instance.equippedTransformCenter.GetChild(0).gameObject);
                    }
                }
                equippedItemCenter = null;
                break;

            case ItemData.EquipType.Both:

                if (equippedItemLeft != null && equippedItemLeft.slot != null)
                {
                    equippedItemLeft.slot.leftHandIcon.enabled = false;
                    if (PlayerController.instance.equippedTransformLeft.childCount > 0)
                    {
                        Destroy(PlayerController.instance.equippedTransformLeft.GetChild(0).gameObject);
                    }
                }
                equippedItemLeft = null;

                if (equippedItemRight != null && equippedItemRight.slot != null)
                {
                    equippedItemRight.slot.rightHandIcon.enabled = false;
                    if (PlayerController.instance.equippedTransformRight.childCount > 0)
                    {
                        Destroy(PlayerController.instance.equippedTransformRight.GetChild(0).gameObject);
                    }
                }
                equippedItemRight = null;

                if (equippedItemCenter != null && equippedItemCenter.slot != null)
                {
                    equippedItemCenter.slot.leftHandIcon.enabled = false;
                    equippedItemCenter.slot.rightHandIcon.enabled = false;
                    if (PlayerController.instance.equippedTransformCenter.childCount > 0)
                    {
                        Destroy(PlayerController.instance.equippedTransformCenter.GetChild(0).gameObject);
                    }
                }
                equippedItemCenter = null;
                break;
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
            detailGameObject.transform.localScale *= selectedItem.data.examineScale;
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
        
        if (UIManager.instance.detailObjectInBound && Input.GetMouseButtonDown(0))
        {
            detailObjectDrag = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            detailObjectDrag = false;

        }

        if(detailObjectDrag)
        {
            rotateValue.x = -(lookVector.x * 2.5f);
            rotateValue.y = lookVector.y * 2.5f;
            UIManager.instance.detailObjectPivot.GetChild(0).transform.Rotate(PlayerController.instance.tHead.GetChild(0).transform.up, rotateValue.x, Space.World);
            UIManager.instance.detailObjectPivot.GetChild(0).transform.Rotate(PlayerController.instance.tHead.GetChild(0).transform.right, rotateValue.y, Space.World);
            detailRotationFix = true;
        }
        else
        {
            Quaternion currentRotation = UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation;

            if (Quaternion.Angle(currentRotation, selectedItem.data.examineRotation) > 2f && detailRotationFix)
            {
                UIManager.instance.detailObjectPivot.GetChild(0).transform.localRotation = Quaternion.Slerp(currentRotation, selectedItem.data.examineRotation, Time.deltaTime * 10f);
            }
            else{
                detailRotationFix = false;
                UIManager.instance.detailObjectPivot.GetChild(0).transform.Rotate(PlayerController.instance.tHead.GetChild(0).transform.up, 0.5f, Space.World);
            }
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

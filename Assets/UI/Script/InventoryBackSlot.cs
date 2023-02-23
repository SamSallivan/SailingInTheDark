using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBackSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int GetIndex()
    {
        return transform.GetSiblingIndex(); ;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.instance.selectedPosition = InventoryManager.instance.GetGridPosition(GetIndex());
        InventoryManager.instance.selectedIndex = GetIndex();

        if (UIManager.instance.inventoryItemGrid.transform.childCount > GetIndex())
        {
            InventoryManager.instance.EquipItem(UIManager.instance.inventoryItemGrid.transform.GetChild(GetIndex()).gameObject.GetComponent<InventorySlot>().inventoryItem);
        } 
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text amount;
    public Image image;
    public InventoryItem inventoryItem;

    public TMP_Text durability;

    public int GetIndex()
    {
        return transform.GetSiblingIndex(); ;
    }
}

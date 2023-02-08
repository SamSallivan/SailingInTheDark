using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public TMP_Text descriptiontext;

    public void Setup(Collectible info)
    {
        this.name = info.name + $" Slot";
        descriptiontext.text = info.description;
    }
}

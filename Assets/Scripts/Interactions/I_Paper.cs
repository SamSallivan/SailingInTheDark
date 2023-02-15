using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class I_Paper : Interactable
{
    public TMP_Text paper;

    public override IEnumerator InteractionEvent()
    {
        if (itemData != null)
        {
            //InventoryManager.instance.AddItem(itemData, itemStatus);
            //Destroy(highlightTarget.gameObject);
            activated = !activated;
            PlayerController.instance.enableMovement = !PlayerController.instance.enableMovement;

            paper.transform.parent.gameObject.SetActive(activated);
            paper.text = itemData.description;
        }
        yield return null;
    }
}

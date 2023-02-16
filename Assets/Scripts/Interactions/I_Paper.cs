using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class I_Paper : Interactable
{

    public override IEnumerator InteractionEvent()
    {
        if (itemData != null)
        {
            //InventoryManager.instance.AddItem(itemData, itemStatus);
            //Destroy(highlightTarget.gameObject);
            activated = !activated;
            //Time.timeScale = activated ? 0.0f : 1.0f;
            PlayerController.instance.LockMovement(activated);
            PlayerController.instance.LockCamera(activated);

            UIManager.instance.paperUI.SetActive(activated);
            UIManager.instance.paperText.text = itemStatus.text;
            ObjectiveManager.instance.CompleteObjetive("Read Paper");
        }
        yield return null;
    }
    private void Update()
    {
        if (activated)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                StartCoroutine(InteractionEvent());
            }
        }
    }

}

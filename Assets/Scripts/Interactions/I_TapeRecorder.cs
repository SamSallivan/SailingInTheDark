using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class I_TapeRecorder : Interactable
{
    //public Transform playerTargetPos;
    //public LockMouse lockMouse;
    //public RecordingButton recordingButtonClone;
    public GameObject tapeGameObject;
    public Transform targeTransform;

    public override IEnumerator InteractionEvent()
    {
        if (tapeGameObject != null)
        {
            GameObject tape = Instantiate(tapeGameObject, targeTransform.position, targeTransform.rotation, null);
        }

        if (InventoryManager.instance.equippedItem != null)
        {
            DialogueData recording = InventoryManager.instance.equippedItem.data.recording;

            if (recording != null)
            {

                tapeGameObject = InventoryManager.instance.equippedItem.data.dropObject;
                InventoryManager.instance.RemoveItem(InventoryManager.instance.equippedItem);
                DialogueManager.instance.ReplaceRecording(recording);
            }
        }

        yield return null;
    }


    private void Update()
    {

    }

    public void CreateButtons()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        for (int i = 0; i < InventoryManager.instance.inventoryItemList.Count; i++)
        {
            if (InventoryManager.instance.inventoryItemList[i].data.type == ItemData.ItemType.Tape)
            {
                items.Add(InventoryManager.instance.inventoryItemList[i]);
            }
        }
    }

    public override void Target()
    {
        if (highlightTarget != null)
        {
            OutlineRenderer outline = highlightTarget.AddComponent<OutlineRenderer>();
            outline.OutlineMode = OutlineRenderer.Mode.OutlineVisible;
            outline.OutlineWidth = 10;
        }
        UIManager.instance.interactionName.text = textName;
        //UI.instance.interactionPrompt.text = textPrompt;

        if (textPrompt != "" && interactionType != InteractionType.None)
        {
            UIManager.instance.interactionPrompt.text = "'E' ";
            UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
            //enable button prompt image instead
            UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }
    }
}

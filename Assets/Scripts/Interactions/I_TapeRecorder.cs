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
    public GameObject tapeGamePrefab;
    public Transform targeTransform;

    public override IEnumerator InteractionEvent()
    {

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
        else{
            if(DialogueManager.instance.generateTape != null){
                ReturnTape(DialogueManager.instance.generateTape);
                DialogueManager.instance.generateTape = null;
                DialogueManager.instance.StopCurrentRecordingLine();
                DialogueManager.instance.currentRecording = null;
            }
            else{
                ReturnTape();
                DialogueManager.instance.StopCurrentRecordingLine();
                DialogueManager.instance.currentRecording = null;
            }
            DialogueManager.instance.PlayNext();
        }

        yield return null;
    }


    private void Update()
    {

    }

    public void ReturnTape(){
        if (tapeGameObject != null)
        {
            GameObject tape = Instantiate(tapeGameObject, targeTransform.position, targeTransform.rotation, null);
            tapeGameObject = null;
        }
    }
    
    public void ReturnTape(ItemData data){
        GameObject tape = Instantiate(tapeGamePrefab, targeTransform.position, targeTransform.rotation, null);
        tape.GetComponentInChildren<I_InventoryItem>().itemData = data;
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

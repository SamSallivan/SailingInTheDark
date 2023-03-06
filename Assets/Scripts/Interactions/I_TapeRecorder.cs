using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class I_TapeRecorder : Interactable
{
    [ReadOnly]
    public GameObject tapeInserted;
    [ReadOnly]
    public DialogueData recordingPlayed;

    public Transform tapeEjectTransform;
    public GameObject radioIndicator;
    public List<GameObject> radioTapes = new List<GameObject>();

    public override IEnumerator InteractionEvent()
    {

        //if player has a tape inserted
        if(tapeInserted != null)
        {
            //eject it early and stop playing
            EjectTape();
            RecordingManager.instance.StopCurrentLine();
            RecordingManager.instance.currentRecording = null;

        }

        //else if have received message
        else if (radioTapes.Count > 0)
        {
            EjectTape();

            tapeInserted = radioTapes[0];
            recordingPlayed = tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData.recording;
            RecordingManager.instance.ReplaceRecording(recordingPlayed);
            RecordingManager.instance.UnpauseRadio();

            radioTapes.RemoveAt(0);
            if (radioTapes.Count <= 0)
            {
                radioIndicator.SetActive(false);
            }
        }

        //if player is holding a item && the item has a recording
        else if (InventoryManager.instance.equippedItem != null && InventoryManager.instance.equippedItem.data.recording != null && InventoryManager.instance.equippedItem.data.type == ItemData.ItemType.Tape)
        {
            //cut the current recording and play the new one.
            EjectTape();

            tapeInserted = InventoryManager.instance.equippedItem.data.dropObject;
            recordingPlayed = tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData.recording;
            RecordingManager.instance.ReplaceRecording(recordingPlayed);
            RecordingManager.instance.UnpauseRadio();

            InventoryManager.instance.RemoveItem(InventoryManager.instance.equippedItem);
        }
        Target();
        yield return null;
    }


    private void Update()
    {

    }

    public void EjectTape(){
        if (tapeInserted != null)
        {
            GameObject tape = Instantiate(tapeInserted, tapeEjectTransform.position, tapeEjectTransform.rotation, null);
            tapeInserted = null;
        }
    }

    public void  ReceiveRadio(GameObject tape)
    {
        radioIndicator.SetActive(true);
        radioTapes.Add(tape);
    }

    public override void Target()
    {
        if (highlightTarget != null)
        {
            if (!highlightTarget.GetComponent<OutlineRenderer>())
            {
                OutlineRenderer outline = highlightTarget.AddComponent<OutlineRenderer>();
                outline.OutlineMode = OutlineRenderer.Mode.OutlineVisible;
                outline.OutlineWidth = 10;
            }
        }

        UIManager.instance.interactionName.text = textName;

        if (tapeInserted != null)
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += "Eject Tape";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }
        else if (radioTapes.Count > 0)
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += "Receive Radio";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }
        else if (InventoryManager.instance.equippedItem != null && InventoryManager.instance.equippedItem.data.type == ItemData.ItemType.Tape)
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += "Insert Tape";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }
        else
        {
            UIManager.instance.interactionPrompt.text = "";
        }
    }
}

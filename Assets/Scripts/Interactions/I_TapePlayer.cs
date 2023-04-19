using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class I_TapePlayer : Interactable
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
        if (tapeInserted != null)
        {
            //eject it early and stop playing
            RecordingManager.instance.StopCurrentLine();
            RecordingManager.instance.currentRecording = null;
            //EjectTape();
            
            InventoryManager.instance.AddItem(tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData, new ItemStatus(1, 1));
            BoatController.instance.boatAudio.EjectTapeSound();
            tapeInserted = null;
        }

        //else if have received message
        else if (radioTapes.Count > 0)
        {

            tapeInserted = radioTapes[0];
            recordingPlayed = tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData.recording;
            RecordingManager.instance.PlayRecording(recordingPlayed);
            RecordingManager.instance.UnpauseRadio();

            radioTapes.RemoveAt(0);
            if (radioTapes.Count <= 0)
            {
                radioIndicator.SetActive(false);
            }
        }

        else if (InventoryManager.instance.equippedItemRight != null 
                 && InventoryManager.instance.equippedItemRight.data != null 
                 && InventoryManager.instance.equippedItemRight.data.type == ItemData.ItemType.Tape 
                 && InventoryManager.instance.equippedItemRight.data.recording != null)
        {

            //cut the current recording and play the new one.
            //EjectTape();

            tapeInserted = InventoryManager.instance.equippedItemRight.data.dropObject;
            recordingPlayed = tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData.recording;
            RecordingManager.instance.PlayRecording(recordingPlayed);
            RecordingManager.instance.UnpauseRadio();

            InventoryManager.instance.RemoveItem(InventoryManager.instance.equippedItemRight);
        }
        else
        {
            InventoryManager.instance.RequireItemType(ItemData.ItemType.Tape, Play);
        }

        //if player is holding a item && the item has a recording
        /*else if (InventoryManager.instance.equippedItemRight != null)
        {
            if (InventoryManager.instance.equippedItemRight.data != null)
            {

                if (InventoryManager.instance.equippedItemRight.data.type == ItemData.ItemType.Tape &&
                    InventoryManager.instance.equippedItemRight.data.recording != null)
                {
                    //cut the current recording and play the new one.
                    //EjectTape();

                    tapeInserted = InventoryManager.instance.equippedItemRight.data.dropObject;
                    recordingPlayed = tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData.recording;
                    RecordingManager.instance.PlayRecording(recordingPlayed);
                    RecordingManager.instance.UnpauseRadio();

                    InventoryManager.instance.RemoveItem(InventoryManager.instance.equippedItemRight);
                }
                else
                {
                    InventoryManager.instance.RequireItemType(ItemData.ItemType.Tape, Play);
                }
            }
        }

        else
        {
            InventoryManager.instance.RequireItemType(ItemData.ItemType.Tape, Play);
        }*/

        Target();
        yield return null;
    }


    public void Play(InventoryItem item)
    {
        tapeInserted = item.data.dropObject;
        recordingPlayed = tapeInserted.GetComponentInChildren<I_InventoryItem>().itemData.recording;
        RecordingManager.instance.PlayRecording(recordingPlayed);
        RecordingManager.instance.UnpauseRadio();
        InventoryManager.instance.RemoveItem(item);
    }

    public void EjectTape()
    {
        if (tapeInserted != null)
        {
            BoatController.instance.boatAudio.EjectTapeSound();
            GameObject tape = Instantiate(tapeInserted, tapeEjectTransform.position, tapeEjectTransform.rotation, null);
            tapeInserted = null;
        }
    }

    public void ReceiveRadio(GameObject tape, bool autoPlay = false)
    {

        if (autoPlay)
        {
            //eject inserted tape, if any
            //stops current recording, if any
            //play the new recording
        }
        else
        {
            radioIndicator.SetActive(true);
            radioTapes.Add(tape);
        }
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
        else if (InventoryManager.instance.equippedItemRight != null
                 && InventoryManager.instance.equippedItemRight.data != null
                 && InventoryManager.instance.equippedItemRight.data.type == ItemData.ItemType.Tape
                 && InventoryManager.instance.equippedItemRight.data.recording != null)
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += "Insert Tape";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }
        else
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += "Use";
        }
    }
}

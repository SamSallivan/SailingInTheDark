using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactable_TapeRecorder : Interactable
{
    public Transform playerTargetPos;
    public LockMouse lockMouse;
    public List<MouseLook> mouseLooks = new List<MouseLook>();
    public RecordingButton recordingButtonClone;

    public override IEnumerator InteractionEvent()
    {
        activated = !activated;
        PlayerController.instance.enableMovement = !PlayerController.instance.enableMovement;
        textPrompt = activated ? "Exit" : "Use";
        UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;
        UIManager.instance.recordingUI.SetActive(activated);
        lockMouse.LockCursor(!activated);
        if (activated)
        {
            CreateButtons();
        }
        for (int i = 0; i < mouseLooks.Count; i++)
            mouseLooks[i].enabled = !mouseLooks[i].enabled;
        yield return null;
    }

    public void AddRecordingButton(string buttonText, DialogueData recording)
    {
        RecordingButton newButton = Instantiate(recordingButtonClone, UIManager.instance.recordingUI.transform.GetChild(0));
        newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = buttonText;
        newButton.recording = recording;
    }

    private void Update()
    {
        if (activated)
        {
            PlayerController.instance.transform.position = Vector3.Lerp(PlayerController.instance.transform.position, playerTargetPos.position, Time.deltaTime * 5f);
            if (Input.GetKey(KeyCode.Escape))
            {
                StartCoroutine(InteractionEvent());
            }
        }
    }

    public void CreateButtons()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        for(int i = 0; i < InventoryManager.instance.inventoryItemList.Count; i++)
        {
            if (InventoryManager.instance.inventoryItemList[i].data.type == ItemData.ItemType.Tape)
            {
                items.Add(InventoryManager.instance.inventoryItemList[i]);
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            AddRecordingButton(items[i].data.title, items[i].data.recording);
        }
    }
}

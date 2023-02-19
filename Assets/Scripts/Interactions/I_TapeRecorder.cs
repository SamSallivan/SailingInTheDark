using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class I_TapeRecorder : Interactable
{
    //public Transform playerTargetPos;
    public LockMouse lockMouse;
    public RecordingButton recordingButtonClone;

    public override IEnumerator InteractionEvent()
    {
        //Time.timeScale = activated ? 0.0f : 1.0f;
        PlayerController.instance.LockMovement(activated);
        PlayerController.instance.LockCamera(activated);

        UIManager.instance.recordingUI.SetActive(activated);
        if (activated)
        {
            CreateButtons();
            //Time.timeScale = 0;
            lockMouse.LockCursor(false);
        }
        else
        {
            ClearAllButtons();
            //Time.timeScale = 1;
            lockMouse.LockCursor(true);
        }
        yield return null;
    }


    private void Update()
    {
        if (activated)
        {
            //PlayerController.instance.transform.position = Vector3.Lerp(PlayerController.instance.transform.position, playerTargetPos.position, Time.deltaTime * 5f);
            if (Input.GetKey(KeyCode.Escape))
            {
                StartCoroutine(InteractionEvent());
            }
        }
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
        for (int i = 0; i < items.Count; i++)
        {
            AddRecordingButton(items[i].data.title, items[i].data.recording);
        }
    }
    public void AddRecordingButton(string buttonText, DialogueData recording)
    {
        RecordingButton newButton = Instantiate(recordingButtonClone, UIManager.instance.recordingUI.transform.GetChild(0));
        newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = buttonText;
        newButton.recording = recording;
    }

    public void ClearAllButtons()
    {
        foreach (Transform child in UIManager.instance.recordingUI.transform.GetChild(0))
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void CloseMenu()
    {
        StartCoroutine(InteractionEvent());
    }
}

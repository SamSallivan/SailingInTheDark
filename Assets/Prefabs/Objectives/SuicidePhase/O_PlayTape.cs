using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class O_PlayTape : Objective
{
    public string requiredRecordingName;
    public bool enableEndCutscene; //THIS IS ONLY FOR TAPE 4

    public void OnEnable()
    {
        RecordingManager.OnRecordingStart += OnRecordingStart;
        RecordingManager.OnRecordingEnd += OnRecordingEnd;
    }

    public void OnDisable()
    {
        RecordingManager.OnRecordingStart -= OnRecordingStart;
        RecordingManager.OnRecordingEnd -= OnRecordingEnd;
    }

    public void OnRecordingStart()
    {
        if (CheckRecordingName())
        {
            //Finish();
            if (enableEndCutscene)
            {
                RecorderController.hasHeardLastTape = true;
            }

            //ObjectiveManager.instance.ObjectiveList.Remove(this);
            this.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.75f).OnComplete(() => CompleteFinished());
        }
    }

    public void OnRecordingEnd()
    {
        if (CheckRecordingName())
        {
            Finish();
        }
    }
    public bool CheckRecordingName()
    {

        if (RecordingManager.instance.tapePlayer.tapeInserted != null &&
            (RecordingManager.instance.tapePlayer.tapeInserted.GetComponent<I_InventoryItem>().itemData.recording.name == requiredRecordingName ||
             RecordingManager.instance.tapePlayer.tapeInserted.GetComponent<I_InventoryItem>().itemData.recordingName == requiredRecordingName ||
             RecordingManager.instance.tapePlayer.tapeInserted.name == requiredRecordingName))
        {
            return true;
        }
        return false;
    }
}

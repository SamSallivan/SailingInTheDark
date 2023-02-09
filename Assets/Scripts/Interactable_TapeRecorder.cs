using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable_TapeRecorder : Interactable
{
    public bool activated;
    public Transform playerTargetPos;
    public Transform listOfRecordings;
    public Button recordingButton;

    public override IEnumerator InteractionEvent()
    {
        activated = !activated;
        PlayerController.instance.enableMovement = !PlayerController.instance.enableMovement;
        textPrompt = activated ? "Exit" : "Use";
        UI.instance.interactionPrompt.text = "[E] " + textPrompt;
        listOfRecordings.transform.localPosition = activated ? new Vector3(0, 90, 0) : new Vector3(-900, 90, 0);
        yield return null;
    }

    private void Update()
    {
        if (activated)
            PlayerController.instance.transform.position = Vector3.Lerp(PlayerController.instance.transform.position, playerTargetPos.position, Time.deltaTime * 5f);
    }
}

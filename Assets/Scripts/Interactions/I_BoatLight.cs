using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoatComponent))]
public class I_BoatLight : Interactable
{
    public GameObject light;
    public override IEnumerator InteractionEvent()
    {
        GetComponent<BoatComponent>().componentActivated = !GetComponent<BoatComponent>().componentActivated;
        light.SetActive(!light.activeInHierarchy);
        yield return null;
    }

    public override void ShutDown()
    {
        activated = false;
        GetComponent<BoatComponent>().componentActivated = false;
        light.SetActive(false);
        UIManager.instance.interactionPrompt.text = "[E] ";
        UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
    }
}

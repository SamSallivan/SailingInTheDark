using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Engine : Interactable
{
    public override IEnumerator InteractionEvent()
    {
        activated = !activated;
        UIManager.instance.interactionPrompt.text = "[E] ";
        UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt; 

        if (!activated)
        {
            BoatController.instance.helm.ShutDown();
        }
        yield return null;
    }

    public void Update()
    {

        if (activated)
        {
            highlightTarget.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (!activated)
        {
            highlightTarget.GetComponent<Renderer>().material.color = Color.red;
        }
        
    }
}

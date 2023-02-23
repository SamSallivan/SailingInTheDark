using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoatComponent))]
public class I_BoatLight : Interactable
{
    public GameObject lightObject;
    public bool upgraded;

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

        if (!activated && InventoryManager.instance.equippedItem != null &&
            InventoryManager.instance.equippedItem.data.title == "Upgrade for Lights")
        {
            UIManager.instance.interactionPrompt.text = "'E' ";
            UIManager.instance.interactionPrompt.text += "Upgrade";
            //enable button prompt image instead
            UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }

        else if (textPrompt != "" && interactionType != InteractionType.None)
        {
            UIManager.instance.interactionPrompt.text = "'E' ";
            UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
            //enable button prompt image instead
            UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }

    }

    public override IEnumerator InteractionEvent()
    {
        BoatComponent boat = GetComponent<BoatComponent>();
        boat.componentActivated = !boat.componentActivated;
        if (boat.componentActivated &&
            InventoryManager.instance.equippedItem != null &&
            InventoryManager.instance.equippedItem.data.title == "Upgrade for Lights")
        {
            upgraded = true;
            InventoryManager.instance.RemoveItem(InventoryManager.instance.equippedItem);
        }

        lightObject.SetActive(!lightObject.activeInHierarchy);
        yield return null;
    }

    public override void ShutDown()
    {
        activated = false;
        GetComponent<BoatComponent>().componentActivated = false;
        lightObject.SetActive(false);
        UIManager.instance.interactionPrompt.text = "[E] ";
        UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoatComponent))]
public class I_BoatLight : Interactable
{
    public GameObject lightObject;
    public bool upgraded;

    public override IEnumerator InteractionEvent()
    {
        if (!BoatController.instance.engine.activated)
        {

        }
        else
        {
            activated = !activated;
            Target();

            lightObject.SetActive(!lightObject.activeInHierarchy);
            GetComponent<BoatComponent>().componentActivated = !GetComponent<BoatComponent>().componentActivated;
        }

        
        /*if (boat.componentActivated &&
            InventoryManager.instance.equippedItem != null &&
            InventoryManager.instance.equippedItem.data.title == "Upgrade for Lights")
        {
            upgraded = true;
            InventoryManager.instance.RemoveItem(InventoryManager.instance.equippedItem);
        }*/


        yield return null;
    }

    public override void ShutDown()
    {
        activated = false;
        GetComponent<BoatComponent>().componentActivated = false;
        lightObject.SetActive(false);
        if (PlayerController.instance.targetInteractable == this)
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
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



        if (!BoatController.instance.engine.activated)
        {
            UIManager.instance.interactionPrompt.text = "Engine is off";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }

        else if (textPrompt != "" && interactionType != InteractionType.None)
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }


        /*if (!activated && InventoryManager.instance.equippedItem != null &&
            InventoryManager.instance.equippedItem.data.title == "Upgrade for Lights")
        {
            UIManager.instance.interactionPrompt.text = "'E' ";
            UIManager.instance.interactionPrompt.text += "Upgrade";
            //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
        }
        else */
    }
}

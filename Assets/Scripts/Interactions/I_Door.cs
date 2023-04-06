using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Door : Interactable
{
    public Transform closeRotation;
    public Transform openRotation;
    public float speed;

    public bool needKey;
    [ConditionalField(nameof(needKey), false)]
    public ItemData keyItem;
    [ConditionalField(nameof(needKey), false)]
    public bool locked;

    public override IEnumerator InteractionEvent()
    {
        if (locked)
        {
            if (InventoryManager.instance.equippedItemRight != null)
            {
                if (InventoryManager.instance.equippedItemRight.data != null)
                {
                    if (InventoryManager.instance.equippedItemRight.data == keyItem)
                    {
                        locked = false;
                    }
                }
            }
        }
        else
        {
            activated = !activated;
            DoorSwitch(activated);
        }

        Target();

        yield return null;
    }

    public void DoorSwitch(bool opened)
    {
        transform.rotation =  opened ? openRotation.rotation : closeRotation.rotation;
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

        if (locked)
        {
            if (InventoryManager.instance.equippedItemRight != null && InventoryManager.instance.equippedItemRight.data != null && InventoryManager.instance.equippedItemRight.data == keyItem)
            {
                UIManager.instance.interactionPrompt.text = "[E] ";
                UIManager.instance.interactionPrompt.text += "Unlock";
                //UIManager.instance.interactionPromptAnimation.Play("PromptButtonAppear");
            }
            else
            {
                UIManager.instance.interactionPrompt.text = "Locked";
            }
        }
        else
        {
            UIManager.instance.interactionPrompt.text = "[E] ";
            UIManager.instance.interactionPrompt.text += activated ? textPromptActivated : textPrompt;
        } 
    }
}

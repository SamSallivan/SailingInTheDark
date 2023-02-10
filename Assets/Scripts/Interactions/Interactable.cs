using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using static InventoryManager;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        OnceOnly,
        Toggle,
        InventoryItem

    }

    public InteractionType interactionType;

    [ConditionalField(nameof(interactionType), false,InteractionType.OnceOnly)]
    public bool interacted;

    [ConditionalField(nameof(interactionType), false, InteractionType.Toggle)]
    public bool activated;

    [ConditionalField(nameof(interactionType), false, InteractionType.InventoryItem)]
    public ItemData itemData;
    [ConditionalField(nameof(interactionType), false, InteractionType.InventoryItem)]
    public ItemStatus itemStatus;


    public GameObject highlightTarget;
    
    public string textName;
    public string textPrompt;


    public abstract IEnumerator InteractionEvent();

    public virtual void Start()
    {
        if (highlightTarget == null)
        {
            //highlightTarget = this.gameObject;
        }
    }

    public virtual void Interact()
    {
        if (!interacted)
        {
            StartCoroutine(InteractionEvent());
            if (interactionType == InteractionType.OnceOnly)
            {
                interacted = true;
            }
        }
    }

    public void Target()
    {
        if (highlightTarget != null)
        {
            highlightTarget.layer = 6;
        }
        UIManager.instance.interactionName.text = textName;
        //UI.instance.interactionPrompt.text = textPrompt;

        if (textPrompt != "")
        {
            UIManager.instance.interactionPrompt.text = "[E] " + textPrompt;
            //enable button prompt image instead
        }
    }
    public void UnTarget()
    {
        if (highlightTarget != null)
        {
            highlightTarget.layer = 0;
        }
        UIManager.instance.interactionName.text = "";
        UIManager.instance.interactionPrompt.text = "";
        //disable button prompt image here
    }

    public virtual void SetTargetHighlight(bool highlighted)
    {
        if (highlightTarget != null)
        {
            highlightTarget.SetActive(highlighted);
        }
    }
}

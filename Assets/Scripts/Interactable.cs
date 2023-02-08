using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        OneTimeInteration,
         
    }
    public bool oneTimeInteraction;

    [ConditionalField("oneTimeInteraction")]
    public bool interacted;



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
            if (oneTimeInteraction)
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

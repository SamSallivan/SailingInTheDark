using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool interactOnce;

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
            if (interactOnce)
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
        UI.instance.interactionName.text = textName;
        //UI.instance.interactionPrompt.text = textPrompt;

        if (textPrompt != "")
        {
            UI.instance.interactionPrompt.text = "[E] " + textPrompt;
            //enable button prompt image instead
        }
    }
    public void UnTarget()
    {
        if (highlightTarget != null)
        {
            highlightTarget.layer = 0;
        }
        UI.instance.interactionName.text = "";
        UI.instance.interactionPrompt.text = "";
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

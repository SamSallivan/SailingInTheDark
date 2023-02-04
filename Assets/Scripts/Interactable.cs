using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool interactOnce;

    public bool interacted;

    public GameObject highlightTarget;
    
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
    }
    public void UnTarget()
    {
        if (highlightTarget != null)
        {
            highlightTarget.layer = 0;
        }
    }

    public virtual void SetTargetHighlight(bool highlighted)
    {
        if (highlightTarget != null)
        {
            highlightTarget.SetActive(highlighted);
        }
    }
}

using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactable;

public abstract class Trigger : MonoBehaviour
{
    public string targetTag = "Player";

    public bool triggerOnce;

    private bool triggered;

    public abstract IEnumerator TriggerEvent();

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !triggered)
        {

            if (triggerOnce)
            {
                triggered = true;
            }

            StartCoroutine(TriggerEvent());
        }
    }
}

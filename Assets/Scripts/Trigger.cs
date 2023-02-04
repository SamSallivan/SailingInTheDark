using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public bool triggerOnce;
    private bool triggered;

    public abstract IEnumerator TriggerEvent();

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            StartCoroutine(TriggerEvent());
            if (triggerOnce)
            {
                triggered = true;
            }
        }
    }
}

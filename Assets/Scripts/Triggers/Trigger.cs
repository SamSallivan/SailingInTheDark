using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactable;

[RequireComponent(typeof(Rigidbody))]
public class Trigger : MonoBehaviour
{
    public string targetTag = "Player";

    [ReadOnly]
    public bool triggered;

    public bool triggerOnce;

    [ConditionalField(nameof(triggerOnce))]
    [ReadOnly]
    public bool triggeredOnce;


    public virtual void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
    public virtual void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public virtual IEnumerator TriggerEvent()
    {
        yield break;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !triggeredOnce)
        {
            if (triggerOnce)
            {
                triggeredOnce = true;
                SaveManager.instance.eventsPending.Add(this);
            }

            triggered = true;
            StartCoroutine(TriggerEvent());
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag) && !triggeredOnce)
        {
            triggered = false;
        }
    }
}

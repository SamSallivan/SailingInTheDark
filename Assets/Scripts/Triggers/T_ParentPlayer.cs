using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class T_ParentPlayer : Trigger
{
    public Transform playerParent;
    public Transform playerHeight;
    public override IEnumerator TriggerEvent()
    {
        if (PlayerController.instance.gameObject.transform.parent != playerParent.gameObject.transform)
        {
            //Debug.Log("Parenting Player");
            //PlayerController.instance.AttachToBoat(playerParent, playerHeight);
        }

        yield return null;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            //Debug.Log("Detaching Player");
            //PlayerController.instance.DetachFromBoat();

        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
        }
    }
}
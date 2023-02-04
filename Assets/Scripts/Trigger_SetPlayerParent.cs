using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_SetPlayerParent : Trigger
{
    public Transform playerParent;
    public override IEnumerator TriggerEvent()
    {
        PlayerController.instance.gameObject.transform.parent = playerParent.gameObject.transform;
        PlayerController.instance.gameObject.transform.localEulerAngles = new Vector3(0, PlayerController.instance.gameObject.transform.localEulerAngles.y, 0);
        yield return null;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.gameObject.transform.parent = null;
            PlayerController.instance.gameObject.transform.eulerAngles = new Vector3(0, PlayerController.instance.gameObject.transform.eulerAngles.y, 0);

        }
    }
}
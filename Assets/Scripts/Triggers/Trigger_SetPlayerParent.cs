using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_SetPlayerParent : Trigger
{
    public Transform playerParent;
    public PhysicMaterial mat;
    public override IEnumerator TriggerEvent()
    {
        Vector3 temp = PlayerController.instance.transform.eulerAngles;
        PlayerController.instance.gameObject.transform.SetParent(playerParent.gameObject.transform, true);
        PlayerController.instance.transform.localEulerAngles = new Vector3(0, 90, 0);
        mat.staticFriction = 1;
        mat.dynamicFriction = 1;
        yield return null;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 temp = PlayerController.instance.transform.eulerAngles;
            PlayerController.instance.gameObject.transform.SetParent(null, true);
            PlayerController.instance.transform.eulerAngles = new Vector3(0, temp.y + playerParent.transform.eulerAngles.y, 0);
            PlayerController.instance.UntetherFromBoat();
            mat.staticFriction = 0;
            mat.dynamicFriction = 0;

        }
    }
}
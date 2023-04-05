using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform targetTransform;
    void Update()
    {
        if (transform.IsChildOf(PlayerController.instance.transform) && PlayerController.instance.enableMovement)
        {
            KeyCode key;

            if (transform.IsChildOf(PlayerController.instance.equippedTransformLeft))
            {
                key = KeyCode.Mouse0;
            }
            else
            {
                key = KeyCode.Mouse1;
            }

            if (Input.GetKey(key))
            {
                transform.position = Vector3.Lerp(transform.position, PlayerController.instance.tHead.GetChild(4).position, Time.fixedDeltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.instance.tHead.GetChild(4).rotation, Time.fixedDeltaTime * 5);
                gameObject.layer = 5;
                transform.GetChild(0).gameObject.layer = 6;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, PlayerController.instance.equippedTransformLeft.position, Time.fixedDeltaTime * 5);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, GetComponent<I_InventoryItem>().itemData.dropObject.transform.localRotation, Time.fixedDeltaTime * 5);
                gameObject.layer = 0;
                transform.GetChild(0).gameObject.layer = 0;

            }
        }
    }
}

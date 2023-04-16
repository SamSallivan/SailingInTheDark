using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform targetTransform;
    public bool active;
    void Update()
    {
        if (transform.IsChildOf(PlayerController.instance.transform) && (PlayerController.instance.enableMovement || BoatController.instance.helm.inControl || active))
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
                active = true;

                transform.position = Vector3.Lerp(transform.position, PlayerController.instance.tHead.GetChild(4).position, Time.fixedDeltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.instance.tHead.GetChild(4).rotation, Time.fixedDeltaTime * 5);
                gameObject.layer = 5;
                transform.GetChild(0).gameObject.layer = 6;
            }
            else
            {
                active = false;
                transform.localPosition = Vector3.Lerp(transform.localPosition, GetComponent<I_InventoryItem>().itemData.equipPosition, Time.fixedDeltaTime * 5);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, GetComponent<I_InventoryItem>().itemData.dropObject.transform.localRotation, Time.fixedDeltaTime * 5);
                gameObject.layer = 0;
                transform.GetChild(0).gameObject.layer = 0;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public bool active;
    void Update()
    {
        if (transform.IsChildOf(PlayerController.instance.transform) && (PlayerController.instance.enableMovement || BoatController.instance.helm.inControl || active) && !SaveManager.instance.isGameOver)
        {
            KeyCode key;

            gameObject.layer = 5;
            transform.GetChild(0).gameObject.layer = 5;

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

                transform.position = Vector3.Lerp(transform.position, PlayerController.instance.equippedTransformCenter.parent.GetChild(3).position, Time.fixedDeltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.instance.equippedTransformCenter.parent.GetChild(3).rotation, Time.fixedDeltaTime * 5);
            }
            else
            {
                active = false;
                transform.localPosition = Vector3.Lerp(transform.localPosition, GetComponent<I_InventoryItem>().itemData.equipPosition, Time.fixedDeltaTime * 5);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, GetComponent<I_InventoryItem>().itemData.dropObject.transform.localRotation, Time.fixedDeltaTime * 5);
                //gameObject.layer = 0;
                //transform.GetChild(0).gameObject.layer = 0;
            }
        }
    }
}

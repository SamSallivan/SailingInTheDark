using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform targetTransform;
    void Update()
    {
        if (transform.IsChildOf(PlayerController.instance.transform))
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                transform.position = Vector3.Lerp(transform.position, PlayerController.instance.tHead.GetChild(2).position, Time.fixedDeltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.instance.tHead.GetChild(2).rotation, Time.fixedDeltaTime * 5);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, PlayerController.instance.equippedTransform.position, Time.fixedDeltaTime * 5);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, GetComponent<I_InventoryItem>().itemData.dropObject.transform.localRotation, Time.fixedDeltaTime * 5);

            }
        }
    }
}

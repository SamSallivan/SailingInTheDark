using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    public GameObject light;
    // Update is called once per frame
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

            if (Input.GetKeyDown(key))
            {
                light.SetActive(!light.activeInHierarchy);
                /*transform.position = Vector3.Lerp(transform.position, PlayerController.instance.tHead.GetChild(2).position, Time.fixedDeltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.instance.tHead.GetChild(2).rotation, Time.fixedDeltaTime * 5);*/
            }
        }

    }
}

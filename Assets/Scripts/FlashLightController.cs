using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    public GameObject light;
    // Update is called once per frame
    void Update()
    {

        if (transform.IsChildOf(PlayerController.instance.transform))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                light.SetActive(!light.activeInHierarchy);
                /*transform.position = Vector3.Lerp(transform.position, PlayerController.instance.tHead.GetChild(2).position, Time.fixedDeltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.instance.tHead.GetChild(2).rotation, Time.fixedDeltaTime * 5);*/
            }
        }

    }
}

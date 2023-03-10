
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[ExecuteInEditMode]
public class CompassController : MonoBehaviour
{
    public enum CompassType
    {
        Absolute,
        Vertical,
        Flat,
        test
    }
    public CompassType type;
    public Transform target;
    public float perspectiveAdjustment = 20;

    private Vector3 newRotation;

    private void Awake()
    {
        if(target == null)
            target = transform;
    }

    void Update()
    {
        switch(type){
            case CompassType.Absolute:
                target.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case CompassType.Vertical:
                target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, target.transform.eulerAngles.y, target.transform.eulerAngles.y);
                break;
            case CompassType.Flat:
                ref Vector3 reference = ref newRotation;
                Vector3 eulerAngles = PlayerController.instance.transform.eulerAngles;
                reference.y = - eulerAngles.y + perspectiveAdjustment;
                if (transform.IsChildOf(PlayerController.instance.transform) || transform.IsChildOf(UIManager.instance.inventoryUI.transform))
                {
                    target.transform.localEulerAngles = newRotation;
                }
                else{
                    //target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, 0, target.transform.eulerAngles.z);
                    target.transform.eulerAngles = new Vector3(0, 0, 0);
                }

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

                break;
            case CompassType.test:
                target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, 0, target.transform.eulerAngles.z);
                break;
        }
    }
}

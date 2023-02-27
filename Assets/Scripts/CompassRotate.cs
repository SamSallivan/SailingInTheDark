
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[ExecuteInEditMode]
public class CompassRotate : MonoBehaviour
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
                Vector3 eulerAngles = PlayerController.instance.tHead.eulerAngles;
                reference.y = - eulerAngles.y + perspectiveAdjustment;
                if (transform.IsChildOf(PlayerController.instance.transform) || transform.IsChildOf(UIManager.instance.inventoryUI.transform))
                {
                    target.transform.localEulerAngles = newRotation;
                }
                else{
                    //target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, 0, target.transform.eulerAngles.z);
                    target.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                break;
            case CompassType.test:
                target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, 0, target.transform.eulerAngles.z);
                break;
        }
    }
}

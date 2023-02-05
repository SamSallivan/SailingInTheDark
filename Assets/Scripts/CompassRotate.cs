using Hertzole.GoldPlayer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
                reference.y = - eulerAngles.y;
                target.transform.localEulerAngles = newRotation;
                break;
            case CompassType.test:
                target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, 0, target.transform.eulerAngles.z);
                break;
        }
    }
}

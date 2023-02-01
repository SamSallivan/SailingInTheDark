using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassRotate : MonoBehaviour
{
    public Transform currentobjective;
    public Transform compass2D;

    void Update()
    {
        transform.LookAt(currentobjective);
        float change = transform.localRotation.y - compass2D.transform.localRotation.z;
        compass2D.transform.Rotate(0, 0, change, Space.Self);

    }
}

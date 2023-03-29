using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBody : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public Transform target;

    private void Update()
    {
        // direction = target.position - transform.position;
        // float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        Vector3 pos = target.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
    }
}

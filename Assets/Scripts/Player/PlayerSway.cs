using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSway : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    public Quaternion lastRotation = Quaternion.identity;

    public void Awake()
    {
        lastRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        base.transform.rotation = lastRotation;
        base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
        lastRotation = base.transform.rotation;
    }
}

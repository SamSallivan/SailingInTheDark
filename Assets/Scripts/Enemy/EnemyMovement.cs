using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private GameObject boat;
    private Transform boatTransform;

    private float movementSpeed = 3f;
    private float rotationDamp = 5f;
    private float raycastDistance = 5f;
    private float raycastOffset = 2.5f;

    private Transform target;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private Transform _transform;

    public LayerMask boatMask;

    // ----------------------------------------------------------------------------------------------
    private void Awake()
    {
        boat = GameObject.Find("Boat");
        boatTransform = boat.transform;

        target = boatTransform;
    }

    private void Update()
    {
        PathFinding();
        Move();
    }

    private void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDamp * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 pos = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
        // pos.y = Mathf.Clamp(pos.y, -0.5f, 0.5f);
        // Debug.Log(pos);
        pos.y = 0.3f;
        transform.position = pos;
        // transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    private void PathFinding()
    {
        RaycastHit hit;
        Vector3 offset = Vector3.zero;

        Vector3 left = transform.position - transform.right * raycastOffset;
        Vector3 right = transform.position + transform.right * raycastOffset;
        // Vector3 down = transform.position - transform.up;

        Debug.DrawRay(left, transform.forward * raycastDistance, Color.red);
        Debug.DrawRay(right, transform.forward * raycastDistance, Color.red);
        // Debug.DrawRay(down, -transform.up * 3f, Color.red);


        if (Physics.Raycast(left, transform.forward, out hit, raycastDistance))
        {
            Debug.Log(hit.collider.gameObject);
            offset += Vector3.right;
            // offset += hit.normal;
        }
        else if (Physics.Raycast(right, transform.forward, out hit, raycastDistance))
        {
            Debug.Log(hit.collider.gameObject);
            offset += Vector3.left;
            // offset += hit.normal;
        }

        if (offset != Vector3.zero)
        {
            transform.Rotate(offset * Time.deltaTime);
        }
        else
        {
            Turn();
        }
    }
}

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
    public float raycastDistance = 5f;
    public float raycastOffset = 2.5f;
    public float raycastRadius = 2.5f;

    private Transform target;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private Transform _transform;

    public LayerMask boatMask;
    public LayerMask obstacleMask;

    public Vector3 offset;
    public Vector3 offsetLeft = Vector3.zero;
    public Vector3 offsetRight = Vector3.zero;
    public Vector3 left;
    public Vector3 right;

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
        pos.y = 0.3f;
        transform.position = pos;
    }

    private void PathFinding()
    {
        RaycastHit hit;
        offset = Vector3.zero;

        bool ray = Physics.SphereCast(transform.position, raycastRadius, transform.forward, out hit, raycastDistance, obstacleMask);
        if (ray)
        {
            float angle = Vector3.Angle(hit.normal, -transform.forward);

            if (-hit.normal.x > transform.forward.x)
            {
                offset = new Vector3(0f, -(90 - angle), 0);
            }
            else
            {
                offset = new Vector3(0f, 90 - angle, 0);
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + transform.forward * raycastDistance, raycastRadius);
    }
}

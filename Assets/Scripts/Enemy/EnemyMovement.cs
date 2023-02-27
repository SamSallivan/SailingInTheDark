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
      public  Vector3 offsetLeft = Vector3.zero;
      public  Vector3 offsetRight = Vector3.zero;
    public Vector3 left;
    public Vector3 right;

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
        offset = Vector3.zero;
        //  offsetLeft = Vector3.zero;
        //  offsetRight = Vector3.zero;

         left = transform.position - transform.right * raycastOffset;
         right = transform.position + transform.right * raycastOffset;
        // Vector3 down = transform.position - transform.up;

        Debug.DrawRay(left, transform.forward * raycastDistance, Color.red);
        Debug.DrawRay(right, transform.forward * raycastDistance, Color.red);
        // Debug.DrawRay(down, -transform.up * 3f, Color.red);


        //if (Physics.Raycast(left, transform.forward, out hit, raycastDistance))
        // if (Physics.SphereCast(left, raycastRadius, transform.forward, out hit, raycastDistance, obstacleMask))
        // {
        //     Debug.Log(hit.collider.gameObject);
        //     offsetLeft += new Vector3(0, 1, 0);
        //     // offset += hit.normal;
        // }
        //  if (Physics.SphereCast(right, raycastRadius, transform.forward, out hit, raycastDistance, obstacleMask))
        // {
        //     Debug.Log(hit.collider.gameObject);
        //     offsetRight += new Vector3(0, -1, 0);
        //     // offset += hit.normal;
        // }
        bool ray = Physics.SphereCast(transform.position, raycastRadius, transform.forward, out hit, raycastDistance, obstacleMask);
         if (ray)
        {
            float angle = Vector3.Angle(hit.normal, -transform.forward);
            Debug.Log(angle);

            if(-hit.normal.x > transform.forward.x){
                offset = 90 - angle;
            }
            else{

                offset = -(90 - angle);
            }
        }



        if (offset != Vector3.zero)
        {
            transform.Rotate(offset * 10 * Time.deltaTime);
        }
        else
        {
            Turn();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position + transform.forward * raycastDistance, raycastRadius);
        //Gizmos.DrawSphere(left + transform.forward * raycastDistance, raycastRadius);
    }
}

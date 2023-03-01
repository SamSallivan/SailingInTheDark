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
    public float raycastRadius = 2.5f;

    private Transform target;

    public LayerMask boatMask;
    public LayerMask obstacleMask;

    public Vector3 offset;

    public bool canAttack = false;
    private float maxAttackCooldown = 5f; //seconds
    public float curAttackCooldown = 5f; //seconds
    private int attackDamage = 100;

    private void Awake()
    {
        boat = GameObject.Find("Boat");
        boatTransform = boat.transform;

        target = boatTransform;
    }

    //if light: lose meter
    //if meter is 0, run - dip under water
    //if too far, despawn

    //start loop
    //go to nearest waypoint
    //circle to adjacent waypoint
    //if at waypoint, 50% choose to attack
    //if after 3 waypoints, attack anyways
    //charge at boat center: deal damage
    //end loop

    //pick node that's not boat
    //go to node
    //once at node decide which way to turn
    //if left touch, turn left, go to left node
    //opposite for right
    //decide when to attack


    private void Update()
    {
        PathFinding();
    }

    private void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * 2, transform.rotation, boatMask, QueryTriggerInteraction.Collide);

        if (hitColliders.Length > 0)
        {
            // Debug.Log("HAS HIT");
            if (curAttackCooldown <= 0)
            {
                BoatController.instance.TakeDamage(attackDamage);
                curAttackCooldown = maxAttackCooldown;
            }
            else
            {
                curAttackCooldown -= Time.deltaTime;
            }
        }
        else
        {
            curAttackCooldown = maxAttackCooldown;
        }
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

        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + transform.forward * raycastDistance, raycastRadius);
    }
}

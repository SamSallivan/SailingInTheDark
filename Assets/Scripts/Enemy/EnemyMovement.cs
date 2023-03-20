using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public GameObject boat;
    private Transform boatTransform;
    private Transform target; //usually set to boat, can set it to other things if AI gets more complex
    public LayerMask boatMask;
    public LayerMask obstacleMask;

    //stats for AI
    private Vector3 offset;
    [SerializeField] private float movementSpeed = 6f;
    private float rotationDamp = 5f;
    private float raycastDistance = 4f;
    private float raycastRadius = 2.5f;

    //attack variables
    private float maxAttackCooldown = 5f; //seconds
    private float curAttackCooldown; //seconds
    private int attackDamage = 100;
    private int attackRange = 2;

    private float despawnDistance = 80f;

    private void Awake()
    {
        curAttackCooldown = maxAttackCooldown;
        //boat = BoatController.instance.gameObject;
        boatTransform = boat.transform;

        target = boatTransform;
    }

    //STATE MACHINE LOGIC
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
        float distance = Vector3.Distance(gameObject.transform.position, boatTransform.transform.position);
        if (distance > despawnDistance)
        {
            Die();
        }

        PathFinding();
    }

    private void FixedUpdate()
    {
        Attack();
    }

    private void Attack()
    {
        //check if boat is in attack range
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * attackRange, transform.rotation, boatMask, QueryTriggerInteraction.Collide);
        if (hitColliders.Length > 0)
        {
            //attack if boat is detected and enemy attack is off cooldown, then reset cooldown
            if (curAttackCooldown <= 0)
            {
                BoatController.instance.TakeDamage(attackDamage);
                curAttackCooldown = maxAttackCooldown;
            }
            else
            {
                //cooldown timer
                curAttackCooldown -= Time.deltaTime;
            }
        }
        else
        {
            //if boat is not in range, reset cooldown timer
            curAttackCooldown = maxAttackCooldown;
        }
    }

    //turn towards boat position
    private void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDamp * Time.deltaTime);
    }

    //move forward based on forward direction
    private void Move()
    {
        Vector3 pos = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
        pos.y = 0.3f; //clamp to sea level
        transform.position = pos;
    }

    //enemy AI movement
    private void PathFinding()
    {
        RaycastHit hit;
        offset = Vector3.zero; //vector to store turn angle

        //look for obstacle
        bool ray = Physics.SphereCast(transform.position, raycastRadius, transform.forward, out hit, raycastDistance, obstacleMask);
        if (ray)
        {
            //get angle between the normal vector of the obstacle and the direction the enemy is facing to determine
            //which way and how much to turn
            float angle = Vector3.Angle(hit.normal, -transform.forward);
            if (-hit.normal.x > transform.forward.x)
            {
                offset = new Vector3(0f, -(90 - angle), 0f);
            }
            else
            {
                offset = new Vector3(0f, 90 - angle, 0f);
            }
        }

        //if turning is required, rotate the enemy to face the turn direction, otherwise turn towards target
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


    public void Die()
    {
        Debug.Log("creature died");
        EnemySpawner.instance.creatureCount--;
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Color color = Color.red;
        color.a = 0.2f;
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position + transform.forward * raycastDistance, raycastRadius);
    }
}

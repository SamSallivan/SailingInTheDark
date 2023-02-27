using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;

public class EnemyMovementTest : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private Transform _transform;

    private float speed = 2f;
    private float rotationSpeed = 720f;

    private Vector3 direction;
    [SerializeField] private GameObject target;
    private WaypointNode currentWaypoint;

    public Transform leftCheck;
    public Transform frontCheck;
    public Transform rightCheck;

    private float raycastLength = 100f;
    [SerializeField] private LayerMask waypointLayerMask;
    [SerializeField] private WaypointHandler waypointHandler;

    private Vector3 offest;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerTransform = player.transform;
        _rb = gameObject.GetComponent<Rigidbody>();
        _transform = gameObject.transform;

        offest = Vector3.zero;
    }

    private void Start()
    {
        currentWaypoint = GetClosestWaypoint();
        target = currentWaypoint.obj;
    }

    private void Update()
    {
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

        //check if reach target
        if (CheckReachWaypoint())
        {
            currentWaypoint = Random.value < 0.5f ? currentWaypoint.previous : currentWaypoint.next;
            target = currentWaypoint.obj;
        }

        //check if blocking
        if (CheckLeftRaycast())
        {
            offest += Vector3.right;
        }
        else if (CheckRightRaycast())
        {
            offest -= Vector3.right;
        }
        else
        {
            offest = Vector3.zero;
        }
    }

    private bool CheckReachWaypoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(frontCheck.position, frontCheck.forward, out hit, 3f, waypointLayerMask))
        {
            if (hit.collider != null)
            {
                return hit.collider.gameObject == currentWaypoint.obj ? true : false;
            }
        }
        return false;
    }

    private bool CheckLeftRaycast()
    {
        RaycastHit hit;
        int layerMask = ~waypointLayerMask;
        if (Physics.Raycast(rightCheck.position, rightCheck.forward, out hit, 5f, layerMask))
        {
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckRightRaycast()
    {
        RaycastHit hit;
        int layerMask = ~waypointLayerMask;
        if (Physics.Raycast(rightCheck.position, rightCheck.forward, out hit, 5f, layerMask))
        {
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private WaypointNode GetClosestWaypoint()
    {
        WaypointNode closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (WaypointNode wp in waypointHandler.waypoints)
        {
            float dist = Vector3.Distance(wp.obj.transform.position, currentPos);
            if (dist < minDist)
            {
                closest = wp;
                minDist = dist;
            }
        }
        return closest;
    }

    private void FixedUpdate()
    {
        if (target)
        {
            Move(target.transform.position + offest);
        }
    }

    private void Move(Vector3 targetVector)
    {
        //get direction
        Vector3 dir = targetVector - _transform.position;
        dir.Normalize();

        //rotate
        Quaternion toRotation = Quaternion.LookRotation(targetVector - transform.position);
        _transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);

        _rb.AddForce(direction * speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(leftCheck.position, leftCheck.position + 5f * leftCheck.forward);
        Gizmos.DrawLine(frontCheck.position, frontCheck.position + 3f * frontCheck.forward);
        Gizmos.DrawLine(rightCheck.position, rightCheck.position + 5f * rightCheck.forward);
    }
}

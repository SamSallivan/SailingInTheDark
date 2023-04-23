using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
public class enemy_test : MonoBehaviour
{
    public EnemySpawner spawner;
    public EnemyAudio enemyAudioManager;
    public Transform boatTransform;
    public LayerMask boatMask;
    public LayerMask obstacleMask;

    //stats for AI
    [ReadOnly]
    public State curState = State.Move;
    [ReadOnly]
    public float movementSpeed;
    [Tooltip("Normal traveling speed.")]
    public float defaultSpeed = 6f;
    [Tooltip("Speed when charging.")]
    public float burstSpeed = 12f;
    private float rotationSpeed = 5f;
    private float avoidanceSpeed = 5f;
    private float raycastDistance = 3f;
    private float raycastRadius = 2f;
    private Vector3 target; //usually set to boat
    private Vector3 patrolTarget;
    private Vector3 offset;

    //attack stats
    public float maxAttackCooldown = 8f; //seconds
    private float curAttackCooldown; //seconds
    public int attackDamage = 5;
    public float attackRange = 1f;

    //aggro
    [Tooltip("Time enemy stays aggro for if exposed to light.")]
    private float maxAggroMeter = 10f; //seconds
    public float curAggroMeter;
    public float loseAggroMultiplier = 1;
    public bool loseAggro = false;
    public bool isPatrolling = false;
    public bool isDead = false;

    //despawn
    public float despawnDistance = 70f;
    private float deathTimer = 5f;

    //misc
    [Tooltip("Y level the creature stays clamped at.")]
    public float surfaceLevel = 0.1f;

    private Color color = Color.blue;

    private void Start()
    {
        // boatTransform = BoatController.instance.transform;
        // boatTransform = 
        target = boatTransform.position;

        movementSpeed = defaultSpeed;
        curAttackCooldown = maxAttackCooldown;
        curAggroMeter = maxAggroMeter;
    }

    private void Update()
    {
        if (loseAggro)
        {
            color = Color.green;
        }

        if (!isDead)
        {
            // CheckDespawn();
            UpdateState();
            switch (curState)
            {
                case State.Move:
                    target = boatTransform.position;
                    break;
                case State.Patrol:
                    curAttackCooldown -= Time.deltaTime;
                    break;
            }
            PathFinding();
        }
    }

    private void UpdateState()
    {
        if (movementSpeed != defaultSpeed)
        {
            movementSpeed -= 3f * Time.deltaTime;
            if (movementSpeed < defaultSpeed)
            {
                movementSpeed = defaultSpeed;
            }
        }

        if (curState == State.Patrol) //if reach patrol point, pick new spot
        {
            if (Vector3.Distance(target, transform.position) <= 5f)
            {
                color = Color.yellow;
                SetNewPatrolTarget();
            }
        }

        // if (curState == State.Move) //if not patrolling
        // {
        //     if (CheckAttack())
        //     {
        //         curState = State.Patrol;
        //         SetPatrolTarget();
        //     }
        // }
        else if (curAttackCooldown <= 0)
        {
            color = Color.blue;
            curState = State.Move;
            target = boatTransform.position;
        }
    }

    private void SetNewPatrolTarget()
    {
        Vector3 intersection1, intersection2;
        int foundPoints = FindCircleCircleIntersections(transform.position, 15f, boatTransform.position, 20f, out intersection1, out intersection2);

        // target = Random.value <= 0.5f ? intersection1 : intersection2;
        target = boatTransform.position;
    }

    private void SetPatrolTarget()
    {
        Vector2 unitCir = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(unitCir.x, 0, unitCir.y);
        Vector3 patrolPoint = boatTransform.position + direction * 20f;
        target = new Vector3(patrolPoint.x, surfaceLevel, patrolPoint.z);
    }

    // Find the points where the two circles intersect.
    private int FindCircleCircleIntersections(Vector3 c0, float r0, Vector3 c1, float r1, out Vector3 intersection1, out Vector3 intersection2)
    {
        // Find the distance between the centers.
        float dx = c0.x - c1.x;
        float dz = c0.z - c1.z;
        float dist = Mathf.Sqrt(dx * dx + dz * dz);

        if (Mathf.Abs(dist - (r0 + r1)) < 0.00001)
        {
            intersection1 = Vector2.Lerp(c0, c1, r0 / (r0 + r1));
            intersection2 = intersection1;
            return 1;
        }

        // See how many solutions there are.
        if (dist > r0 + r1)
        {
            // No solutions, the circles are too far apart.
            intersection1 = new Vector3(float.NaN, float.NaN, float.NaN);
            intersection2 = new Vector3(float.NaN, float.NaN, float.NaN);
            return 0;
        }
        else if (dist < Mathf.Abs(r0 - r1))
        {
            // Debug.Log("contains - target: " + target + ", pos: " + transform.position);
            // No solutions, one circle contains the other.
            intersection1 = new Vector3(float.NaN, float.NaN, float.NaN);
            intersection2 = new Vector3(float.NaN, float.NaN, float.NaN);
            return 0;
        }
        else if ((dist == 0) && (r0 == r1))
        {
            // No solutions, the circles coincide.
            intersection1 = new Vector3(float.NaN, float.NaN, float.NaN);
            intersection2 = new Vector3(float.NaN, float.NaN, float.NaN);
            return 0;
        }
        else
        {
            // Find a and h.
            float a = (r0 * r0 -
                        r1 * r1 + dist * dist) / (2 * dist);
            float h = Mathf.Sqrt(r0 * r0 - a * a);

            // Find P2.
            float cx2 = c0.x + a * (c1.x - c0.x) / dist;
            float cz2 = c0.z + a * (c1.z - c0.z) / dist;

            // Get the points P3.
            intersection1 = new Vector3(
                (float)(cx2 + h * (c1.z - c0.z) / dist), 0,
                (float)(cz2 - h * (c1.x - c0.x) / dist));
            intersection2 = new Vector3(
                (float)(cx2 - h * (c1.z - c0.z) / dist), 0,
                (float)(cz2 + h * (c1.x - c0.x) / dist));

            return 2;
        }
    }

    //turn towards boat position
    private void TurnToBoat()
    {
        Vector3 pos = target - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    private void TurnToPatrolPoint()
    {
        Vector3 pos = patrolTarget - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    //move forward based on forward direction
    private void Move()
    {
        Vector3 pos = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
        pos.y = surfaceLevel; //clamp to sea level
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
            // var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, offset.y * 2, transform.eulerAngles.z);
            // transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime);
            transform.Rotate(offset * Time.deltaTime * avoidanceSpeed);
        }
        else if (isPatrolling)
        {
            TurnToPatrolPoint();
        }
        else
        {
            TurnToBoat();
        }

        Move();
    }

    public void LoseAggro()
    {
        Debug.Log("HIT");
        curAggroMeter -= loseAggroMultiplier * Time.deltaTime;
        curAttackCooldown = maxAttackCooldown;
        if (curAggroMeter <= 0)
        {
            isDead = true;
            StartCoroutine(enemyAudioManager.PlayDeathSound());
            // enemyAudioManager.PlayDeathSound();
            StartCoroutine(Die());
        }
    }

    private void GainAggro()
    {
        if (curAggroMeter < maxAggroMeter)
        {
            curAggroMeter += Time.deltaTime / 2;
        }
    }

    private IEnumerator Die()
    {
        float diveAngleStart = -10f;
        float diveAngleEnd = 60f;
        float diveAngle = diveAngleStart;
        float _currentLerpTime = 0f;
        while (deathTimer >= 0)
        {
            _currentLerpTime += Time.deltaTime;
            diveAngle = EaseInExpo(_currentLerpTime, diveAngleStart, diveAngleEnd, 5f);
            // Vector3 diveRotationAngle = new Vector3(diveA
            transform.localRotation = Quaternion.Euler(diveAngle, transform.localRotation.y, transform.localRotation.z);

            Vector3 pos = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
            transform.position = pos;

            deathTimer -= Time.deltaTime;
            yield return null;
        }
        // DestroySelf();
    }

    public void DestroySelf()
    {
        // Debug.Log("destroyed self");
        spawner.CreatureDied();
        Destroy(gameObject);
    }

    private float EaseInExpo(float time, float start, float end, float duration)
    {
        return end * (-Mathf.Pow(2, -10 * time / duration) + 1) + start;
    }

    public void EnterLight(float intensity = 50f)
    {
        if (loseAggro != true)
        {
            enemyAudioManager.PlayHurtSound();
        }
        loseAggroMultiplier = intensity / 50;
        loseAggro = true;
    }

    public void ExitLight()
    {
        loseAggro = false;
    }

    private void OnDrawGizmos()
    {
        // Color color = Color.red;
        color.a = 0.2f;
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position + transform.forward * raycastDistance, raycastRadius);
    }
}


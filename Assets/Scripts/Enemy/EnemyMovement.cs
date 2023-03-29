using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public enum State
{
    Move,
    Patrol,
    Death
}

public class EnemyMovement : MonoBehaviour
{
    public EnemySpawner spawner;
    private Transform boatTransform;
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
    public int attackDamage = 10;
    public float attackRange = 1f;

    //aggro
    [Tooltip("Time enemy stays aggro for if exposed to light.")]
    public float maxAggroMeter = 5f; //seconds
    public float curAggroMeter;
    public bool loseAggro = false;
    public bool isPatrolling = false;
    public bool isDead = false;

    //despawn
    private float despawnDistance = 120f;
    private float deathTimer = 5f;

    private void Start()
    {
        boatTransform = BoatController.instance.transform;
        target = boatTransform.position;

        movementSpeed = defaultSpeed;
        curAttackCooldown = maxAttackCooldown;
        curAggroMeter = maxAggroMeter;
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDespawn();
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
                // Debug.Log("reset");
                SetPatrolTarget();
            }
        }

        if (curState != State.Patrol && CheckAttack()) //if not patrolling and attacked
        {
            // Debug.Log("patrol");
            curState = State.Patrol;
            SetPatrolTarget();
        }
        else if (curAttackCooldown <= 0)
        {
            // Debug.Log("move");
            curState = State.Move;
            target = boatTransform.position;
        }
    }

    private void SetPatrolTarget()
    {
        Vector3 direction = Random.insideUnitCircle.normalized;
        Vector3 patrolPoint = boatTransform.position + direction * 20f;
        target = new Vector3(patrolPoint.x, 0.3f, patrolPoint.z);
    }

    private void CheckDespawn()
    {
        float distance = Vector3.Distance(gameObject.transform.position, boatTransform.transform.position);
        if (distance > despawnDistance || loseAggro)
        {
            LoseAggro();
        }
        else
        {
            GainAggro();
        }
    }

    private bool CheckAttack() //return true if hit
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * attackRange, transform.rotation, boatMask, QueryTriggerInteraction.Collide);
        if (hitColliders.Length > 0)
        {
            Attack();
            return true;
        }

        return false;
    }

    private void Attack()
    {
        // Debug.Log("attacked");
        movementSpeed = burstSpeed;
        BoatController.instance.TakeDamage(attackDamage);
        curAttackCooldown = maxAttackCooldown;
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
            // Debug.Log("called: " + offset.y);
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
        curAggroMeter -= Time.deltaTime;
        if (curAggroMeter <= 0)
        {
            isDead = true;
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

    private void IsDead()
    {
        isDead = true;
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

        spawner.CreatureDied();
        Destroy(gameObject);
    }

    private float EaseInExpo(float time, float start, float end, float duration)
    {
        return end * (-Mathf.Pow(2, -10 * time / duration) + 1) + start;
    }

    public void EnterLight()
    {
        loseAggro = true;
    }

    public void ExitLight()
    {
        loseAggro = false;
    }

    private void OnDrawGizmos()
    {
        Color color = Color.red;
        color.a = 0.2f;
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position + transform.forward * raycastDistance, raycastRadius);
    }
}


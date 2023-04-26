using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Collider _collider;

    [ReadOnly]
    public float creatureCount = 0;

    public float totalSpawns = 0;
    public string boatTag = "Boat";
    public int maxCreatureNum = 3;
    public int maxCreatureBeforeDelay = 5;
    public float minSpawnInterval = 10f; //in seconds 
    public float maxSpawnInterval = 60f; //in seconds 
    public float curSpawnInterval; //for incrementing

    public float minDelayAfterKillAll = 180f;
    public float maxDelayAfterKillAll = 300f;
    public float delayAfterKillAll;

    public bool canSpawn = true;

    [Range(0f, 1f)]
    public float chanceForGroupSpawn = 0.5f;
    public int maxCreaturesInGroup = 3;

    [ReadOnly]
    public float spawnTimer;
    public float waitTimer = 0;

    private List<GameObject> enemiesList;

    private void Start()
    {
        enemiesList = new List<GameObject>();

        curSpawnInterval = minSpawnInterval;
        spawnTimer = curSpawnInterval;
        delayAfterKillAll = Random.Range(minDelayAfterKillAll, maxDelayAfterKillAll);
    }

    private void Update()
    {
        //TODO: REMOVE, ONLY FOR TESTING
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillAllCreatures();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        canSpawn = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == boatTag)
        {
            if (canSpawn)
            {
                if (spawnTimer <= 0f)
                {
                    CheckSpawnConditions();
                    spawnTimer = curSpawnInterval;
                }
                else
                {
                    spawnTimer -= Time.deltaTime;
                }
            }
            else
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= delayAfterKillAll)
                {
                    canSpawn = true;
                    waitTimer = 0;
                    delayAfterKillAll = Random.Range(minDelayAfterKillAll, maxDelayAfterKillAll);
                }
            }
        }
    }

    private void CheckSpawnConditions()
    {
        //random chance to spawn extra enemy/enemies
        if (Random.value <= chanceForGroupSpawn)
        {
            int numInGroup = Random.Range(2, maxCreaturesInGroup);
            for (int i = 0; i < numInGroup; i++)
            {
                SpawnEnemy();
            }
        }
        else
        {
            SpawnEnemy();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == boatTag)
        {
            spawnTimer = minSpawnInterval;
            totalSpawns = creatureCount;
        }
    }

    private void SpawnEnemy()
    {
        if (creatureCount < maxCreatureNum)
        {
            // Vector3 spawnPos = RandomSpawnPointInBounds(_collider.bounds);
            Vector3 spawnPos = RandomSpawnPointAroundBoat();
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().spawner = this;
            creatureCount++;
            totalSpawns++;
            //add time to the spawn interval
            curSpawnInterval = curSpawnInterval >= maxSpawnInterval ? maxSpawnInterval : curSpawnInterval + 20f;

            enemiesList.Add(enemy);
        }
    }

    public Vector3 RandomSpawnPointAroundBoat()
    {
        Vector2 unitCir = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(unitCir.x, 0, unitCir.y);
        Vector3 patrolPoint = BoatController.instance.gameObject.transform.position + direction * Random.Range(20f, 40f);
        return patrolPoint;
    }

    public Vector3 RandomSpawnPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0.3f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public void CreatureDied()
    {
        creatureCount--;
        if (totalSpawns >= maxCreatureBeforeDelay || creatureCount <= 0)
        {
            totalSpawns = 0;
            canSpawn = false;
        }
    }

    public void KillAllCreatures()
    {
        foreach (GameObject enemy in enemiesList)
        {
            Destroy(enemy);
        }
        creatureCount = 0;
    }
}

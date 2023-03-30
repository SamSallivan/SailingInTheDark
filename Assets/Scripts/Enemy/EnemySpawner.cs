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

    public string boatTag = "Boat";
    public int maxCreatureNum = 3;
    public float minSpawnInterval = 5f; //in seconds 
    public float maxSpawnInterval = 30f; //in seconds 
    public float curSpawnInterval; //for incrementing

    [Range(0f, 1f)]
    public float chanceForGroupSpawn = 0.5f;
    public int maxCreaturesInGroup = 3;

    [ReadOnly]
    public float spawnTimer;

    private void Start()
    {
        curSpawnInterval = minSpawnInterval;
        spawnTimer = curSpawnInterval;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == boatTag)
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
        }
    }

    private void SpawnEnemy()
    {
        if (creatureCount < maxCreatureNum)
        {
            Vector3 spawnPos = RandomSpawnPointInBounds(_collider.bounds);
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().spawner = this;
            creatureCount++;
            //add time to the spawn interval
            curSpawnInterval = curSpawnInterval >= maxSpawnInterval ? maxSpawnInterval : curSpawnInterval + 5f;
        }
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class EnemySpawner : MonoBehaviour
{
    // public static EnemySpawner instance;
    public GameObject enemyPrefab;
    public Collider _collider;

    [ReadOnly]
    public float creatureCount = 0;

    public string boatTag = "Boat";
    public int maxCreatureNum = 3;
    public float spawnInterval = 10f; //in seconds 
    [ReadOnly]
    public float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == boatTag)
        {
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == boatTag)
        {
            spawnTimer = spawnInterval;
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

            // Debug.Log("spawned: " + spawnPos);
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

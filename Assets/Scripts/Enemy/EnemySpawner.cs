using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public GameObject enemyPrefab;
    private float radius = 50f;
    private int maxCreatureNum = 3;
    [HideInInspector] public float creatureCount = 0;
    private float cooldown = 10f; //in seconds 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", cooldown, cooldown);
    }

    private void SpawnEnemy()
    {
        Debug.Log("spawn creature");
        if (creatureCount < maxCreatureNum)
        {
            Vector3 spawnPos = GetSpawnPosition(gameObject.transform.position);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            creatureCount++;
        }
    }

    private Vector3 GetSpawnPosition(Vector3 centerPoint)
    {
        Vector3 centerOfRadius = centerPoint;
        Vector3 target = centerOfRadius + (Vector3)(radius * UnityEngine.Random.insideUnitCircle);
        return target;
    }

}

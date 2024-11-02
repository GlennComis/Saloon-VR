using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Header("Depedencies")]
    [SerializeField] private InsanityController insanityController;
    
    [Header("Configuration")]
    [SerializeField] private float spawnRate = 60f;
    [Tooltip("Extra enemy spawn rate is based on the current insanity level. Value = Normalized %")]
    [SerializeField] private float extraEnemyInsanityPercentage = 0.8f;
    [Tooltip("1 in X chance of spawning an extra enemy")]
    [SerializeField] private int extraEnemyChance = 6;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnPoints;

    // State
    private float spawnTimer = 0f;

    private void Update()
    {
        if (insanityController.CurrentInsanity < 1f)
        {
            spawnTimer = 0f;
            return;
        }

        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= Mathf.Max(5, spawnRate - insanityController.CurrentInsanity))
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private Vector3 GetSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        return spawnPoints[randomIndex].position;
    }
    
    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, GetSpawnPoint(), Quaternion.identity);

        if (insanityController.CurrentInsanity / insanityController.MaxInsanity > extraEnemyInsanityPercentage) // Above extraEnemyInsanityPercentage insanity, chance of spawning extra enemy
        {
            int randomIndex = Random.Range(0, extraEnemyChance);
            if (randomIndex == 0)
            {
                Instantiate(enemyPrefab, GetSpawnPoint(), Quaternion.identity);
            }
        }
    }
}
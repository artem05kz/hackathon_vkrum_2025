using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // монстр-жучок
    public GameObject spiderPrefab;
    private Transform playerTransform;

    public float spawnDistance = 8f;

    public void SpawnSpider()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
        if (playerTransform != null && spiderPrefab != null)
        {
            Vector2 spawnDirection = UnityEngine.Random.insideUnitCircle.normalized;
            Vector2 spawnPos = (Vector2)playerTransform.position + spawnDirection * spawnDistance;
            Instantiate(spiderPrefab, spawnPos, Quaternion.identity);
        }
    }
}

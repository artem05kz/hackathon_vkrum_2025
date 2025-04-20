using System;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public FallingBlock blockPrefab;    // Префаб падающего блока
    public float spawnInterval = 3f;      // Интервал между спавном блоков
    public float spawnXMin = -8f;         // Минимальное значение X для спавна (с учетом размеров экрана)
    public float spawnXMax = 8f;          // Максимальное значение X для спавна
    public float spawnY = 6f;             // Высота спавна (над экраном)

    void Start()
    {
        InvokeRepeating("SpawnBlock", 1f, spawnInterval);
    }

    void SpawnBlock()
    {
        if (TimeManager.Instance != null && (TimeManager.Instance.IsFrozen || TimeManager.Instance.IsRewind))
        {
            return;
        }

        float xPos = UnityEngine.Random.Range(spawnXMin, spawnXMax);
        Vector2 spawnPos = new Vector2(xPos, spawnY);
        Instantiate(blockPrefab, spawnPos, Quaternion.identity);
    }

}

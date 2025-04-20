using System;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public FallingBlock blockPrefab;    // ������ ��������� �����
    public float spawnInterval = 3f;      // �������� ����� ������� ������
    public float spawnXMin = -8f;         // ����������� �������� X ��� ������ (� ������ �������� ������)
    public float spawnXMax = 8f;          // ������������ �������� X ��� ������
    public float spawnY = 6f;             // ������ ������ (��� �������)

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

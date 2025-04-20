using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Collider2D triggerArea;
    public string playerTag = "Player";

    public DoorController doorBehind;
    public DoorController doorAhead;

    public Transform[] enemySpawnPoints;
    public GameObject[] enemyPrefabs;

    public BlockSpawner blockSpawner;

    private bool triggered = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        doorAhead.Close();
        blockSpawner.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag(playerTag))
        {
            triggered = true;
            EnterRoom();
        }
    }

    private void EnterRoom()
    {
        if (doorBehind != null) doorBehind.Close();

        if (doorAhead != null) doorAhead.Close();

        blockSpawner.enabled = true;

        for (int i = 0; i < enemySpawnPoints.Length && i < enemyPrefabs.Length; i++)
        {
            var go = Instantiate(enemyPrefabs[i], enemySpawnPoints[i].position, Quaternion.identity);
            spawnedEnemies.Add(go);

            var gc = UnityEngine.Object.FindAnyObjectByType<GameController>();
            if (gc != null) gc.RegisterEnemy(go.GetComponent<MonoBehaviour>());
        }
    }

    void Update()
    {
        if (!triggered) return;

        spawnedEnemies.RemoveAll(e => e == null);
        if (spawnedEnemies.Count == 0)
        {
            if (doorAhead != null)
            {
                doorAhead.Open();
                doorBehind.Open();
            }

            enabled = false;
        }
    }
}

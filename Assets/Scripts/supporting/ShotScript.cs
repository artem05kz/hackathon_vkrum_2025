using UnityEngine;

public class ShotScript : MonoBehaviour
{
    public int damage = 20;
    public float timeToDead = 7f;
    public bool isEnemyShot = false;

    void Start()
    {
        Destroy(gameObject, timeToDead);
    }
}
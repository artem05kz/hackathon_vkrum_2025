using System.Diagnostics;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public int damage = 10;
    public float fallSpeed = 5f;
    public float timeToDead = 7f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.linearVelocity = Vector2.down * fallSpeed;

        Destroy(gameObject, timeToDead);
    }

    void FixedUpdate()
    {
        if (rb != null && rb.simulated)
        {
            rb.linearVelocity = Vector2.down * fallSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("Block entered trigger with " + other.name);
        PersonScript player = other.GetComponent<PersonScript>();
        if (player != null && !player.isEnemy)
        {
            player.Damage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }
}

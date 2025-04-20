using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [Tooltip("Время жизни до автоудаления (сек)")]
    public float lifeTime = 15f;
    [Tooltip("Сколько HP восстанавливает")]
    public int healAmount = 30;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PersonScript>();
        if (player != null && !player.isEnemy)
        {
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}

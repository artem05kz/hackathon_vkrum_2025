using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [Tooltip("����� ����� �� ������������ (���)")]
    public float lifeTime = 15f;
    [Tooltip("������� HP ���������������")]
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

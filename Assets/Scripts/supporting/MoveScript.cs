using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 5f;

    void Update()
    {
        if (speed <= 0f) speed = 10f;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
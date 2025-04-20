using System.Collections.Generic;
using UnityEngine;

public struct PointInTime
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
}

public class TimeBody : MonoBehaviour
{
    [Header("Настройки записи")]
    public bool isRewindable = true;
    [Tooltip("Можно ли замораживать этот объект?")]
    public bool canBeFrozen = true;
    [Tooltip("За сколько секунд сохранять историю")]
    public float recordTime = 5f;

    private List<PointInTime> pointsInTime;
    private Rigidbody2D rb;
    private bool isRewinding = false;
    private bool isFrozen = false;

    public bool IsRewinding => isRewinding;
    public bool IsFrozen => isFrozen;

    void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else if (!isFrozen)
        {
            Record();
        }
    }

    void Record()
    {
        var ps = GetComponent<PersonScript>();
        if (ps != null && ps.isDead)
            return;
        int maxPoints = Mathf.RoundToInt(recordTime / Time.deltaTime);
        if (pointsInTime.Count > maxPoints)
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        PointInTime point = new PointInTime
        {
            position = transform.position,
            rotation = transform.rotation,
            velocity = rb != null ? rb.linearVelocity : Vector3.zero
        };
        pointsInTime.Insert(0, point);
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime point = pointsInTime[0];
            transform.position = point.position;
            transform.rotation = point.rotation;
            if (rb != null)
                rb.linearVelocity = point.velocity;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        if (!isRewindable)
            return;
        isRewinding = true;
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Kinematic;

    }

    public void StopRewind()
    {
        isRewinding = false;
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Freeze()
    {
        if (!canBeFrozen)
            return;
        isFrozen = true;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }
    }

    public void Unfreeze()
    {
        if (!canBeFrozen)
            return;
        isFrozen = false;
        if (rb != null)
            rb.simulated = true;
    }
}

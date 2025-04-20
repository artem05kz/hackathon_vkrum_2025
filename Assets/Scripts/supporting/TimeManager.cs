using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private TimeBody[] timeBodies;

    [Header("Настройки времени")]
    [Tooltip("Длительность эффекта перемотки (в секундах)")]
    public float rewindDuration = 5f;
    [Tooltip("Длительность эффекта заморозки (в секундах)")]
    public float freezeDuration = 5f;

    public bool IsFrozen { get; private set; } = false;
    public bool IsRewind { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timeBodies = UnityEngine.Object.FindObjectsByType<TimeBody>(FindObjectsSortMode.None);
    }

    public void StartTimeRewind()
    {
        IsRewind = true;
        timeBodies = UnityEngine.Object.FindObjectsByType<TimeBody>(FindObjectsSortMode.None);
        foreach (TimeBody tb in timeBodies)
        {
            if (tb.isRewindable)
            {
                tb.StartRewind();
            }
        }
        Invoke("StopTimeRewind", rewindDuration);
    }

    public void StopTimeRewind()
    {
        IsRewind = false;
        foreach (TimeBody tb in timeBodies)
        {
            if (tb.isRewindable)
            {
                tb.StopRewind();
            }
        }
    }

    public void StartTimeFreeze()
    {
        FindAnyObjectByType<ScreenEffectsController>()?.ShowFreezeOverlay();

        IsFrozen = true;
        timeBodies = UnityEngine.Object.FindObjectsByType<TimeBody>(FindObjectsSortMode.None);
        foreach (TimeBody tb in timeBodies)
        {
            tb.Freeze();
        }
        Invoke("StopTimeFreeze", freezeDuration);
    }

    public void StopTimeFreeze()
    {
        foreach (TimeBody tb in timeBodies)
        {
            tb.Unfreeze();
        }
        IsFrozen = false;
    }
}

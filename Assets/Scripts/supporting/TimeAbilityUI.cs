using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeAbilityUI : Sounds
{
    public Button rewindButton;
    public Button freezeButton;
    [Tooltip("Время перезарядки способностей в секундах")]
    public float abilityCooldown = 15f;

    private bool canUseAbility = true;

    void Start()
    {
        rewindButton.onClick.AddListener(() =>
        {
            if (canUseAbility)
            {
                UseRewindAbility();
            }
        });
        freezeButton.onClick.AddListener(() =>
        {
            if (canUseAbility)
            {
                UseFreezeAbility();
            }
        });
    }

    void UseRewindAbility()
    {
        PlaySound(sounds[0], 0.15f, p1: 2);
        TimeManager.Instance.StartTimeRewind();
        StartCoroutine(CooldownRoutine());
        
        if (UnityEngine.Random.value < 0.8f)
        {
            EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.SpawnSpider();
            }
        }

    }

    void UseFreezeAbility()
    {
        PlaySound(sounds[1], 0.15f, p1: 2);
        TimeManager.Instance.StartTimeFreeze();
        StartCoroutine(CooldownRoutine());
        
        if (UnityEngine.Random.value < 0.8f)
        {
            EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.SpawnSpider();
            }
        }

    }

    IEnumerator CooldownRoutine()
    {
        canUseAbility = false;
        rewindButton.interactable = false;
        freezeButton.interactable = false;
        yield return new WaitForSeconds(abilityCooldown);
        canUseAbility = true;
        rewindButton.interactable = true;
        freezeButton.interactable = true;
    }
}

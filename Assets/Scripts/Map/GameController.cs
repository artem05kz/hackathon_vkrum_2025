using System.Collections.Generic;
using UnityEngine;

public class GameController : Sounds
{
    [Header("Все враги на сцене")]
    public List<MonoBehaviour> enemies = new List<MonoBehaviour>();
    private bool dangerMusicPlaying = false;

    void Start()
    {
        PlayCalmMusic();
    }

    void Update()
    { 
        bool anyAggroed = false;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            var ranged = enemy as RangedEnemy;
            if (ranged != null && ranged.isAggro)
            {
                anyAggroed = true;
                break;
            }

            var melee = enemy as MeleeEnemy;
            if (melee != null && melee.isAggro)
            {
                anyAggroed = true;
                break;
            }
        }

        if (anyAggroed && !dangerMusicPlaying)
        {
            PlayDangerMusic();
        }
        else if (!anyAggroed && dangerMusicPlaying)
        {
            PlayCalmMusic();
        }
    }

    private void PlayCalmMusic()
    {
        dangerMusicPlaying = false;
        PlayMusic(sounds[0], 0.15f);
    }

    private void PlayDangerMusic()
    {
        dangerMusicPlaying = true;
        PlayMusic(sounds[1], 0.15f);
    }


    public void RegisterEnemy(MonoBehaviour enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(MonoBehaviour enemy)
    {
        enemies.Remove(enemy);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

public class ScreenEffectsController : MonoBehaviour
{
    [Header("—сылки на UI Image")]
    public UnityEngine.UI.Image freezeOverlay;
    public UnityEngine.UI.Image damageOverlay;

    [Header("ѕараметры")]
    public float freezeDisplayTime = 5f;
    public float damageFlashDuration = 0.5f;
    [Range(0, 1)]
    public float lowHealthAlpha = 0.8f;
    [Range(0, 1)]
    public float lowHealthThreshold = 0.3f;

    private PersonScript player;

    void Awake()
    {
        freezeOverlay.gameObject.SetActive(false);
        damageOverlay.gameObject.SetActive(false);
        player = FindAnyObjectByType<PersonScript>();
    }

    void Update()
    {
        if (player == null) return;

        float hpRatio = (float)player.hp / player.maxHp;
        if (hpRatio <= lowHealthThreshold)
        {
            if (!damageOverlay.gameObject.activeSelf)
                damageOverlay.gameObject.SetActive(true);

            var c = damageOverlay.color;
            c.a = lowHealthAlpha;
            damageOverlay.color = c;
        }
        else
        {
            if (!isFlashingDamage)
                damageOverlay.gameObject.SetActive(false);
        }
    }

    bool isFlashingDamage = false;

    public void ShowFreezeOverlay()
    {
        StopCoroutine("FreezeRoutine");
        StartCoroutine(FreezeRoutine());
    }

    IEnumerator FreezeRoutine()
    {
        freezeOverlay.gameObject.SetActive(true);
        yield return new WaitForSeconds(freezeDisplayTime);
        freezeOverlay.gameObject.SetActive(false);
    }

    public void FlashDamageOverlay()
    {
        if (isFlashingDamage) return;
        StartCoroutine(DamageFlashRoutine());
    }

    IEnumerator DamageFlashRoutine()
    {
        isFlashingDamage = true;
        damageOverlay.color = new Color(1, 1, 1, 1);
        damageOverlay.gameObject.SetActive(true);
        yield return new WaitForSeconds(damageFlashDuration);
        isFlashingDamage = false;
        float hpRatio = (float)player.hp / player.maxHp;
        if (hpRatio > lowHealthThreshold)
            damageOverlay.gameObject.SetActive(false);
    }
}

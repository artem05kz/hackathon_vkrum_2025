using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIEnemy : MonoBehaviour
{
    public GameObject barRoot;
    public UnityEngine.UI.Image fillImage;
    private Transform cam;


    void Start()
    {
        cam = Camera.main.transform;
        if (barRoot != null)
            barRoot.SetActive(false);
    }

    void Update()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    public void SetHealth(float current, float max)
    {
        float value = Mathf.Clamp01(current / max);
        fillImage.fillAmount = value;

        if (value >= 1f)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        if (!barRoot.activeSelf)
            barRoot.SetActive(true);
    }

    public void Hide()
    {
        if (barRoot.activeSelf)
            barRoot.SetActive(false);
    }
}

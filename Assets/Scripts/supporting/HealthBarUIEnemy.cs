using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject barRoot;
    public UnityEngine.UI.Image fillImage;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        Hide();
    }

    void Update()
    {
        transform.LookAt(transform.position + cam.forward);
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;
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

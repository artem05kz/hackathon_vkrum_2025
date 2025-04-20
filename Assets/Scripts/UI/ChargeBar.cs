using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    private Image chargeImage;

    void Awake()
    {
        chargeImage = GetComponent<Image>();
    }

    public void SetCharge(float chargeNormalized)
    {
        chargeImage.fillAmount = chargeNormalized;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

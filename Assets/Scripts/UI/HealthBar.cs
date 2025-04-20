using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    public PersonScript person;

    void Start()
    {
        healthBar = GetComponent<Image>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            person = player.GetComponent<PersonScript>();
        }
    }

    void Update()
    {
        if (person != null)
        {
            healthBar.fillAmount = (float)person.hp / person.maxHp;
        }
    }
}

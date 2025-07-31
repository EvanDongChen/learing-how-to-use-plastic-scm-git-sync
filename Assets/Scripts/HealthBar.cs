using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public StatsManager statsManager;
    public Image healthFillImage;
    public TMPro.TextMeshProUGUI healthText;


    void Start()
    {
        if (statsManager == null)
        {
            Debug.LogError("StatsManager not assigned");
        }
    }

    void Update()
    {
        if (statsManager == null)
        {
            return;
        }

        float fillAmount = (float)statsManager.currentHealth / statsManager.maxHealth;
        healthFillImage.fillAmount = fillAmount;

        healthText.text = $"{statsManager.currentHealth}/{statsManager.maxHealth}";
    }
}

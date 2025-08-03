using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public StatsManager statsManager;
    public Image healthFillImage;

    public float lerpSpeed = 3f;
    private float targetFillAmount;

    void Start()
    {
        if (statsManager == null)
        {
            Debug.LogError("StatsManager not assigned");
        }

        targetFillAmount = healthFillImage.fillAmount;
    }

    void Update()
    {
        if (statsManager == null)
            return;

        targetFillAmount = (float)statsManager.currentHealth / statsManager.maxHealth;
        Debug.Log($"Health: {statsManager.currentHealth}/{statsManager.maxHealth}, Fill Amount: {targetFillAmount}");

        healthFillImage.fillAmount = Mathf.Lerp(healthFillImage.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
    }
}

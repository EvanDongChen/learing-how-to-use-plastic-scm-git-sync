using UnityEngine;
using System.Collections;

public class StatsManager : MonoBehaviour
{
    [Header("Base Stats")]
    public int baseHealth = 5;
    public float baseMoveSpeed = 5f;
    public float baseAttackSpeed = 1f;

    [Header("Current Stats")]
    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }
    public float moveSpeed { get; private set; }
    public float attackSpeed { get; private set; }

    [Header("Note Stats")]
    public int beefyCount = 0;

    [Header("Speed Buff Settings")]
    public float speedBuffAmountPerStack = 1f;
    private int speedBuffStacks = 0;
    private float buffTimer = 0f;
    private Coroutine buffCoroutine;

    public bool IsDead { get; private set; } = false;
    void Awake()
    {
        moveSpeed = baseMoveSpeed;
        attackSpeed = baseAttackSpeed;
        RecalculateMaxHealth();
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Healed: " + amount + ", Current Health: " + currentHealth);
    }

    public void RecalculateMaxHealth()
    {
        maxHealth = baseHealth + beefyCount * 5;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"Max Health Recalculated: {maxHealth} (Beefy Count: {beefyCount})");
    }

    public void SetBeefyCount(int count)
    {
        beefyCount = Mathf.Max(0, count);
        RecalculateMaxHealth();
    }

    public void BuffMoveSpeed()
    {
        speedBuffStacks++;
        moveSpeed = baseMoveSpeed + speedBuffStacks * speedBuffAmountPerStack;

        buffTimer = Mathf.Min(buffTimer + 5f, 5f);

        if (buffCoroutine == null)
        {
            buffCoroutine = StartCoroutine(SpeedBuffTimer());
        }

        Debug.Log($"Buffed move speed | Stacks: {speedBuffStacks}, New Move Speed: {moveSpeed}, Timer: {buffTimer}");
    }

    private IEnumerator SpeedBuffTimer()
    {
        while (buffTimer > 0)
        {
            buffTimer -= Time.deltaTime;
            yield return null;
        }

        speedBuffStacks = 0;
        moveSpeed = baseMoveSpeed;
        buffCoroutine = null;
        Debug.Log("Move Speed Buff expired. Reset to base.");
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log("Took Damage: " + amount + ", Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died.");
            IsDead = true;
            GameManager.Instance.updateGameState(GameState.Lose);
        }
    }

    public void ResetStats()
    {
        moveSpeed = baseMoveSpeed;
        attackSpeed = baseAttackSpeed;
        beefyCount = 0;
        currentHealth = baseHealth;
        maxHealth = baseHealth;
        speedBuffStacks = 0;
        buffTimer = 0f;

        if (buffCoroutine != null)
        {
            StopCoroutine(buffCoroutine);
            buffCoroutine = null;
        }

        Debug.Log("Stats Reset.");
    }
}

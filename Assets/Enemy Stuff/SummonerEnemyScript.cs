using UnityEngine;

public class SummonerEnemyScript : MonoBehaviour
{
    enum State { Idle, Walking, Attacking, Dazed } // Added Dazed state
    State currentState = State.Idle;

    public float walkSpeed = 2.5f;
    public float attackRange = 8f;
    public float fireRate = 3f;
    public GameObject minionPrefab;
    public Transform spawnPoint;

    private float fireTimer = 0f;
    private float stateTimer = 0f; // Timer for Dazed state
    private Animator animator;

    void Start()
    {
        currentState = State.Idle;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                GameObject target = FindNearestPlayer();
                if (target != null)
                {
                    currentState = State.Walking;
                    if (animator != null)
                    {
                        animator.SetTrigger("Idle");
                    }
                }
                break;
            case State.Walking:
                GameObject walkTarget = FindNearestPlayer();
                if (walkTarget != null)
                {
                    Vector3 toPlayer = walkTarget.transform.position - transform.position;
                    float dist = toPlayer.magnitude;
                    if (dist <= attackRange)
                    {
                        currentState = State.Attacking;
                        fireTimer = 0f;
                        if (animator != null)
                        {
                            animator.SetTrigger("Summon");
                        }
                    }
                    else
                    {
                        transform.position += toPlayer.normalized * walkSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    currentState = State.Idle;
                    if (animator != null)
                    {
                        animator.SetTrigger("Idle");
                    }
                }
                break;
            case State.Attacking:
                GameObject attackTarget = FindNearestPlayer();
                if (attackTarget != null)
                {
                    Vector3 toPlayer = attackTarget.transform.position - transform.position;
                    float dist = toPlayer.magnitude;
                    if (dist > attackRange)
                    {
                        currentState = State.Walking;
                        if (animator != null)
                        {
                            animator.SetTrigger("Idle");
                        }
                    }
                    else
                    {
                        fireTimer -= Time.deltaTime;
                        if (fireTimer <= 0f)
                        {
                            SpawnMinion(toPlayer.normalized);
                            fireTimer = fireRate;
                        }
                    }
                }
                else
                {
                    currentState = State.Idle;
                    if (animator != null)
                    {
                        animator.SetTrigger("Idle");
                        if (spawnPoint != null)
                        {
                            SpawnMinion(Vector3.zero); // Spawn minions at spawn point if no target
                        }
                    }
                }
                break;
            case State.Dazed:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = State.Idle;
                    if (animator != null)
                    {
                        animator.SetTrigger("Idle");
                    }
                }
                break;
        }
    }

    void SpawnMinion(Vector3 direction)
    {
        if (minionPrefab != null && spawnPoint != null)
        {
            float radius = 2f;
            for (int i = 0; i < 3; i++)
            {
                float angle = i * Mathf.PI * 2f / 3f;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
                Vector3 spawnPos = spawnPoint.position + offset;
                GameObject proj = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    GameObject FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = player;
            }
        }
        return nearest;
    }

    public void ApplyDazedEffect()
    {
        if (animator != null)
        {
            animator.SetTrigger("Dazed");
        }
        currentState = State.Dazed;
        stateTimer = 1f; // Dazed state lasts for 1 second
    }
}

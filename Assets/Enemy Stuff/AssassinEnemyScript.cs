using UnityEngine;

public class AssassinEnemyScript : MonoBehaviour
{
    enum State { Idle, Teleporting, Dashing, Recovering, Resting, Dazed } // Added Dazed state
    State currentState = State.Idle;

    public float dashSpeed = 10f;
    public float dashDuration = 0.3f;
    public float recoveryDuration = 1f;
    public float teleportRadius = 5f;
    public float restDuration = 2f;

    private Vector3 dashDirection;
    private float stateTimer = 0f;
    private int dashCount = 0;
    private int maxDashes = 3;
    private Vector3 lastTeleportPos = Vector3.zero;

    private Animator animator;

    void Start()
    {
        currentState = State.Idle;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                
                dashCount = 0;
                GameObject target = FindNearestPlayer();
                if (target != null)
                {
                    currentState = State.Teleporting;
                }
                break;
            case State.Teleporting:
                GameObject tpTarget = FindNearestPlayer();
                if (tpTarget != null)
                {
                    Vector3 teleportPos = transform.position;
                    int attempts = 0;
                    do
                    {
                        Vector3 randomOffset = Random.insideUnitSphere;
                        randomOffset = randomOffset.normalized * teleportRadius;
                        teleportPos = tpTarget.transform.position + randomOffset;
                        attempts++;
                    } while ((attempts < 10) && (Vector3.Distance(teleportPos, lastTeleportPos) < teleportRadius * 0.5f));
                    transform.position = teleportPos;
                    lastTeleportPos = teleportPos;
                    dashDirection = (tpTarget.transform.position - transform.position).normalized;
                    currentState = State.Dashing;
                    stateTimer = dashDuration;
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
            case State.Dashing:
                transform.position += dashDirection * dashSpeed * Time.deltaTime;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    dashCount++;
                    if (dashCount < maxDashes)
                    {
                        currentState = State.Recovering;
                        stateTimer = recoveryDuration;
                        if (animator != null)
                        {
                            animator.SetTrigger("Dazed");
                        }
                    }
                    else
                    {
                        currentState = State.Resting;
                        stateTimer = restDuration;
                        if (animator != null)
                        {
                            animator.SetTrigger("Dazed");
                        }
                    }
                }
                break;
            case State.Recovering:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = State.Teleporting;
                }
                break;
            case State.Resting:
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
            case State.Dazed:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    if (animator != null)
                    {
                        animator.SetTrigger("Idle");
                    }
                    currentState = State.Idle;
                }
                break;
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

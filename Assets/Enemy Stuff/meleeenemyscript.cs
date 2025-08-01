using UnityEngine;

public class meleeenemyscript : MonoBehaviour
{
    enum State { Idle, Walking, Windup, Dashing, Recovering, Dazed }
    State currentState = State.Idle;

    public float walkSpeed = 3f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.3f;
    public float recoveryDuration = 1f;
    public float dashTriggerDistance = 5f;
    public float windupDuration = 0.5f;

    private Vector3 dashDirection;
    private float stateTimer = 0f;
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

                    Flip(toPlayer.x); // Face player

                    if (dist <= dashTriggerDistance)
                    {
                        dashDirection = toPlayer.normalized;
                        currentState = State.Windup;
                        stateTimer = windupDuration;
                        animator?.SetTrigger("Idle");
                    }
                    else
                    {
                        transform.position += toPlayer.normalized * walkSpeed * Time.deltaTime;
                        animator?.SetTrigger("Run");
                    }
                }
                else
                {
                    currentState = State.Idle;
                    animator?.SetTrigger("Idle");
                }
                break;

            case State.Windup:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = State.Dashing;
                    stateTimer = dashDuration;
                    animator?.SetTrigger("Dash");
                }
                break;

            case State.Dashing:
                transform.position += dashDirection * dashSpeed * Time.deltaTime;
                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                {
                    currentState = State.Recovering;
                    stateTimer = recoveryDuration;
                    animator?.SetTrigger("Dazed");
                }
                break;

            case State.Recovering:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = State.Idle;
                    animator?.SetTrigger("Idle");
                }
                break;

            case State.Dazed:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = State.Idle;
                    animator?.SetTrigger("Idle");
                }
                break;
        }
    }

    void Flip(float xDir)
    {
        if (xDir != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(xDir) * Mathf.Abs(scale.x);
            transform.localScale = scale;
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
        animator?.SetTrigger("Dazed");
        currentState = State.Dazed;
        stateTimer = 1f;
    }
}

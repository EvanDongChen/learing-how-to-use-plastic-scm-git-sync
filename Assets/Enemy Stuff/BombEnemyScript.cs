using UnityEngine;

public class BombEnemyScript : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float explosionRange = 2f;
    public GameObject explosionPrefab;

    private bool exploded = false;

    enum State { Idle, Dazed } // Added Dazed state
    private State currentState = State.Idle;
    private float stateTimer = 0f;
    private Animator animator;

    void Start()
    {
        currentState = State.Idle;
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (exploded) return;

        switch (currentState)
        {
            case State.Idle:
                GameObject target = FindNearestPlayer();
                if (target != null)
                {
                    Vector3 toPlayer = target.transform.position - transform.position;
                    float dist = toPlayer.magnitude;
                    if (dist <= explosionRange)
                    {
                        if (animator != null)
                        {
                            animator.SetTrigger("Explode");
                        }
                        Explode();
                    }
                    else
                    {
                        transform.position += toPlayer.normalized * walkSpeed * Time.deltaTime;
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

    void Explode()
    {
        exploded = true;
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        // TODO: Add damage logic here
        Destroy(gameObject);
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
        currentState = State.Dazed;
        stateTimer = 1f; // Dazed state lasts for 1 second
    }
}

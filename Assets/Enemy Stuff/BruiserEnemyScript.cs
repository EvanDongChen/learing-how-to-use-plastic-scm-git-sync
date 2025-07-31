using UnityEngine;

public class BruiserEnemyScript : MonoBehaviour
{
    public float walkSpeed = 2f;

    enum State { Idle, Dazed } // Added Dazed state
    State currentState = State.Idle;
    float stateTimer = 0f;

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                GameObject target = FindNearestPlayer();
                if (target != null)
                {
                    Vector3 toPlayer = target.transform.position - transform.position;
                    transform.position += toPlayer.normalized * walkSpeed * Time.deltaTime;
                }
                break;

            case State.Dazed:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
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
        currentState = State.Dazed;
        stateTimer = 1f; // Dazed state lasts for 1 second
    }
}

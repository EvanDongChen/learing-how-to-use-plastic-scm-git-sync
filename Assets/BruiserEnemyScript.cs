using UnityEngine;

public class BruiserEnemyScript : MonoBehaviour
{
    public float walkSpeed = 2f;

    void Update()
    {
        GameObject target = FindNearestPlayer();
        if (target != null)
        {
            Vector3 toPlayer = target.transform.position - transform.position;
            transform.position += toPlayer.normalized * walkSpeed * Time.deltaTime;
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
}

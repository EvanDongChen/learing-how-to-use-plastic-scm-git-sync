using UnityEngine;

public class BombEnemyScript : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float explosionRange = 2f;
    public GameObject explosionPrefab;

    private bool exploded = false;

    void Update()
    {
        if (exploded) return;

        GameObject target = FindNearestPlayer();
        if (target != null)
        {
            Vector3 toPlayer = target.transform.position - transform.position;
            float dist = toPlayer.magnitude;
            if (dist <= explosionRange)
            {
                Explode();
            }
            else
            {
                transform.position += toPlayer.normalized * walkSpeed * Time.deltaTime;
            }
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
}

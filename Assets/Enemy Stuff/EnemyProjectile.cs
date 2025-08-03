using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 3f;
    public float speed = 12f;
    public Vector3 direction = Vector3.right;

    void Start()
    {
        Destroy(gameObject, lifetime);
    GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
    }

    // void Update()
    // {
    //     transform.Translate(direction * speed * Time.deltaTime);
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[COLLISION] Projectile collided with: {collision.gameObject.name}");
        HandleHit(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[TRIGGER] Projectile trigger hit: {other.gameObject.name}");
        HandleHit(other.gameObject);
    }

    private void HandleHit(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            Debug.Log("Projectile hit Player!");

            var statsManager = target.GetComponent<StatsManager>();
            if (statsManager != null)
            {
                statsManager.TakeDamage(2);
            }
            else
            {
                Debug.LogWarning("StatsManager not found on Player!");
            }

            Destroy(gameObject); // optional
        }
    }

}

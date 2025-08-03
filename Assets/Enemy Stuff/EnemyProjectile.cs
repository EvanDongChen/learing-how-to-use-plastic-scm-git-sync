using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 3f;
    public float speed = 12f;
    public Vector3 direction = Vector3.right;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var statsManager = collision.gameObject.GetComponent<StatsManager>();
            if (statsManager != null)
            {
                statsManager.TakeDamage(2);
            }
        }
    }
}

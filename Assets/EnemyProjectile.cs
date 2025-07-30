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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Damage player here
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            // Destroy on hitting environment
            Destroy(gameObject);
        }
    }
}

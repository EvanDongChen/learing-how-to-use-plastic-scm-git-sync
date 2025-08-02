using UnityEngine;

public class CombustionScript : MonoBehaviour
{
    public float growDuration = 1f;
    public float growSpeed = 6f;
    private float timer = 0f;
    public NoteData.Elements element = NoteData.Elements.Fire;
    int damage = 2;

    void Update()
    {
        if (timer < growDuration)
        {
            float scaleInc = growSpeed * Time.deltaTime;
            transform.localScale += new Vector3(scaleInc, scaleInc, scaleInc);
            timer += Time.deltaTime;
            if (timer >= growDuration)
            {
                Destroy(gameObject);
            }
        }
    }
      private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthScript enemyHealth = collision.gameObject.GetComponent<EnemyHealthScript>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, element.ToString());
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningBoltScript : MonoBehaviour
{
    public GameObject target; // The target to circle around
    public float speed = 5f;
    public bool searching = false;
    public int hitsleft;
    public Rigidbody2D rb;
    private Collider2D boltCollider;

    int damage = 1;
    public NoteData.Elements element = NoteData.Elements.Lightining;
    private float initialDisableTime = 0.1f; // Time to disable collider after spawn

    void Start()
    {
        searching = false;
        hitsleft = 4;
         rb = GetComponent<Rigidbody2D>();
        boltCollider = GetComponent<Collider2D>();
        if (boltCollider != null)
            StartCoroutine(DisableColliderBriefly());
    }
    

    // Update is called once per frame
    void Update()
    {
        if (searching == false || target == null){
            target = findEnemy();
            searching = true;
            if (target == null){
                Destroy(gameObject);
            }
        }
        move();
        if(hitsleft == 0)
        {
            Destroy(gameObject);
        }

    }
 
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy" && hitsleft == 0)
        {
            
            // Add logic to handle collision (e.g., damage enemy, destroy bullet, etc.)
            Destroy(gameObject); // Destroy bullet on hit
        }
        else
        {
            EnemyHealthScript enemyHealth = collision.gameObject.GetComponent<EnemyHealthScript>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, element.ToString());
                StartCoroutine(MoveAwayFromEnemy(collision));
            }
            searching = false;
            hitsleft--;
        }
    }
    private void move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

    }
    private GameObject findEnemy()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
        
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // Check if this player is closer than the closest found so far
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer; // Return the close


    }

    private IEnumerator MoveAwayFromEnemy(Collision2D collision)
    {
        Vector3 awayDirection = (transform.position - collision.transform.position).normalized;
        float moveAwayTime = 0.5f;
        float elapsedTime = 0f;

        if (boltCollider != null)
            boltCollider.enabled = false; // Disable collider to avoid getting stuck

        while (elapsedTime < moveAwayTime)
        {
            transform.position += awayDirection * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (boltCollider != null)
            boltCollider.enabled = true; // Re-enable collider

        searching = true; // Resume searching after moving away
    }

    private IEnumerator DisableColliderBriefly()
    {
        boltCollider.enabled = false;
        yield return new WaitForSeconds(initialDisableTime);
        boltCollider.enabled = true;
    }
}

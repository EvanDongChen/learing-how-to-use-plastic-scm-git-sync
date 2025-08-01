using UnityEngine;

public class testbullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemyScript = collision.gameObject.GetComponent<MonoBehaviour>();
            if (enemyScript != null)
            {
                var method = enemyScript.GetType().GetMethod("ApplyDazedEffect");
                if (method != null)
                {
                    method.Invoke(enemyScript, null); // Call ApplyDazedEffect dynamically
                }
            }

        }
    }
}

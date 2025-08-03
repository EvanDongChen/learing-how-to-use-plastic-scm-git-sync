using UnityEditor.UI;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public string lastElementHit;
    public GameObject reactionManager; // Assign ReactionManager in the inspector
    public GameObject textNumberManager; // Assign TextNumberManager in the inspector

    public GameObject damageTextPrefab;       // Assign your floatingText prefab here

    public EnemySpawner spawner;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        // Ensure the GameObject has a Rigidbody
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }

        // Ensure the GameObject has a Collider
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        // Find the ReactionManager GameObject and its script
        GameObject reactionManagerObject = GameObject.Find("ReactionManager");
        if (reactionManagerObject != null)
        {
            reactionManager = reactionManagerObject;
        }
        else
        {
            Debug.LogWarning("ReactionManager GameObject not found in the scene.");
        }

        // Find the TextNumberManager GameObject and its script
        GameObject textNumberManagerObject = GameObject.Find("TextNumberManager");
        if (textNumberManagerObject != null)
        {
            textNumberManager = textNumberManagerObject;
        }
        else
        {
            Debug.LogWarning("TextNumberManager GameObject not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage, string element = "None")
    {
        if (!string.IsNullOrEmpty(lastElementHit) && lastElementHit != "None")
        {
            if (lastElementHit == element)
            {
                Debug.Log($"Enemy hit by the same element: {element}. No combo triggered.");
            }
            else
            {
                string combo = lastElementHit + " + " + element;
                Debug.Log($"Element combo triggered: {combo}");

                if (reactionManager != null)
                {
                    ReactionManager rm = reactionManager.GetComponent<ReactionManager>();
                    TextNumberManager tnm = textNumberManager.GetComponent<TextNumberManager>();

                    if (rm != null)
                    {
                        if ((lastElementHit == "Water" && element == "Fire") || (lastElementHit == "Fire" && element == "Water"))
                        {
                            rm.WaterFireReaction();
                            damage *= 2;
                            tnm.SpawnText("VAPORIZE", Color.purple, transform.position);

                        }
                        else if ((lastElementHit == "Fire" && element == "Lightining") || (lastElementHit == "Lightining" && element == "Fire"))
                        {
                            rm.FireLightningReaction(transform.position);
                            tnm.SpawnText("COMBUSTION", Color.orange, transform.position);

                        }
                        else if ((lastElementHit == "Water" && element == "Lightining") || (lastElementHit == "Lightining" && element == "Water"))
                        {
                            rm.WaterLightningReaction(transform.position);
                            tnm.SpawnText("ELECTROFLOW", Color.greenYellow, transform.position);


                        }
                    }
                }

                lastElementHit = "None"; // Reset last element hit after combo
            }
        }
        else
        {
            lastElementHit = element; // Set the new element as the last hit
        }

        // Update the sprite based on the last element hit
        Transform elementStatusChild = transform.Find("elementstatus");
        if (elementStatusChild != null)
        {
            elementalstatus es = elementStatusChild.GetComponent<elementalstatus>();
            if (es != null)
            {
                if (lastElementHit == "Water")
                {
                    es.ChangeToWaterSprite();
                }
                else if (lastElementHit == "Fire")
                {
                    es.ChangeToFireSprite();
                }
                else if (lastElementHit == "Lightining")
                {
                    es.ChangeToLightningSprite();
                }
                else
                {
                    es.ClearSprite();
                    Debug.Log("No sprite to show for the current element.");
                }
            }
        }

        // Call the damage number function from TextNumberManager
        if (textNumberManager != null)
        {
            TextNumberManager tnm = textNumberManager.GetComponent<TextNumberManager>();
            if (tnm != null)
            {
                Color damageColor = Color.white; // Default color for damage numbers
                if (element == "Fire") damageColor = Color.red;
                else if (element == "Water") damageColor = Color.blue;
                else if (element == "Lightning") damageColor = Color.yellow;

                tnm.SpawnText(damage.ToString(), damageColor, transform.position);
            }
        }
          
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage from {element}. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");

        Destroy(gameObject);
    }
}

using UnityEngine;

using System.Collections.Generic;

public class ProjectileScript : MonoBehaviour
{
    private float duration;
    public NoteData.Elements element;
    private List<NoteData.AttributeType> attributes;

    public GameObject PlayerStatManager;

    private string noteName;
    private int rarity;
    private int level;

    [HideInInspector] public Vector3 direction = Vector3.right;
    public float speed = 5f;

    public int damage = 1;

    public GameObject explosionPrefab; // Assign in inspector or via manager

    // Returns damage after attribute modifications
    public int GetModifiedDamage(int baseDamage)
    {
        int modifiedDamage = baseDamage;
        if (attributes == null) return modifiedDamage;

        foreach (var attr in attributes)
        {
            switch (attr)
            {
                case NoteData.AttributeType.Forte:
                    modifiedDamage *= 2;
                    break;
                case NoteData.AttributeType.Harmonic:
                    // TODO: Enhance next note to deal double damage, stack if next is harmonic
                    break;
                case NoteData.AttributeType.Symponic:
                    // TODO: Bonus damage per note of matching type
                    break;
                case NoteData.AttributeType.Legato:
                    // TODO: Increase damage by # of legato notes in your music
                    break;
                    // Staccato, Accelerando, Reverb are handled in ProjectileManagerScript or elsewhere
            }
        }
        return modifiedDamage;
    }


    public void ApplyPlayerEffects(GameObject player, int damageDealt)
    {
        if (attributes == null || PlayerStatManager == null) return;
        foreach (var attr in attributes)
        {
            switch (attr)
            {
                case NoteData.AttributeType.Melody:
                    // Heal player for a portion of damage dealt
                    PlayerStatManager.GetComponent<StatsManager>().Heal(1);
                    break;
                case NoteData.AttributeType.Cadence:
                    // Increase player movement speed temporarily
                    PlayerStatManager.GetComponent<StatsManager>().BuffMoveSpeed(); // Example: Boost speed by 5 for 3 seconds
                    break;
            }
        }
    }

    public void Initialize(NoteData data)
    {
        duration = data.noteDuration;
        element = data.element;
        attributes = data.attributes;
        noteName = data.noteName;
        rarity = data.rarity;
        level = data.level;

        damage = level; // Set damage equal to level

        // Set color based on element
        Color color = Color.white;
        switch (element)
        {
            case NoteData.Elements.Fire:
                color = Color.red;
                break;
            case NoteData.Elements.Water:
                color = Color.blue;
                break;
            case NoteData.Elements.Lightining:
                color = Color.yellow;
                break;
        }
        // For 2D objects, use SpriteRenderer
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color;
        }

        // Accelerando: increase speed
        if (attributes != null && attributes.Contains(NoteData.AttributeType.Accelerando))
        {
            speed *= 1.5f;
        }

        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            int finalDamage = GetModifiedDamage(damage);
            EnemyHealthScript enemyHealth = collision.gameObject.GetComponent<EnemyHealthScript>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(finalDamage, element.ToString());
            }

            ApplyPlayerEffects(PlayerStatManager, finalDamage);

            // Reverb: spawn a larger, stationary note as explosion
            if (attributes != null && attributes.Contains(NoteData.AttributeType.Reverb))
            {
                GameObject explosionNote = Instantiate(gameObject, transform.position, Quaternion.identity);
                ProjectileScript ps = explosionNote.GetComponent<ProjectileScript>();
                if (ps != null)
                {
                    ps.speed = 0f;
                    explosionNote.transform.localScale = transform.localScale * 2f;
                }
            }
            Destroy(gameObject);
        }
    }
}
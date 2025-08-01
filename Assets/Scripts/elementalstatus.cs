using UnityEngine;

public class elementalstatus : MonoBehaviour
{
    public Sprite waterSprite;
    public Sprite fireSprite;
    public Sprite lightningSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeToWaterSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = waterSprite;
            Debug.Log("Changed to Water sprite.");
        }
    }

    public void ChangeToFireSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = fireSprite;
            Debug.Log("Changed to Fire sprite.");
        }
    }

    public void ChangeToLightningSprite()
    {
        if (spriteRenderer != null)
        {
            if (lightningSprite != null)
            {
                spriteRenderer.sprite = lightningSprite;
                Debug.Log("Changed to Lightning sprite.");
            }
            else
            {
                Debug.LogError("Lightning sprite is not assigned in the Inspector.");
            }
        }
        else
        {
            Debug.LogError("SpriteRenderer is null. Cannot change to Lightning sprite.");
        }
    }

    public void ClearSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = null;
            Debug.Log("Cleared the sprite. No sprite is now shown.");
        }
        else
        {
            Debug.LogError("SpriteRenderer is null. Cannot clear the sprite.");
        }
    }
}

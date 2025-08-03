using UnityEngine;

public class DoorController : MonoBehaviour
{
    private BoxCollider2D doorCollider;
    private SpriteRenderer spriteRenderer;

    [Header("Door Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    private void Awake()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OpenDoor()
    {
        doorCollider.enabled = false;
        spriteRenderer.sprite = openSprite;
    }

    public void CloseDoor()
    {
        doorCollider.enabled = true;
        spriteRenderer.sprite = closedSprite;
    }
}

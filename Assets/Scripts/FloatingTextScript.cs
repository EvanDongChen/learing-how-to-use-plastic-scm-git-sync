using UnityEngine;
using TMPro;

public class FloatingTextScript : MonoBehaviour
{
    public float floatSpeed = 0.4f;  // Float upward speed
    public float lifetime = 2f;      // Time before destruction

    private TextMeshPro textMesh;
    private Color originalColor;
    private float timer = 0f;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
    }

    void Update()
    {
        // Move upward
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade out over lifetime
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0, timer / lifetime);
        Color c = originalColor;
        c.a = alpha;
        textMesh.color = c;

        // Destroy after lifetime ends
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text, Color color)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();

        textMesh.text = text;
        originalColor = color;
        textMesh.color = originalColor;
    }
}

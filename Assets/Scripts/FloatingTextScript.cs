using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{
    public float lifetime = 2f;
    public float moveSpeed = 1f;
    private TextMesh textMesh;
    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        if (textMesh != null)
        {
            originalColor = textMesh.color;
        }
        Destroy(gameObject, lifetime); // Destroy after lifetime
    }

    // Update is called once per frame
    void Update()
    {
        // Move the text upwards
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Fade out the text
        if (textMesh != null)
        {
            float fadeAmount = Mathf.Clamp01(lifetime - Time.timeSinceLevelLoad);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAmount);
        }
    }
}

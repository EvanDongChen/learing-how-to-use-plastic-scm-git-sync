using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class TextNumberManager : MonoBehaviour
{
    public GameObject floatingTextPrefab; // Assign your floating text prefab in the Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnText(string text, Color color, Vector3 position)
    {
        if (floatingTextPrefab != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            Text uiText = floatingText.GetComponentInChildren<Text>(); // Use Text component for UI
            if (uiText != null)
            {
                uiText.text = text;
                uiText.color = color;
            }
            else
            {
                Debug.LogError("Floating text prefab is missing a Text component.");
            }
        }
        else
        {
            Debug.LogError("Floating text prefab is not assigned.");
        }
    }

    public void SpawnNumber(int number, Color color, Vector3 position)
    {
        SpawnText(number.ToString(), color, position);
    }
}

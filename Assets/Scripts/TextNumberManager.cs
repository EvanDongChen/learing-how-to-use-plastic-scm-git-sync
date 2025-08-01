using UnityEngine;
using UnityEngine.UI; // Required for UI components
using TMPro;
public class TextNumberManager : MonoBehaviour
{
    public GameObject damageTextPrefab; // Assign your floating text prefab in the Inspector

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
        if (damageTextPrefab != null)
        {
            position.z = 0f;  // Fix the z position for 2D
            position.y += 0.25f;

            GameObject textObj = Instantiate(damageTextPrefab, position, Quaternion.identity);

            FloatingTextScript ft = textObj.GetComponent<FloatingTextScript>();
            if (ft != null)
            {
                ft.SetText(text, color);
            }
        }
    }


}

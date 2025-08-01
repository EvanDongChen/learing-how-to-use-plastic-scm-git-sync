using UnityEngine;

public class ReactionManager : MonoBehaviour
{

    public GameObject CombustionEffect; // Assign the effect prefab in the inspector
    public GameObject ElectroflowEffect; // Assign the effect prefab in the inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WaterFireReaction()
    {
        Debug.Log("Water + Fire reaction triggered!");
    }

    public void FireLightningReaction()
    {
        Debug.Log("Fire + Lightning reaction triggered!");
    }

    public void WaterLightningReaction()
    {
        Debug.Log("Water + Lightning reaction triggered!");
    }
}

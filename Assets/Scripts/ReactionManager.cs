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

    public void FireLightningReaction(Vector3 spawnPos)
    {
        Debug.Log("Fire + Lightning reaction triggered!");
        Instantiate(CombustionEffect, spawnPos, Quaternion.identity);
    }

    public void WaterLightningReaction(Vector3 spawnPos)
    {
        Debug.Log("Water + Lightning reaction triggered!");
                Instantiate(ElectroflowEffect, spawnPos, Quaternion.identity);

    }
}

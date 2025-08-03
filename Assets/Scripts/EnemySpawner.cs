using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;          // Assign multiple enemy prefabs in the Inspector
    public Transform[] spawnPoints;            // Assign spawn points for enemies

    [Header("Wave and Door Settings")]
    public DoorController door;                // Reference to the room's door
    public int waveNumber = 0;

    private int enemiesRemaining = 0;
    private bool waveActive = false;

    void Start()
    {
        if (waveNumber == 0)
        {
            StartWave();
        }
    }

    // Call this from your RoomController when the player enters
    public void StartWave()
    {
        if (waveActive) return;

        waveNumber++;
        int enemyCount = waveNumber + 2;
        enemiesRemaining = enemyCount;
        waveActive = true;

        door.CloseDoor(); // Close the door before spawning starts
        StartCoroutine(SpawnEnemies(enemyCount));
    }

    private IEnumerator SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Pick a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Pick a random enemy prefab
            GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Instantiate enemy
            GameObject enemy = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

            // Link enemy to this spawner
            EnemyHealthScript healthScript = enemy.GetComponent<EnemyHealthScript>();
            if (healthScript != null)
            {
                healthScript.spawner = this;
            }

            yield return new WaitForSeconds(0.5f); // Delay between spawns
        }
    }

    // Called by EnemyHealthScript when an enemy dies
    public void OnEnemyDied()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0 && waveActive)
        {
            waveActive = false;
            door.OpenDoor(); // Open door after clearing the wave
        }
    }
}

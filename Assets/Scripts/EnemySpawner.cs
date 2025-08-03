using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    [Header("Wave and Door Settings")]
    public DoorController[] doors;
    public int waveNumber = 0;

    [Header("Spawner Behavior")]
    public bool autoStartOnGameBegin = false;

    private bool waveActive = false;

    void Start()
    {
        if (autoStartOnGameBegin)
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        if (waveActive) return;

        waveNumber++;
        int enemyCount = waveNumber + 2;
        waveActive = true;

        CloseAllDoors();
        StartCoroutine(SpawnEnemies(enemyCount));
    }

    private IEnumerator SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

            // Make sure enemy is tagged "Enemy" in the prefab for detection
            yield return new WaitForSeconds(0.5f);
        }

        // After spawning all enemies, start monitoring for wave end
        StartCoroutine(CheckForWaveEnd());
    }

    private IEnumerator CheckForWaveEnd()
    {
        while (waveActive)
        {
            yield return new WaitForSeconds(1f);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                waveActive = false;
                OpenAllDoors();
                yield break;
            }
        }
    }

    private void CloseAllDoors()
    {
        if (doors != null)
        {
            foreach (var door in doors)
                door.CloseDoor();
        }
    }

    private void OpenAllDoors()
    {
        if (doors != null)
        {
            foreach (var door in doors)
                door.OpenDoor();
        }
    }

    public bool IsWaveActive()
    {
        return waveActive;
    }

}

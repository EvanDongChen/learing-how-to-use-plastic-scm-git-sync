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

    [Header("Spawner Behavior")]
    public bool autoStartOnGameBegin = false;


    void Start()
    {
        if (autoStartOnGameBegin)
        {
            StartWave(GameManager.Instance.currentWave);
        }
    }

    public void StartWave(int currentWave)
    {

        int enemyCount = GameManager.Instance.currentWave*2 + 1;// Example logic for enemy count
        GameManager.Instance.SetWaveActive(true);

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
        while (GameManager.Instance.isWaveActive)
        {
            yield return new WaitForSeconds(1f);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                GameManager.Instance.OnWaveEnd();
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

}

using UnityEngine;

public class RoomController : MonoBehaviour
{
    public EnemySpawner spawner;

    private bool roomActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!roomActivated && other.CompareTag("Player"))
        {
            roomActivated = true;
            spawner.StartWave();
        }
    }
}

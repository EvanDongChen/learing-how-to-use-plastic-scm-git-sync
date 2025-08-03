using UnityEngine;

public class RoomController : MonoBehaviour
{
    public EnemySpawner spawner;
    public DoorController[] doorsToCloseOnEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.isWaveActive && GameManager.Instance.state == GameState.RoundStart)
        {
            // Close the doors of the *previous room*
            foreach (var door in doorsToCloseOnEnter)
            {
                if (door != null)
                    door.CloseDoor();
            }

            // Start this room's wave
            GameManager.Instance.StartWave();
        }
    }
}

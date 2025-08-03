using UnityEngine;

public class LyreScript : MonoBehaviour
{
    public Transform player; // Assign the player GameObject's transform in the Inspector
    public float followSpeed = 5f; // Adjust for smoothness
    public Animator animator; // Assign in Inspector
    public float offsetDistance = 1f; // How far from the player to offset towards the cursor
    public float holdDuration = 0.5f; // How long to stay at the offset position
    public ParticleSystem attackParticles; // Assign in Inspector

    private float holdTimer = 0f;
    private Vector3 holdPosition;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerAttack();
        }
        */
    }

    public void TriggerAttack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        if (player != null)
        {
            // Get direction from player to cursor in world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z - player.position.z);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 direction = (worldMousePos - player.position);
            direction.z = 0;
            if (direction.sqrMagnitude > 0.001f)
                direction = direction.normalized;
            else
                direction = Vector3.right; // Default direction if mouse is exactly at player

            // Move lyre to player position plus offset in direction of cursor
            holdPosition = player.position + direction * offsetDistance;
            transform.position = holdPosition;
            holdTimer = holdDuration;

            if (attackParticles != null)
                Instantiate(attackParticles, holdPosition, Quaternion.identity);
        }
    }

    void LateUpdate()
    {
        if (holdTimer > 0f)
        {
            holdTimer -= Time.deltaTime;
            transform.position = holdPosition;
        }
        else if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }
}

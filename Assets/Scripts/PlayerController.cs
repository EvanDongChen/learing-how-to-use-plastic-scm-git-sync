using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f; 

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private PlayerInputActions inputActions;
    private StatsManager statsManager;
    public Animator animator; // Re-added animator reference

    // Add your effect reference here (example: ParticleSystem)
    public ParticleSystem moveEffect;
    // If using GameObject: public GameObject moveEffect;

    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        inputActions = InputManager.Instance.InputActions;
        inputActions.Player.Enable();

        rb.freezeRotation = true;

        statsManager = GetComponent<StatsManager>();
        if (statsManager == null)
        {
            Debug.LogError("StatsManager not found");
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Dash.performed += OnDash; 
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Dash.performed -= OnDash;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        UpdateAnimationState(); // Update animation state on movement
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!isDashing && dashCooldownTimer <= 0f && moveInput != Vector2.zero)
        {
            isDashing = true;
            dashTimeRemaining = dashDuration;
            dashDirection = moveInput.normalized;
            animator.SetTrigger("Dodge"); // Trigger dodge animation
        }
    }

    private void FixedUpdate()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.fixedDeltaTime;
        }

        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimeRemaining -= Time.fixedDeltaTime;

            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }
        else
        {
            rb.linearVelocity = moveInput * statsManager.moveSpeed;
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        float movementThreshold = 0.1f;

        bool isMoving = !isDashing && rb.linearVelocity.magnitude > movementThreshold;

        if (isDashing)
        {
            animator.SetBool("IsRunning", false);
            animator.ResetTrigger("WalkLeft");
            animator.ResetTrigger("WalkRight");
        }
        else if (isMoving)
        {
            animator.SetBool("IsRunning", true);

            if (moveInput.x > 0.01f)
            {
                animator.SetTrigger("WalkRight");
                animator.ResetTrigger("WalkLeft");
            }
            else if (moveInput.x < -0.01f)
            {
                animator.SetTrigger("WalkLeft");
                animator.ResetTrigger("WalkRight");
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.ResetTrigger("WalkLeft");
            animator.ResetTrigger("WalkRight");
        }

        // Handle move effect
        if (moveEffect != null)
        {
            if (isMoving)
            {
                if (!moveEffect.isPlaying)
                    moveEffect.Play();
            }
            else
            {
                if (moveEffect.isPlaying)
                    moveEffect.Stop();
            }
        }
        // If using GameObject:
        // if (moveEffect != null) moveEffect.SetActive(isMoving);
    }
}
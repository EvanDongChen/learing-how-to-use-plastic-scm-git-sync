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

    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();
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
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        
        if (!isDashing && dashCooldownTimer <= 0f && moveInput != Vector2.zero)
        {
            isDashing = true;
            dashTimeRemaining = dashDuration;
            dashDirection = moveInput.normalized;
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
    }
}

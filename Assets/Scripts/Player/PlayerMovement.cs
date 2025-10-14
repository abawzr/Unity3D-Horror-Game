using UnityEngine;

/// <summary>
/// Handles player movement using CharacterController.
/// Receives input externally, applies gravity, and ensures proper grounding.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundedOffset = 1f;
    [SerializeField] private float groundedRadius = 0.4f;
    [SerializeField] private LayerMask groundLayers;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private float _verticalVelocity;
    private bool _isGrounded;
    private Vector3 _groundSpherePosition;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Subscribes to the InputManager's OnMove event.
    /// </summary>
    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnMove += HandleMovement;
    }

    /// <summary>
    /// Unsubscribes to the InputManager's OnMove event.
    /// </summary>
    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnMove -= HandleMovement;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }

    /// <summary>
    /// Called once per frame by Unity.  
    /// Handles player movement and gravity application.  
    /// - Checks if the player is grounded.  
    /// - Applies gravity if the player is in the air.  
    /// - Computes movement direction based on input.  
    /// - Moves the CharacterController accordingly.
    /// </summary>
    private void Update()
    {
        CheckGround();
        ApplyGravity();

        Vector3 moveDirection = (transform.right * _moveInput.x + transform.forward * _moveInput.y).normalized;
        Vector3 finalDirection = moveDirection * moveSpeed;

        finalDirection.y = _verticalVelocity;

        _controller.Move(finalDirection * Time.deltaTime);
    }

    /// <summary>
    /// Create sphere to check if player is grounded or not.
    /// If it is grounded then set vertical velocity to -2f.
    /// </summary>
    private void CheckGround()
    {
        _groundSpherePosition.Set(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        _isGrounded = Physics.CheckSphere(_groundSpherePosition, groundedRadius, groundLayers);

        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f; // Small downward force to ensure CharacterController stays grounded
        }
    }

    /// <summary>
    /// Applies gravity to the player when not grounded.
    /// </summary>
    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// Updates movement input from InputManager.
    /// </summary>
    private void HandleMovement(Vector2 input)
    {
        _moveInput = input;
    }
}

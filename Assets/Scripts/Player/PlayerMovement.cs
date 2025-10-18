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

    private CharacterController _controller;
    private Vector2 _moveInput;
    private float _verticalVelocity = -2f;

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

    /// <summary>
    /// Called once per frame by Unity.  
    /// Handles player movement and gravity application.  
    /// - Applies gravity if the player is in the air.  
    /// - Computes movement direction based on input.  
    /// - Moves the CharacterController accordingly.
    /// </summary>
    private void Update()
    {
        ApplyGravity();

        Vector3 moveDirection = (transform.right * _moveInput.x + transform.forward * _moveInput.y).normalized;
        Vector3 finalDirection = moveDirection * moveSpeed;

        finalDirection.y = _verticalVelocity;

        _controller.Move(finalDirection * Time.deltaTime);
    }

    /// <summary>
    /// Applies gravity to the player when not grounded.
    /// </summary>
    private void ApplyGravity()
    {
        if (_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f; // Small downward force to ensure CharacterController stays grounded
        }

        if (!_controller.isGrounded)
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

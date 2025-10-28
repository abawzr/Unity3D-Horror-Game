using System.Collections;
using UnityEngine;

/// <summary>
/// Handles player movement using CharacterController.
/// Receives input externally, applies gravity, and ensures proper grounding.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepAudioInterval;
    [SerializeField] private float footstepAudioPositionOffset;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private Vector3 _finalDirection;
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

    private void Start()
    {
        StartCoroutine(PlayFootstep());
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

        _moveDirection = (transform.right * _moveInput.x + transform.forward * _moveInput.y).normalized;
        _finalDirection = _moveDirection * moveSpeed;

        _finalDirection.y = _verticalVelocity;

        _controller.Move(_finalDirection * Time.deltaTime);
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

    private IEnumerator PlayFootstep()
    {
        while (true)
        {
            if (_moveDirection != Vector3.zero && AudioManager.Instance != null && _controller.isGrounded)
            {
                AudioManager.Instance.Play3DSFX(footstepClip, transform.position - Vector3.up * footstepAudioPositionOffset, volume: 0.5f);
            }

            yield return new WaitForSeconds(footstepAudioInterval);
        }
    }
}

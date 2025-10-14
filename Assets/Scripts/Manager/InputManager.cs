using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton manager that handles player input via the new Unity Input System.
/// Provides events for movement, looking, interaction, flashlight, and item dropping.
/// </summary>
public class InputManager : MonoBehaviour
{
    // The input action asset generated from the Input System
    private InputSystem _playerInputActions;

    /// <summary>
    /// Singleton instance of InputManager.
    /// </summary>
    public static InputManager Instance { get; private set; }

    #region Input Events

    /// <summary>
    /// Fired when the player provides movement input (e.g., WASD or analog stick).
    /// Vector2 represents horizontal and vertical axes.
    /// </summary>
    public event Action<Vector2> OnMove;

    /// <summary>
    /// Fired when the player moves the look input (mouse or controller stick).
    /// Provides the input device and look vector.
    /// </summary>
    public event Action<InputDevice, Vector2> OnLook;

    /// <summary>
    /// Fired when the player presses the interact button.
    /// </summary>
    public event Action OnInteract;

    /// <summary>
    /// Fired when the player toggles the flashlight input.
    /// </summary>
    public event Action OnFlashlight;

    /// <summary>
    /// Fired when the player presses the drop item input.
    /// </summary>
    public event Action OnDropItem;

    #endregion

    // when enabling this script, subscribe to all input system asset events
    private void OnEnable()
    {
        if (_playerInputActions == null) return;

        // Subscribe to input events
        _playerInputActions.Player.Move.performed += HandleMove;
        _playerInputActions.Player.Look.performed += HandleLook;
        _playerInputActions.Player.Look.canceled += HandleLookCanceled;

        _playerInputActions.Player.Interact.performed += HandleInteract;
        _playerInputActions.Player.Flashlight.performed += HandleFlashlight;
        _playerInputActions.Player.DropItem.performed += HandleDropItem;
    }

    // when disabling this script, unsubscribe from all input system asset events
    private void OnDisable()
    {
        if (_playerInputActions == null) return;

        // Unsubscribe from input events
        _playerInputActions.Player.Move.performed -= HandleMove;
        _playerInputActions.Player.Look.performed -= HandleLook;
        _playerInputActions.Player.Look.canceled -= HandleLookCanceled;

        _playerInputActions.Player.Interact.performed -= HandleInteract;
        _playerInputActions.Player.Flashlight.performed -= HandleFlashlight;
        _playerInputActions.Player.DropItem.performed -= HandleDropItem;
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);

        // Initialize input action asset
        _playerInputActions = new InputSystem();
        _playerInputActions.Enable();
    }

    public void TurnOffInputs()
    {
        _playerInputActions.Player.Disable();
        OnMove?.Invoke(Vector2.zero);
        OnLook?.Invoke(null, Vector2.zero);
    }

    #region Handlers

    private void HandleMove(InputAction.CallbackContext ctx) => OnMove?.Invoke(ctx.ReadValue<Vector2>());
    private void HandleLook(InputAction.CallbackContext ctx) => OnLook?.Invoke(ctx.control.device, ctx.ReadValue<Vector2>());
    private void HandleLookCanceled(InputAction.CallbackContext ctx) => OnLook?.Invoke(ctx.control.device, Vector2.zero);
    private void HandleInteract(InputAction.CallbackContext ctx) => OnInteract?.Invoke();
    private void HandleFlashlight(InputAction.CallbackContext ctx) => OnFlashlight?.Invoke();
    private void HandleDropItem(InputAction.CallbackContext ctx) => OnDropItem?.Invoke();

    #endregion
}

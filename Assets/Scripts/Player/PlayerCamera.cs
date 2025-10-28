using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player's camera rotation based on input from mouse or gamepad.
/// Supports adjustable sensitivity and optional Y-axis inversion.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    [Tooltip("Invert vertical look (Y-axis).")]
    [SerializeField] private bool invertY = false;

    [Header("References")]
    [Tooltip("Transform of the player head, used for vertical rotation.")]
    [SerializeField] private Transform playerHead;

    [Header("Mouse Sensitivity")]
    [Range(0.1f, 1f)][SerializeField] private float mouseSensitivityX;
    [Range(0.1f, 1f)][SerializeField] private float mouseSensitivityY;

    [Header("Gamepad Sensitivity")]
    [Range(0.1f, 1f)][SerializeField] private float gamepadSensitivityX;
    [Range(0.1f, 1f)][SerializeField] private float gamepadSensitivityY;

    /// <summary>
    /// Input vector received from InputManager's OnLook event.
    /// </summary>
    private Vector2 _lookInput;

    /// <summary>
    /// Tracks whether the current input device is a gamepad.
    /// </summary>
    private bool _isGamepad = false;

    /// <summary>
    /// Current vertical rotation of the camera (pitch).
    /// </summary>
    private float _xRotation = 0f;

    /// <summary>
    /// Subscribes to the InputManager's OnLook event.
    /// </summary>
    private void OnEnable()
    {
        // on enabling this script, subscribe to OnLook event
        if (InputManager.Instance != null)
            InputManager.Instance.OnLook += HandleLook;
    }

    /// <summary>
    /// Unsubscribes from the InputManager's OnLook event.
    /// </summary>
    private void OnDisable()
    {
        // on disabling this script, unsubscribe from OnLook event
        if (InputManager.Instance != null)
            InputManager.Instance.OnLook -= HandleLook;
    }

    /// <summary>
    /// Applies rotation to the camera each frame based on the latest input.
    /// Mouse input is applied directly; gamepad input is multiplied by Time.deltaTime.
    /// </summary>
    private void LateUpdate()
    {
        float mouseX, mouseY;

        if (_isGamepad)
        {
            // Multiply by deltaTime for smooth continuous rotation on gamepad
            mouseX = _lookInput.x * gamepadSensitivityX * Time.deltaTime;
            mouseY = _lookInput.y * gamepadSensitivityY * Time.deltaTime;
        }
        else
        {
            // Mouse input is already frame-rate independent
            mouseX = _lookInput.x * mouseSensitivityX;
            mouseY = _lookInput.y * mouseSensitivityY;
        }

        // Invert Y if needed and clamp vertical rotation between -90 and 90
        _xRotation -= invertY ? -mouseY : mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // Apply vertical rotation to camera (player head)
        if (playerHead != null)
            playerHead.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Apply horizontal rotation to player object
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Receives input from InputManager and detects device type (mouse/gamepad).
    /// </summary>
    private void HandleLook(InputDevice device, Vector2 vector2)
    {
        _lookInput = vector2;

        if (device is Gamepad)
        {
            _isGamepad = true;
        }
        else if (device is Mouse)
        {
            _isGamepad = false;
        }
    }
}

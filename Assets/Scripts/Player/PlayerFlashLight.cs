using UnityEngine;

/// <summary>
/// Handles toggling the player's flashlight on and off
/// in response to input events from InputManager.
/// </summary>
public class PlayerFlashLight : MonoBehaviour
{
    [SerializeField] private Light flashlightSpotLight;

    /// <summary>
    /// Subscribes to the InputManager's OnFlashlight event.
    /// </summary>
    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnFlashlight += HandleFlashlight;
    }

    /// <summary>
    /// Unsubscribes from the InputManager's OnFlashlight event.
    /// </summary>
    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnFlashlight -= HandleFlashlight;
    }

    /// <summary>
    /// Toggles the flashlight GameObject on or off.
    /// </summary>
    private void HandleFlashlight()
    {
        if (flashlightSpotLight != null)
            flashlightSpotLight.enabled = !flashlightSpotLight.enabled;
    }
}

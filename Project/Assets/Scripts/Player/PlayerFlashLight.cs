using UnityEngine;

/// <summary>
/// Handles toggling the player's flashlight on and off
/// in response to input events from InputManager.
/// </summary>
public class PlayerFlashLight : MonoBehaviour
{
    [SerializeField] private Light flashlightSpotLight;
    [SerializeField] private AudioClip turnOnClip;
    [SerializeField] private AudioClip turnOffClip;

    private bool _isOff;

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

    private void Awake()
    {
        _isOff = true;
    }

    /// <summary>
    /// Toggles the flashlight GameObject on or off.
    /// </summary>
    private void HandleFlashlight()
    {
        if (_isOff && !flashlightSpotLight.enabled)
        {
            _isOff = false;
            flashlightSpotLight.enabled = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play3DSFX(turnOnClip, transform.position, volume: 0.2f);
            }
        }
        else
        {
            _isOff = true;
            flashlightSpotLight.enabled = false;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play3DSFX(turnOffClip, transform.position, volume: 0.2f);
            }
        }
    }
}

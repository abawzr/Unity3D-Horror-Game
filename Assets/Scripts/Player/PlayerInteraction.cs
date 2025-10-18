using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

/// <summary>
/// Handles player interactions in response to input events from <see cref="InputManager"/>.
/// Casts a ray from the player's head forward and interacts with any object implementing <see cref="IInteractable"/>.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange;
    [SerializeField] private Transform playerHead;
    [SerializeField] private LayerMask interactableLayer;

    private const string PROMPT_PRESS = "Press E to ";
    private const string PROMPT_INVENTORY_FULL = "Inventory is Full!";
    private IInteractable _currentTarget;

    public static event Action<string> OnInteractableObjectLook;

    /// <summary>
    /// Subscribes to <see cref="InputManager.OnInteract"/> when enabled.
    /// </summary>
    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract += HandleInteraction;
    }

    /// <summary>
    /// Unsubscribes from <see cref="InputManager.OnInteract"/> when disabled.
    /// </summary>
    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract -= HandleInteraction;
    }

    /// <summary>
    /// Draws a debug ray and sphere to visualize interaction range in the Scene view.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (playerHead == null) return;

        // draw interaction ray
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(playerHead.position, playerHead.forward * interactionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerHead.position + playerHead.forward * interactionRange, 0.1f);
    }

    private void Update()
    {
        if (playerHead == null) return;

        Ray ray = new(playerHead.position, playerHead.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange, interactableLayer))
        {
            // Ray Hits Something
            // Invoke OnInteractableObjectLook event

            if (hitInfo.collider.TryGetComponent<IInteractable>(out var interactableObject))
            {
                // There is new interactable object detected
                if (_currentTarget != interactableObject)
                {
                    // Current target not equal to new interactable object
                    _currentTarget = interactableObject;
                    if (playerInventory.IsInventoryFull() && interactableObject is Item)
                    {
                        OnInteractableObjectLook?.Invoke(PROMPT_INVENTORY_FULL);
                        return;
                    }
                    OnInteractableObjectLook?.Invoke(PROMPT_PRESS + interactableObject?.InteractionPrompt);
                }
                else if (interactableObject is not Item)
                {
                    OnInteractableObjectLook?.Invoke(PROMPT_PRESS + interactableObject?.InteractionPrompt);
                }
            }
            else
            {
                if (_currentTarget != null)
                {
                    _currentTarget = null;
                    OnInteractableObjectLook?.Invoke(string.Empty);
                }
            }
        }
        else
        {
            if (_currentTarget != null)
            {
                _currentTarget = null;
                OnInteractableObjectLook?.Invoke(string.Empty);
            }
        }
    }

    /// <summary>
    /// Handles player interaction input by raycasting forward and calling Interact on any <see cref="IInteractable"/> hit.
    /// </summary>
    private void HandleInteraction()
    {
        if (playerHead == null) return;

        Ray ray = new(playerHead.position, playerHead.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange, interactableLayer))
        {
            IInteractable interactableObject = hitInfo.collider.GetComponent<IInteractable>();
            interactableObject?.Interact();
        }
    }
}
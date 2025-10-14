using System;
using UnityEngine;

/// <summary>
/// Handles door interaction, opening, closing, and locked state logic.
/// Plays audio and animation when door state changes.
/// </summary>
[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Lock Settings")]
    [SerializeField] private bool isLocked = false; // Whether the door is initially locked
    [SerializeField] private ItemSO requiredKey; // Key required to unlock the door

    [Header("Audio")]
    [SerializeField] private AudioClip openDoorClip; // Audio played when door opens
    [SerializeField] private AudioClip closeDoorClip; // Audio played when door closes
    [SerializeField] private AudioClip lockedDoorClip; // Audio played when door is locked
    [SerializeField] private AudioClip unlockDoorClip; // Audio played when door opens with key

    private const string OPEN_DOOR = "Open"; // Animator parameter name
    private const string SLAM_OPEN_DOOR = "Slam Open Door"; // Animator parameter name
    private const string PROMPT_OPEN = "Open Door";
    private const string PROMPT_CLOSE = "Close Door";

    private Animator _animator;
    private DoorState _currentState = DoorState.Closed;

    /// <summary>
    /// Current state of the door (Opened or Closed)
    /// </summary>
    public DoorState CurrentState => _currentState;

    public enum DoorState { Opened, Closed };

    /// <summary>
    /// Fired when the door is opened.
    /// </summary>
    public event Action OnDoorOpened;

    /// <summary>
    /// Fired when the door is closed.
    /// </summary>
    public event Action OnDoorClosed;

    public string InteractionPrompt { get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Initialize door as closed
        OnDoorClosed?.Invoke();
        _currentState = DoorState.Closed;
        InteractionPrompt = PROMPT_OPEN;
    }

    /// <summary>
    /// Opens the door, plays animation and audio, and fires the OnDoorOpened event.
    /// </summary>
    private void OpenDoor()
    {
        _animator.SetBool(OPEN_DOOR, true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play3DSFX(openDoorClip, transform.position);

        _currentState = DoorState.Opened;
        OnDoorOpened?.Invoke();
        InteractionPrompt = PROMPT_CLOSE;
    }

    /// <summary>
    /// Closes the door, plays animation and audio, and fires the OnDoorClosed event.
    /// </summary>
    private void CloseDoor()
    {
        _animator.SetBool(OPEN_DOOR, false);
        _animator.SetBool(SLAM_OPEN_DOOR, false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play3DSFX(closeDoorClip, transform.position);

        _currentState = DoorState.Closed;
        OnDoorClosed?.Invoke();
        InteractionPrompt = PROMPT_OPEN;
    }

    /// <summary>
    /// Called when the player interacts with the door.
    /// Opens or closes the door based on its current state and whether the required key is equipped.
    /// </summary>
    /// <remarks>
    /// If the door is locked, it will check the equipped item against <see cref="requiredKey"/>.
    /// If the correct key is equipped, the door unlocks and removes the key from inventory.
    /// </remarks>
    public void Interact()
    {
        if (CurrentState == DoorState.Closed)
        {
            if (!isLocked || (isLocked && playerInventory.HasItem(requiredKey)))
            {
                if (isLocked)
                {
                    isLocked = false;
                    playerInventory.UseOrDropItem(requiredKey, true);
                    requiredKey = null;
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.Play3DSFX(unlockDoorClip, transform.position);
                }
                OpenDoor();
            }
            else
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.Play3DSFX(lockedDoorClip, transform.position);
            }
        }
        else if (CurrentState == DoorState.Opened)
        {
            CloseDoor();
        }
    }

    public void SlamOpenDoor()
    {
        _animator.SetBool(SLAM_OPEN_DOOR, true);

        _currentState = DoorState.Opened;
        OnDoorOpened?.Invoke();
        InteractionPrompt = PROMPT_CLOSE;
    }
}

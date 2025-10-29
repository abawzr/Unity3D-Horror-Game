using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    [SerializeField] private AudioClip openCabinetClip; // Audio played when cabinet opens
    [SerializeField] private AudioClip closeCabinetClip; // Audio played when cabinet closes

    private const string OPEN_CABINET = "Open"; // Animator parameter name
    private const string PROMPT_OPEN = "Open Cabinet";
    private const string PROMPT_CLOSE = "Close Cabinet";

    private Animator _animator;
    private CabinetState _currentState = CabinetState.Closed;

    /// <summary>
    /// Current state of the cabinet (Opened or Closed)
    /// </summary>
    public CabinetState CurrentState => _currentState;

    public enum CabinetState { Opened, Closed };

    public string InteractionPrompt { get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Initialize cabinet as closed
        _currentState = CabinetState.Closed;
        InteractionPrompt = PROMPT_OPEN;
    }

    /// <summary>
    /// Opens the cabinet, plays animation and audio, and fires the OnCabinetOpened event.
    /// </summary>
    private void OpenCabinet()
    {
        _animator.SetBool(OPEN_CABINET, true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play3DSFX(openCabinetClip, transform.position);

        _currentState = CabinetState.Opened;
        InteractionPrompt = PROMPT_CLOSE;
    }

    /// <summary>
    /// Closes the cabinet, plays animation and audio, and fires the OnCabinetClosed event.
    /// </summary>
    private void CloseCabinet()
    {
        _animator.SetBool(OPEN_CABINET, false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play3DSFX(closeCabinetClip, transform.position);

        _currentState = CabinetState.Closed;
        InteractionPrompt = PROMPT_OPEN;
    }

    /// <summary>
    /// Called when the player interacts with the cabinet.
    /// Opens or closes the cabinet based on its current state.
    /// </summary>
    public void Interact()
    {
        if (CurrentState == CabinetState.Closed)
        {
            OpenCabinet();
        }
        else if (CurrentState == CabinetState.Opened)
        {
            CloseCabinet();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEventManager : MonoBehaviour
{
    [Header("Door References")]
    [SerializeField] private Door mainDoor; // The main door that triggers jump scares
    [SerializeField] private List<Door> doors; // List of doors for random knock sounds

    [Header("Audio Clips")]
    [SerializeField] private AudioClip slamOpenMainDoorClip; // Audio clip for slam open main door
    [SerializeField] private List<AudioClip> knockClips; // Audio clips for knocking
    [SerializeField] private List<AudioClip> jumpscareClips; // Audio clips for main door jump scare

    [Header("Timing Settings")]
    [SerializeField] private Vector2 knockIntervalRange = new(5f, 15f); // Min/max interval between random knocks
    [SerializeField] private Vector2 slamOpenMainDoorIntervalRange = new(5f, 15f); // Min/max interval between random slam open main door
    [SerializeField] private float timeBeforeJumpscare; // Delay before playing jump scare
    [SerializeField] private float postJumpscareDelay; // Delay after jump scare (currently unused)

    private Coroutine _knockCoroutine; // Reference to the random knock coroutine
    private Coroutine _jumpscareCoroutine; // Reference to the main door jump scare coroutine
    private Coroutine _slamOpenMainDoorCoroutine;

    public static event Action OnJumpscareOccurred;

    private void OnEnable()
    {
        // Subscribe to main door events
        if (mainDoor != null)
        {
            mainDoor.OnDoorOpened += HandleMainDoorOpened;
            mainDoor.OnDoorClosed += HandleMainDoorClosed;
        }

        // Start random knock coroutine if not already running
        if (_knockCoroutine == null)
            _knockCoroutine = StartCoroutine(PlayRandomKnockSoundOnRandomDoor());

        if (_slamOpenMainDoorCoroutine == null)
            _slamOpenMainDoorCoroutine = StartCoroutine(RandomOpenMainDoor());
    }

    private void OnDisable()
    {
        // Unsubscribe from main door events
        if (mainDoor != null)
        {
            mainDoor.OnDoorOpened -= HandleMainDoorOpened;
            mainDoor.OnDoorClosed -= HandleMainDoorClosed;
        }

        StopAllCoroutines();
        _knockCoroutine = null;
        _jumpscareCoroutine = null;
        _slamOpenMainDoorCoroutine = null;
    }

    // --- Main Door Event Handlers ---

    /// <summary>
    /// Triggered when the main door is opened. Starts the jump scare coroutine if not already running.
    /// </summary>
    private void HandleMainDoorOpened()
    {
        if (_jumpscareCoroutine == null)
            _jumpscareCoroutine = StartCoroutine(PlayJumpscare());
    }

    /// <summary>
    /// Triggered when the main door is closed. Stops the jump scare coroutine if it is running.
    /// </summary>
    private void HandleMainDoorClosed()
    {
        if (_jumpscareCoroutine != null)
        {
            StopCoroutine(_jumpscareCoroutine);
            _jumpscareCoroutine = null;
        }
    }

    // --- Coroutines ---

    /// <summary>
    /// Continuously selects a random closed door and plays a knock sound at random intervals.
    /// </summary>
    private IEnumerator PlayRandomKnockSoundOnRandomDoor()
    {
        while (true)
        {
            if (doors.Count == 0 || knockClips.Count == 0)
            {
                yield return null;
                continue;
            }

            // Pick a random door
            int randomDoorIndex = UnityEngine.Random.Range(0, doors.Count);
            Door randomDoor = doors[randomDoorIndex];

            if (randomDoor.CurrentState == Door.DoorState.Opened)
            {
                yield return null;
                continue;
            }

            float randomTimeBeforeKnockDoor = UnityEngine.Random.Range(knockIntervalRange.x, knockIntervalRange.y);
            randomTimeBeforeKnockDoor = Mathf.Floor(randomTimeBeforeKnockDoor);

            // Pick a random knock clip
            int randomKnockClipIndex = UnityEngine.Random.Range(0, knockClips.Count);
            AudioClip randomKnockClip = knockClips[randomKnockClipIndex];

            float elapsed = 0f;
            while (elapsed < randomTimeBeforeKnockDoor)
            {
                // If door opened, break early and skip this knock attempt
                if (randomDoor.CurrentState == Door.DoorState.Opened)
                {
                    randomDoor = null;
                    break;
                }

                elapsed += Time.deltaTime;
                yield return null; // Wait one frame and continue
            }

            // Only knock if the door is closed
            if (randomDoor != null && randomDoor.CurrentState == Door.DoorState.Closed)
            {
                // Play the knock sound at the door's position
                if (AudioManager.Instance != null)
                    AudioManager.Instance.Play3DSFX(randomKnockClip, randomDoor.transform.position);
            }
            else
            {
                continue;
            }
        }
    }

    /// <summary>
    /// Plays a jump scare audio clip for the main door after a delay.
    /// </summary>
    private IEnumerator PlayJumpscare()
    {
        if (jumpscareClips.Count == 0)
            yield break;

        int randomJumpscareClipIndex = UnityEngine.Random.Range(0, jumpscareClips.Count);
        AudioClip randomJumpscareClip = jumpscareClips[randomJumpscareClipIndex];

        yield return new WaitForSeconds(timeBeforeJumpscare);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play2DSFX(randomJumpscareClip);

        if (InputManager.Instance != null)
            InputManager.Instance.TurnOffInputs();

        yield return new WaitForSeconds(postJumpscareDelay);

        OnJumpscareOccurred?.Invoke();
    }

    private IEnumerator RandomOpenMainDoor()
    {
        while (true)
        {
            if (slamOpenMainDoorClip == null) yield break;

            if (mainDoor.CurrentState == Door.DoorState.Opened)
            {
                yield return null;
                continue;
            }

            float randomTimerBeforeSlamOpenMainDoor = UnityEngine.Random.Range(slamOpenMainDoorIntervalRange.x, slamOpenMainDoorIntervalRange.y);
            randomTimerBeforeSlamOpenMainDoor = Mathf.Floor(randomTimerBeforeSlamOpenMainDoor);

            float elapsed = 0f;
            while (elapsed < randomTimerBeforeSlamOpenMainDoor)
            {
                if (mainDoor.CurrentState == Door.DoorState.Opened)
                {
                    break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (mainDoor.CurrentState == Door.DoorState.Closed)
            {
                mainDoor.SlamOpenDoor();
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Play3DSFX(slamOpenMainDoorClip, mainDoor.transform.position);
                }
            }
            else
            {
                yield return null;
                continue;
            }
        }
    }
}

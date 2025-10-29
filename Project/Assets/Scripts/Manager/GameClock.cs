using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the in-game night cycle, tracking hours and firing events when time changes or night ends.
/// </summary>
public class GameClock : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Number of real-time seconds that pass for one in-game hour.")]
    [SerializeField] private float secondsPerHour = 90f;

    [Tooltip("Total number of in-game hours that make up a night.")]
    [SerializeField] private int hoursPerNight = 6;

    private int _currentHour = 0;
    private Coroutine _clockCoroutine;

    /// <summary>
    /// Fired when the hour changes (e.g., 1 -> 2).
    /// </summary>
    /// <remarks>
    /// Subscribers receive the new hour as an integer.
    /// </remarks>
    public static event Action<int> OnTimeChanged;

    /// <summary>
    /// Fired when the night finishes.
    /// </summary>
    public static event Action OnNightFinished;

    private void OnEnable()
    {
        if (_clockCoroutine == null)
        {
            _currentHour = 0;
            _clockCoroutine = StartCoroutine(RunClock());
        }
    }

    private void OnDisable()
    {
        if (_clockCoroutine != null)
        {
            StopCoroutine(_clockCoroutine);
            _clockCoroutine = null;
        }
    }

    private void Start()
    {
        OnTimeChanged?.Invoke(GetDisplayHour());
    }

    /// <summary>
    /// Runs the in-game clock, incrementing hours over time and firing events.
    /// </summary>
    private IEnumerator RunClock()
    {
        while (_currentHour < hoursPerNight)
        {
            yield return new WaitForSeconds(secondsPerHour);

            _currentHour++;

            OnTimeChanged?.Invoke(GetDisplayHour());
        }
        OnNightFinished?.Invoke();
    }

    /// <summary>
    /// Returns the current hour in a 12-hour display format.
    /// </summary>
    private int GetDisplayHour()
    {
        return (_currentHour == 0) ? 12 : _currentHour;
    }
}

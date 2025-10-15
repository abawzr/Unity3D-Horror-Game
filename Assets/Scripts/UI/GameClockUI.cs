using TMPro;
using UnityEngine;

/// <summary>
/// Updates a UI Text component to display the current hour of the in-game clock.
/// </summary>
/// <remarks>
/// Subscribes to <see cref="GameClock.OnTimeChanged"/> to receive updates when the hour changes.
/// Formats the hour as "HH:00 AM". Currently assumes a 6-hour night cycle.
/// </remarks>
[RequireComponent(typeof(TMP_Text))]
public class GameClockUI : MonoBehaviour
{
    private TMP_Text timeText;

    private void Awake()
    {
        timeText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GameClock.OnTimeChanged += UpdateGameClock;

        UpdateGameClock(12);
    }

    /// <summary>
    /// Updates the displayed hour when the game clock changes.
    /// </summary>
    /// <param name="currentHour">Current hour from GameClock.</param>
    private void UpdateGameClock(int currentHour)
    {
        timeText.text = $"{currentHour:00}:00 AM";
    }
}

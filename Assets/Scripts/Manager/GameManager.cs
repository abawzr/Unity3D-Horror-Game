using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SocialPlatforms;



#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Manages the global game state, frame rate, and cursor behavior.
/// Provides a singleton instance for easy access.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("Target frame rate for the game.")]
    [SerializeField] private int framerate = 120;

    private GameState _currentState;

    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Enumeration of possible game states.
    /// </summary>
    public enum GameState { Playing, Paused, GameOver, MainMenu }

    /// <summary>
    /// Fired whenever the game state changes.
    /// Subscribers can react to state changes.
    /// </summary>
    public event Action<GameState> OnGameStateChanged;

    public event Action<bool> OnGameWin;

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
    }

    private void Start()
    {
        SceneManager.LoadScene("MainMenu");

        SceneManager.sceneLoaded += OnSceneLoaded;

        Application.targetFrameRate = framerate;
        SetGameState(GameState.MainMenu);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            DoorEventManager.OnJumpscareOccurred += GameLose;
            GameClock.OnNightFinished += IsItemsCollected;
            GameObjective.OnObjectiveCompleted += GameWin;
        }

        else
        {
            DoorEventManager.OnJumpscareOccurred -= GameLose;
            GameClock.OnNightFinished -= IsItemsCollected;
            GameObjective.OnObjectiveCompleted -= GameWin;
        }
    }

    private void IsItemsCollected()
    {
        if (GameObjective.TotalItems != GameObjective.CollectedItems)
        {
            GameLose();
        }

        else
        {
            GameWin();
        }
    }

    private void GameWin()
    {
        OnGameWin?.Invoke(true);

        StartCoroutine(AfterWinLoseConditionRoutine());
    }

    private void GameLose()
    {
        OnGameWin?.Invoke(false);

        StartCoroutine(AfterWinLoseConditionRoutine());
    }

    private IEnumerator AfterWinLoseConditionRoutine()
    {
        SetGameState(GameState.GameOver);

        float timer = 3f;
        float elapsed = 0f;

        while (elapsed < timer)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        GameOver();
    }

    private void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
        SetGameState(GameState.MainMenu);
    }

    /// <summary>
    /// Sets the current game state and adjusts time scale, cursor lock, and visibility accordingly.
    /// </summary>
    /// <param name="newState">The new state to switch to.</param>
    public void SetGameState(GameState newState)
    {
        _currentState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                AudioListener.pause = false;
                InputManager.Instance.TurnOnInputs();
                break;

            case GameState.Paused:
            case GameState.GameOver:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                InputManager.Instance.TurnOffInputs();
                AudioListener.pause = true;
                foreach (GameObject localManager in GameObject.FindGameObjectsWithTag("LocalManager"))
                {
                    localManager.SetActive(false);
                }
                break;

            case GameState.MainMenu:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                AudioListener.pause = false;
                InputManager.Instance.TurnOffInputs();
                break;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        SetGameState(GameState.Playing);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stops play mode
#else
        Application.Quit(); // Quits build
#endif
    }
}


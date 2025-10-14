using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


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

    public event Action OnGameWin;
    public event Action OnGameLose;

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
        SceneManager.sceneLoaded += OnSceneLoaded;

        Application.targetFrameRate = framerate;
        SetGameState(GameState.MainMenu);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            DoorEventManager.OnJumpscareOccurred += GameLose;
            GameObjective.OnObjectiveCompleted += GameWin;
        }
    }

    private void GameWin()
    {
        OnGameWin?.Invoke();

        if (InputManager.Instance != null)
        {
            InputManager.Instance.TurnOffInputs();
        }

        StartCoroutine(GameWinRoutine());
    }

    private IEnumerator GameWinRoutine()
    {
        float timer = 3f;
        float elapsed = 0f;

        while (elapsed < timer)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        GameOver();
    }

    private void GameLose()
    {
        OnGameLose?.Invoke();
        GameOver();
    }

    private void GameOver()
    {
        QuitGame();
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
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
            case GameState.GameOver:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0f;
                break;

            case GameState.MainMenu:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 1f;
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


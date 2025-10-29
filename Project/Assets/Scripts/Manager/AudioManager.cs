using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton AudioManager responsible for playing 2D and 3D sound effects and music.
/// Uses a pool for 3D audio sources to allow multiple overlapping 3D sounds at different positions.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [Range(0, 10)][SerializeField] private int masterVolume = 10; // Master volume (0-10)
    [Range(0, 10)][SerializeField] private int soundEffectsVolume = 10; // SFX volume (0-10)
    [Range(0, 10)][SerializeField] private int musicVolume = 10; // Music volume (0-10)

    [Header("3D Audio Settings")]
    [Range(1, 30)][SerializeField] private int sfx3dPoolSize = 10; // Number of 3D audio sources to pool

    [Header("Music Clips")]
    [SerializeField] private AudioClip mainmenuMusic;
    [SerializeField] private AudioClip ambientMusic;

    private const float MAX_VOLUME = 10f;

    private List<AudioSource> _sfx3dPool; // Pool of 3D audio sources for spatial sounds
    private AudioSource _sfx2dSource; // Single audio source for 2D sound effects
    private AudioSource _musicSource; // Single audio source for background music
    private float _musicClipVolume = 0.2f;

    /// <summary>
    /// Singleton instance of AudioManager
    /// </summary>
    public static AudioManager Instance { get; private set; }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged += UpdateMusic;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= UpdateMusic;
    }

    /// <summary>
    /// Initialize the AudioManager singleton and create audio sources
    /// </summary>
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Detach from parent if it exists
        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);

        // Initialize 3D audio pool
        _sfx3dPool = new List<AudioSource>();
        for (int i = 0; i < sfx3dPoolSize; i++)
        {
            GameObject sfxObj = new GameObject($"3D_AudioSource_{i}");
            sfxObj.transform.parent = transform;
            AudioSource source = sfxObj.AddComponent<AudioSource>();

            source.spatialBlend = 1; // 3D Sound
            source.playOnAwake = false;

            _sfx3dPool.Add(source);
        }

        // Create single audio source for music
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.clip = ambientMusic;
        _musicSource.loop = true;
        _musicSource.spatialBlend = 0; // 2D Sound

        // Create single audio source for 2D sound effects
        _sfx2dSource = gameObject.AddComponent<AudioSource>();
        _sfx2dSource.spatialBlend = 0; // 2D SFX Sound
    }

    private void UpdateMusic(GameManager.GameState gameState)
    {
        _musicSource.volume = Mathf.Clamp01(_musicClipVolume * (masterVolume / MAX_VOLUME) * (musicVolume / MAX_VOLUME));
        switch (gameState)
        {
            case GameManager.GameState.Playing:
                _musicSource.clip = ambientMusic;
                _musicSource.Play();
                break;
            case GameManager.GameState.MainMenu:
                _musicSource.clip = mainmenuMusic;
                _musicSource.Play();
                break;
        }
    }

    /// <summary>
    /// Plays a 3D sound effect at the given world position using a pooled AudioSource.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="position">The world position where the sound originates.</param>
    /// <param name="volume">Optional volume multiplier (default = 1).</param>
    public void Play3DSFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = null;
        for (int i = 0; i < _sfx3dPool.Count; i++)
        {
            // Find a free 3D audio source in the pool
            if (!_sfx3dPool[i].isPlaying)
            {
                source = _sfx3dPool[i];
                break;
            }
        }

        // If all sources are busy, use the first one and stop it
        if (source == null)
        {
            source = _sfx3dPool[0];
            source.Stop();
        }

        // Set clip, position, volume, and play
        source.clip = clip;
        source.transform.position = position;
        source.volume = Mathf.Clamp01(volume * (masterVolume / MAX_VOLUME) * (soundEffectsVolume / MAX_VOLUME));
        source.Play();

    }

    /// <summary>
    /// Plays a 2D sound effect using the single 2D AudioSource.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="volume">Optional volume multiplier (default = 1).</param>
    public void Play2DSFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        _sfx2dSource.volume = Mathf.Clamp01(volume * (masterVolume / MAX_VOLUME) * (soundEffectsVolume / MAX_VOLUME));
        _sfx2dSource.PlayOneShot(clip);
    }
}

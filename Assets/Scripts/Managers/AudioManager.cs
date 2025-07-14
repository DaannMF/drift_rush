using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ContinuousAudioType {
    CarIdle,
    CarAccelerate,
    CarBrake,
    CarDrift
}

public class AudioManager : MonoBehaviour {
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;
    [SerializeField] private int sfxSourcesCount = 10;

    private Dictionary<ContinuousAudioType, AudioSource> continuousAudioSources;

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header("UI Sound Effects")]
    [SerializeField] private AudioClip buttonHoverClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip menuOpenClip;
    [SerializeField] private AudioClip menuCloseClip;

    [Header("Car Sound Effects")]
    [SerializeField] private AudioClip carAccelerateClip;
    [SerializeField] private AudioClip carBrakeClip;
    [SerializeField] private AudioClip carIdleClip;
    [SerializeField] private AudioClip carDriftClip;
    [SerializeField] private AudioClip carCrashClip;
    [SerializeField] private AudioClip carResetClip;

    [Header("Game Sound Effects")]
    [SerializeField] private AudioClip coinCollectedClip;
    [SerializeField] private AudioClip levelWinClip;
    [SerializeField] private AudioClip levelLoseClip;
    [SerializeField] private AudioClip countdownClip;

    [Header("Audio Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float uiVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;

    // PlayerPrefs keys for volume persistence
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string UI_VOLUME_KEY = "UIVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private static AudioManager instance;
    private int currentSFXIndex = 0;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();
            InitializeAudioSources();
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void InitializeAudioSources() {
        // Create music source if not assigned
        if (musicSource == null) {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        // Initialize continuous audio sources dictionary
        continuousAudioSources = new Dictionary<ContinuousAudioType, AudioSource>();

        // Create continuous audio sources for each type
        foreach (ContinuousAudioType audioType in System.Enum.GetValues(typeof(ContinuousAudioType))) {
            AudioSource continuousSource = gameObject.AddComponent<AudioSource>();
            continuousSource.loop = true;
            continuousSource.playOnAwake = false;
            continuousAudioSources[audioType] = continuousSource;
        }

        // Create SFX sources pool
        sfxSources = new AudioSource[sfxSourcesCount];
        for (int i = 0; i < sfxSourcesCount; i++) {
            sfxSources[i] = gameObject.AddComponent<AudioSource>();
            sfxSources[i].playOnAwake = false;
        }

        UpdateAllVolumes();
    }

    private void SubscribeToEvents() {
        // UI Events
        UIEvents.onButtonHover += OnButtonHover;
        UIEvents.onButtonClick += OnButtonClick;
        UIEvents.onMenuOpen += OnMenuOpen;
        UIEvents.onMenuClose += OnMenuClose;

        // Car Events
        AudioEvents.onCarAccelerate += OnCarAccelerate;
        AudioEvents.onCarAccelerateStop += OnCarAccelerateStop;
        AudioEvents.onCarBrake += OnCarBrake;
        AudioEvents.onCarBrakeStop += OnCarBrakeStop;
        AudioEvents.onCarIdle += OnCarIdle;
        AudioEvents.onCarIdleStop += OnCarIdleStop;
        AudioEvents.onCarDrift += OnCarDrift;
        AudioEvents.onCarDriftStop += OnCarDriftStop;
        AudioEvents.onCarCrash += OnCarCrash;
        AudioEvents.onCarReset += OnCarReset;
        AudioEvents.onStopAllCarAudio += OnStopAllCarAudio;

        // Game Events
        AudioEvents.onCoinCollected += OnCoinCollected;
        AudioEvents.onLevelWin += OnLevelWin;
        AudioEvents.onLevelLose += OnLevelLose;
        AudioEvents.onCountdown += OnCountdown;

        // Music Events
        AudioEvents.onPlayMenuMusic += OnPlayMenuMusic;
        AudioEvents.onPlayGameMusic += OnPlayGameMusic;
        AudioEvents.onStopMusic += OnStopMusic;

        // Volume Events
        AudioEvents.onSetMasterVolume += OnSetMasterVolume;
        AudioEvents.onSetMusicVolume += OnSetMusicVolume;
        AudioEvents.onSetUIVolume += OnSetUIVolume;
        AudioEvents.onSetSFXVolume += OnSetSFXVolume;

        // Volume Request Events
        AudioEvents.onGetMasterVolume += OnGetMasterVolume;
        AudioEvents.onGetMusicVolume += OnGetMusicVolume;
        AudioEvents.onGetUIVolume += OnGetUIVolume;
        AudioEvents.onGetSFXVolume += OnGetSFXVolume;
    }

    private void UnsubscribeFromEvents() {
        // UI Events
        UIEvents.onButtonHover -= OnButtonHover;
        UIEvents.onButtonClick -= OnButtonClick;
        UIEvents.onMenuOpen -= OnMenuOpen;
        UIEvents.onMenuClose -= OnMenuClose;

        // Car Events
        AudioEvents.onCarAccelerate -= OnCarAccelerate;
        AudioEvents.onCarAccelerateStop -= OnCarAccelerateStop;
        AudioEvents.onCarBrake -= OnCarBrake;
        AudioEvents.onCarBrakeStop -= OnCarBrakeStop;
        AudioEvents.onCarIdle -= OnCarIdle;
        AudioEvents.onCarIdleStop -= OnCarIdleStop;
        AudioEvents.onCarDrift -= OnCarDrift;
        AudioEvents.onCarDriftStop -= OnCarDriftStop;
        AudioEvents.onCarCrash -= OnCarCrash;
        AudioEvents.onCarReset -= OnCarReset;
        AudioEvents.onStopAllCarAudio -= OnStopAllCarAudio;

        // Game Events
        AudioEvents.onCoinCollected -= OnCoinCollected;
        AudioEvents.onLevelWin -= OnLevelWin;
        AudioEvents.onLevelLose -= OnLevelLose;
        AudioEvents.onCountdown -= OnCountdown;

        // Music Events
        AudioEvents.onPlayMenuMusic -= OnPlayMenuMusic;
        AudioEvents.onPlayGameMusic -= OnPlayGameMusic;
        AudioEvents.onStopMusic -= OnStopMusic;

        // Volume Events
        AudioEvents.onSetMasterVolume -= OnSetMasterVolume;
        AudioEvents.onSetMusicVolume -= OnSetMusicVolume;
        AudioEvents.onSetUIVolume -= OnSetUIVolume;
        AudioEvents.onSetSFXVolume -= OnSetSFXVolume;

        // Volume Request Events
        AudioEvents.onGetMasterVolume -= OnGetMasterVolume;
        AudioEvents.onGetMusicVolume -= OnGetMusicVolume;
        AudioEvents.onGetUIVolume -= OnGetUIVolume;
        AudioEvents.onGetSFXVolume -= OnGetSFXVolume;
    }

    // Music Methods
    private void OnPlayMenuMusic() {
        PlayMusic(menuMusic);
    }

    private void OnPlayGameMusic() {
        PlayMusic(gameMusic);
    }

    private void OnStopMusic() {
        if (musicSource != null)
            musicSource.Stop();
    }

    private void PlayMusic(AudioClip clip) {
        if (musicSource != null && clip != null) {
            if (musicSource.clip != clip) {
                musicSource.clip = clip;
                musicSource.Play();
            }
            else if (!musicSource.isPlaying) {
                musicSource.Play();
            }
        }
    }

    // SFX Methods (simplified - no volume multipliers)
    private void PlaySFX(AudioClip clip) {
        if (clip != null && sfxSources != null) {
            AudioSource source = GetAvailableSFXSource();
            if (source != null)
                StartCoroutine(PlaySFXCoroutine(source, clip));
        }
    }

    private IEnumerator PlaySFXCoroutine(AudioSource source, AudioClip clip) {
        source.clip = clip;
        source.volume = sfxVolume * masterVolume;
        source.Play();
        yield return new WaitUntil(() => source.time >= clip.length - 0.1f);
        source.clip = null;
        source.Stop();
    }

    // UI SFX methods (simplified - no volume multipliers)
    private void PlayUISFX(AudioClip clip) {
        if (clip != null && sfxSources != null) {
            AudioSource source = GetAvailableSFXSource();
            if (source != null)
                StartCoroutine(PlayUISFXCoroutine(source, clip));
        }
    }

    private IEnumerator PlayUISFXCoroutine(AudioSource source, AudioClip clip) {
        source.clip = clip;
        source.volume = uiVolume * masterVolume;
        source.Play();
        yield return new WaitUntil(() => source.time >= clip.length - 0.1f);
        source.clip = null;
        source.Stop();
    }

    private AudioSource GetAvailableSFXSource() {
        // Find an available source
        for (int i = 0; i < sfxSources.Length; i++) {
            if (!sfxSources[i].isPlaying)
                return sfxSources[i];
        }

        // If none available, use the next in round-robin
        currentSFXIndex = (currentSFXIndex + 1) % sfxSources.Length;
        return sfxSources[currentSFXIndex];
    }



    // Generic continuous audio control methods (simplified volume)
    private void StartContinuousAudio(ContinuousAudioType audioType, AudioClip clip) {
        if (continuousAudioSources.TryGetValue(audioType, out AudioSource source) && clip != null) {
            if (!source.isPlaying) {
                source.clip = clip;
                source.volume = sfxVolume * masterVolume;
                source.Play();
            }
        }
    }

    private void StopContinuousAudio(ContinuousAudioType audioType) {
        if (continuousAudioSources.TryGetValue(audioType, out AudioSource source) && source.isPlaying) {
            source.Stop();
        }
    }

    private void StopAllContinuousAudio() {
        foreach (var source in continuousAudioSources.Values) {
            if (source.isPlaying) {
                source.Stop();
            }
        }
    }

    // Volume Control (with persistence)
    private void OnSetMasterVolume(float volume) {
        masterVolume = Mathf.Clamp01(volume);
        SaveVolumeSettings();
        UpdateAllVolumes();
    }

    private void OnSetMusicVolume(float volume) {
        musicVolume = Mathf.Clamp01(volume);
        SaveVolumeSettings();
        if (musicSource != null) {
            musicSource.volume = musicVolume * masterVolume;
        }
    }

    private void OnSetUIVolume(float volume) {
        uiVolume = Mathf.Clamp01(volume);
        SaveVolumeSettings();
    }

    private void OnSetSFXVolume(float volume) {
        sfxVolume = Mathf.Clamp01(volume);
        SaveVolumeSettings();

        // Update all continuous audio sources volume
        foreach (var audioSource in continuousAudioSources.Values) {
            audioSource.volume = sfxVolume * masterVolume;
        }
    }

    private void UpdateAllVolumes() {
        // Update music volume
        if (musicSource != null) {
            musicSource.volume = musicVolume * masterVolume;
        }

        // Update continuous audio sources volume
        foreach (var audioSource in continuousAudioSources.Values) {
            audioSource.volume = sfxVolume * masterVolume;
        }
    }

    // Volume Request methods (using callbacks)
    private void OnGetMasterVolume(System.Action<float> callback) {
        callback?.Invoke(masterVolume);
    }

    private void OnGetMusicVolume(System.Action<float> callback) {
        callback?.Invoke(musicVolume);
    }

    private void OnGetUIVolume(System.Action<float> callback) {
        callback?.Invoke(uiVolume);
    }

    private void OnGetSFXVolume(System.Action<float> callback) {
        callback?.Invoke(sfxVolume);
    }

    // Updated Event Handlers (UI sounds use UI volume - no additional multipliers)
    private void OnButtonHover() => PlayUISFX(buttonHoverClip);
    private void OnButtonClick() => PlayUISFX(buttonClickClip);
    private void OnMenuOpen() => PlayUISFX(menuOpenClip);
    private void OnMenuClose() => PlayUISFX(menuCloseClip);

    private void OnCarAccelerate() => StartContinuousAudio(ContinuousAudioType.CarAccelerate, carAccelerateClip);
    private void OnCarAccelerateStop() => StopContinuousAudio(ContinuousAudioType.CarAccelerate);
    private void OnCarBrake() => StartContinuousAudio(ContinuousAudioType.CarBrake, carBrakeClip);
    private void OnCarBrakeStop() => StopContinuousAudio(ContinuousAudioType.CarBrake);
    private void OnCarIdle() => StartContinuousAudio(ContinuousAudioType.CarIdle, carIdleClip);
    private void OnCarIdleStop() => StopContinuousAudio(ContinuousAudioType.CarIdle);
    private void OnCarDrift() => StartContinuousAudio(ContinuousAudioType.CarDrift, carDriftClip);
    private void OnCarDriftStop() => StopContinuousAudio(ContinuousAudioType.CarDrift);
    private void OnCarCrash() => PlaySFX(carCrashClip);

    private void OnCarReset() {
        StopAllContinuousAudio();
        PlaySFX(carResetClip);
    }

    private void OnCoinCollected() => PlaySFX(coinCollectedClip);
    private void OnLevelWin() => PlaySFX(levelWinClip);
    private void OnLevelLose() => PlaySFX(levelLoseClip);
    private void OnCountdown() => PlaySFX(countdownClip);

    private void OnStopAllCarAudio() {
        StopAllContinuousAudio();
    }

    private void LoadVolumeSettings() {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
        uiVolume = PlayerPrefs.GetFloat(UI_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }

    private void SaveVolumeSettings() {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.SetFloat(UI_VOLUME_KEY, uiVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }
}
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

    [Header("Game Sound Effects")]
    [SerializeField] private AudioClip coinCollectedClip;
    [SerializeField] private AudioClip levelWinClip;
    [SerializeField] private AudioClip levelLoseClip;

    [Header("Audio Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float uiVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;



    private int currentSFXIndex = 0;

    private void Awake() {
        InitializeAudioSources();
    }

    private void Start() {
        SubscribeToEvents();

        // Inicializar música del menú principal si estamos en MainMenu
        LevelEvents.onGetIsInMainMenu?.Invoke(isInMainMenu => {
            if (isInMainMenu) {
                OnPlayMenuMusic();
            }
        });
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void InitializeAudioSources() {
        // Cargar clips de música automáticamente si no están asignados
        if (menuMusic == null) {
            menuMusic = Resources.Load<AudioClip>("Art/Audio/menu_loop");
        }
        if (gameMusic == null) {
            gameMusic = Resources.Load<AudioClip>("Art/Audio/game_loop");
        }

        if (musicSource == null) {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        continuousAudioSources = new Dictionary<ContinuousAudioType, AudioSource>();

        foreach (ContinuousAudioType audioType in System.Enum.GetValues(typeof(ContinuousAudioType))) {
            AudioSource continuousSource = gameObject.AddComponent<AudioSource>();
            continuousSource.loop = true;
            continuousSource.playOnAwake = false;
            continuousAudioSources[audioType] = continuousSource;
        }

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
        AudioEvents.onStopAllCarAudio += OnStopAllCarAudio;

        // Game Events
        AudioEvents.onCoinCollected += OnCoinCollected;
        AudioEvents.onLevelWin += OnLevelWin;
        AudioEvents.onLevelLose += OnLevelLose;

        // Music Events
        AudioEvents.onPlayMenuMusic += OnPlayMenuMusic;
        AudioEvents.onPlayGameMusic += OnPlayGameMusic;
        AudioEvents.onStopMusic += OnStopMusic;
        AudioEvents.onPauseMusic += OnPauseMusic;
        AudioEvents.onResumeMusic += OnResumeMusic;

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
        AudioEvents.onStopAllCarAudio -= OnStopAllCarAudio;

        // Game Events
        AudioEvents.onCoinCollected -= OnCoinCollected;
        AudioEvents.onLevelWin -= OnLevelWin;
        AudioEvents.onLevelLose -= OnLevelLose;

        // Music Events
        AudioEvents.onPlayMenuMusic -= OnPlayMenuMusic;
        AudioEvents.onPlayGameMusic -= OnPlayGameMusic;
        AudioEvents.onStopMusic -= OnStopMusic;
        AudioEvents.onPauseMusic -= OnPauseMusic;
        AudioEvents.onResumeMusic -= OnResumeMusic;

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
        if (musicSource != null) {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    private void OnPauseMusic() {
        if (musicSource != null && musicSource.isPlaying) {
            musicSource.Pause();
        }
    }

    private void OnResumeMusic() {
        if (musicSource != null && !musicSource.isPlaying && musicSource.clip != null) {
            musicSource.UnPause();
        }
    }

    private void PlayMusic(AudioClip clip) {
        if (musicSource != null && clip != null) {
            if (musicSource.clip != clip) {
                musicSource.Stop();
                musicSource.clip = clip;
                musicSource.volume = musicVolume * masterVolume;
                musicSource.Play();
            }
            else if (!musicSource.isPlaying) {
                musicSource.Play();
            }
        }
    }

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
        for (int i = 0; i < sfxSources.Length; i++) {
            if (!sfxSources[i].isPlaying)
                return sfxSources[i];
        }

        currentSFXIndex = (currentSFXIndex + 1) % sfxSources.Length;
        return sfxSources[currentSFXIndex];
    }



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

    private void OnSetMasterVolume(float volume) {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    private void OnSetMusicVolume(float volume) {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null) {
            musicSource.volume = musicVolume * masterVolume;
        }
    }

    private void OnSetUIVolume(float volume) {
        uiVolume = Mathf.Clamp01(volume);
    }

    private void OnSetSFXVolume(float volume) {
        sfxVolume = Mathf.Clamp01(volume);

        foreach (var audioSource in continuousAudioSources.Values) {
            audioSource.volume = sfxVolume * masterVolume;
        }
    }

    private void UpdateAllVolumes() {
        if (musicSource != null) {
            musicSource.volume = musicVolume * masterVolume;
        }

        foreach (var audioSource in continuousAudioSources.Values) {
            audioSource.volume = sfxVolume * masterVolume;
        }
    }

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

    private void OnCoinCollected() => PlaySFX(coinCollectedClip);
    private void OnLevelWin() => PlaySFX(levelWinClip);
    private void OnLevelLose() => PlaySFX(levelLoseClip);

    private void OnStopAllCarAudio() {
        StopAllContinuousAudio();
    }


}
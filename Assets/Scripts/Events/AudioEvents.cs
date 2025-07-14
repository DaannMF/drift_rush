using UnityEngine.Events;

/// <summary>
/// Events related to audio system, sound effects, and music control
/// </summary>
public abstract class AudioEvents {
    // Car Sound Events
    public static UnityAction onCarAccelerate;
    public static UnityAction onCarAccelerateStop;
    public static UnityAction onCarBrake;
    public static UnityAction onCarBrakeStop;
    public static UnityAction onCarIdle;
    public static UnityAction onCarIdleStop;
    public static UnityAction onCarDrift;
    public static UnityAction onCarDriftStop;
    public static UnityAction onCarCrash;
    public static UnityAction onCarReset;
    public static UnityAction onStopAllCarAudio; // Stop all continuous car sounds

    // Game Sound Events
    public static UnityAction onCoinCollected;
    public static UnityAction onLevelWin;
    public static UnityAction onLevelLose;
    public static UnityAction onCountdown;

    // Music Control Events
    public static UnityAction onPlayMenuMusic;
    public static UnityAction onPlayGameMusic;
    public static UnityAction onStopMusic;

    // Volume Control Events
    public static UnityAction<float> onSetMasterVolume;
    public static UnityAction<float> onSetMusicVolume;
    public static UnityAction<float> onSetUIVolume;
    public static UnityAction<float> onSetSFXVolume;

    // Volume Request Events (for UI to get current values)
    public static UnityAction<System.Action<float>> onGetMasterVolume;
    public static UnityAction<System.Action<float>> onGetMusicVolume;
    public static UnityAction<System.Action<float>> onGetUIVolume;
    public static UnityAction<System.Action<float>> onGetSFXVolume;
}
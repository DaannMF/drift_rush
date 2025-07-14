using UnityEngine.Events;

public abstract class AudioEvents {
    public static UnityAction onCarAccelerate;
    public static UnityAction onCarAccelerateStop;
    public static UnityAction onCarBrake;
    public static UnityAction onCarBrakeStop;
    public static UnityAction onCarIdle;
    public static UnityAction onCarIdleStop;
    public static UnityAction onCarDrift;
    public static UnityAction onCarDriftStop;
    public static UnityAction onStopAllCarAudio;

    public static UnityAction onCoinCollected;
    public static UnityAction onLevelWin;
    public static UnityAction onLevelLose;

    public static UnityAction onPlayMenuMusic;
    public static UnityAction onPlayGameMusic;
    public static UnityAction onStopMusic;
    public static UnityAction onPauseMusic;
    public static UnityAction onResumeMusic;

    public static UnityAction<float> onSetMasterVolume;
    public static UnityAction<float> onSetMusicVolume;
    public static UnityAction<float> onSetUIVolume;
    public static UnityAction<float> onSetSFXVolume;

    public static UnityAction<System.Action<float>> onGetMasterVolume;
    public static UnityAction<System.Action<float>> onGetMusicVolume;
    public static UnityAction<System.Action<float>> onGetUIVolume;
    public static UnityAction<System.Action<float>> onGetSFXVolume;
}
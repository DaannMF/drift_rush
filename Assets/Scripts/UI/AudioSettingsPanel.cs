using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsPanel : MonoBehaviour {
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start() {
        InitializeSliders();
        LoadVolumeSettings();
    }

    private void OnDestroy() {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);

        if (uiVolumeSlider != null)
            uiVolumeSlider.onValueChanged.RemoveListener(OnUIVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }

    private void InitializeSliders() {
        if (masterVolumeSlider != null) {
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }

        if (musicVolumeSlider != null) {
            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (uiVolumeSlider != null) {
            uiVolumeSlider.minValue = 0f;
            uiVolumeSlider.maxValue = 1f;
            uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        }

        if (sfxVolumeSlider != null) {
            sfxVolumeSlider.minValue = 0f;
            sfxVolumeSlider.maxValue = 1f;
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    }

    private void LoadVolumeSettings() {
        AudioEvents.onGetMasterVolume?.Invoke(volume => {
            if (masterVolumeSlider != null)
                masterVolumeSlider.SetValueWithoutNotify(volume);
        });

        AudioEvents.onGetMusicVolume?.Invoke(volume => {
            if (musicVolumeSlider != null)
                musicVolumeSlider.SetValueWithoutNotify(volume);
        });

        AudioEvents.onGetUIVolume?.Invoke(volume => {
            if (uiVolumeSlider != null)
                uiVolumeSlider.SetValueWithoutNotify(volume);
        });

        AudioEvents.onGetSFXVolume?.Invoke(volume => {
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.SetValueWithoutNotify(volume);
        });
    }

    private void OnMasterVolumeChanged(float value) {
        AudioEvents.onSetMasterVolume?.Invoke(value);
    }

    private void OnMusicVolumeChanged(float value) {
        AudioEvents.onSetMusicVolume?.Invoke(value);
    }

    private void OnUIVolumeChanged(float value) {
        AudioEvents.onSetUIVolume?.Invoke(value);
    }

    private void OnSFXVolumeChanged(float value) {
        AudioEvents.onSetSFXVolume?.Invoke(value);
    }
}
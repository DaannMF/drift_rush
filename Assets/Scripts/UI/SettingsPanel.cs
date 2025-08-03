using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Video settings")]
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Toggle toggle;

    [Header("Tabs")]
    [SerializeField] private GameObject audioTab;
    [SerializeField] private GameObject videoTab;

    private List<string> resolutionOptions = new();
    private Dictionary<string, Resolution> resolutions = new();

    private void OnEnable() {
        audioTab.SetActive(true);
        videoTab.SetActive(false);
    }

    private void Start() {
        InitializeSliders();
        LoadVolumeSettings();
        PopulateDropDown();
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

        if (toggle != null)
            toggle.onValueChanged.RemoveListener(OnFullScreenToggleChanged);
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

        if (toggle != null)
            toggle.onValueChanged.AddListener(OnFullScreenToggleChanged);

        if (dropdown != null)
            dropdown.onValueChanged.AddListener(OnResolutionValueChange);
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

    private void PopulateDropDown() {
        if (dropdown == null) return;

        resolutionOptions.Clear();
        resolutions.Clear();

        foreach (var res in Screen.resolutions) {
            string key = res.width + "x" + res.height;
            if (!resolutions.ContainsKey(key)) {
                resolutionOptions.Add(key);
                resolutions.Add(key, res);
            }
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(resolutionOptions);
    }

    private void OnFullScreenToggleChanged(bool isOn) {
        Debug.Log($"FullScreen changed to : {isOn}");
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, isOn);
    }

    private void OnResolutionValueChange(int value) {
        string key = resolutionOptions[value];
        if (resolutions.TryGetValue(key, out Resolution res)) {
            Debug.Log($"Resolution changed to : {key}");
            Screen.SetResolution(res.width, res.height, toggle.isOn);
        }
        else {
            Debug.LogWarning("Resolution key:{} not found in dictionary");
        }
    }
}
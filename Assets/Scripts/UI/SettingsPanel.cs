using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
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
    private PlayerSettingsSaveData playerSettings;

    private void OnEnable()
    {
        audioTab.SetActive(true);
        videoTab.SetActive(false);
    }

    private void Start()
    {
        InitializeSliders();
        LoadPlayerSettings();
        PopulateDropDown();
        ApplyLoadedSettings();
    }

    private void OnDestroy()
    {
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

    private void InitializeSliders()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (uiVolumeSlider != null)
        {
            uiVolumeSlider.minValue = 0f;
            uiVolumeSlider.maxValue = 1f;
            uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.minValue = 0f;
            sfxVolumeSlider.maxValue = 1f;
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        if (toggle != null)
            toggle.onValueChanged.AddListener(OnFullScreenToggleChanged);

        if (dropdown != null)
            dropdown.onValueChanged.AddListener(OnResolutionValueChange);
    }

    private void LoadPlayerSettings()
    {
        playerSettings = PlayerSettingsSaveData.LoadFromPlayerPrefs();
    }

    private void ApplyLoadedSettings()
    {
        // Apply volume settings to sliders
        if (masterVolumeSlider != null)
            masterVolumeSlider.SetValueWithoutNotify(playerSettings.MasterVolume);

        if (musicVolumeSlider != null)
            musicVolumeSlider.SetValueWithoutNotify(playerSettings.MusicVolume);

        if (uiVolumeSlider != null)
            uiVolumeSlider.SetValueWithoutNotify(playerSettings.UIVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.SetValueWithoutNotify(playerSettings.SFXVolume);

        // Apply resolution settings
        if (toggle != null)
            toggle.SetIsOnWithoutNotify(playerSettings.isFullScreen);

        // Set current resolution in dropdown
        SetCurrentResolutionInDropdown();

        // Apply volume settings to AudioManager
        AudioEvents.onSetMasterVolume?.Invoke(playerSettings.MasterVolume);
        AudioEvents.onSetMusicVolume?.Invoke(playerSettings.MusicVolume);
        AudioEvents.onSetUIVolume?.Invoke(playerSettings.UIVolume);
        AudioEvents.onSetSFXVolume?.Invoke(playerSettings.SFXVolume);

        // Apply resolution and fullscreen
        Resolution currentRes = playerSettings.GetResolution();
        Screen.SetResolution(currentRes.width, currentRes.height, playerSettings.isFullScreen);
    }

    private void SetCurrentResolutionInDropdown()
    {
        if (dropdown == null) return;

        string currentResKey = playerSettings.resolutionWidth + "x" + playerSettings.resolutionHeight;

        for (int i = 0; i < resolutionOptions.Count; i++)
        {
            if (resolutionOptions[i] == currentResKey)
            {
                dropdown.SetValueWithoutNotify(i);
                break;
            }
        }
    }

    private void OnMasterVolumeChanged(float value)
    {
        playerSettings.MasterVolume = value;
        SaveSettings();
        AudioEvents.onSetMasterVolume?.Invoke(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        playerSettings.MusicVolume = value;
        SaveSettings();
        AudioEvents.onSetMusicVolume?.Invoke(value);
    }

    private void OnUIVolumeChanged(float value)
    {
        playerSettings.UIVolume = value;
        SaveSettings();
        AudioEvents.onSetUIVolume?.Invoke(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        playerSettings.SFXVolume = value;
        SaveSettings();
        AudioEvents.onSetSFXVolume?.Invoke(value);
    }

    private void SaveSettings()
    {
        playerSettings.SaveToPlayerPrefs();
    }

    private void PopulateDropDown()
    {
        if (dropdown == null) return;

        resolutionOptions.Clear();
        resolutions.Clear();

        foreach (var res in Screen.resolutions)
        {
            string key = res.width + "x" + res.height;
            if (!resolutions.ContainsKey(key))
            {
                resolutionOptions.Add(key);
                resolutions.Add(key, res);
            }
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(resolutionOptions);
    }

    private void OnFullScreenToggleChanged(bool isOn)
    {
        Debug.Log($"FullScreen changed to : {isOn}");
        playerSettings.isFullScreen = isOn;
        SaveSettings();

        Resolution currentRes = playerSettings.GetResolution();
        Screen.SetResolution(currentRes.width, currentRes.height, isOn);
    }

    private void OnResolutionValueChange(int value)
    {
        string key = resolutionOptions[value];
        if (resolutions.TryGetValue(key, out Resolution res))
        {
            Debug.Log($"Resolution changed to : {key}");
            playerSettings.SetResolution(res);
            SaveSettings();
            Screen.SetResolution(res.width, res.height, toggle.isOn);
        }
        else
        {
            Debug.LogWarning($"Resolution key:{key} not found in dictionary");
        }
    }
}
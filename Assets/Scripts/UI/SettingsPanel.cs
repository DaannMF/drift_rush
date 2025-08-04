using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Video settings")]
    [SerializeField] private TMP_Dropdown resolutionsDropDown;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TMP_Dropdown frameRateDropDown;

    [Header("Tabs")]
    [SerializeField] private GameObject audioTab;
    [SerializeField] private GameObject videoTab;

    private List<string> resolutionOptions = new();
    private Dictionary<string, Resolution> resolutions = new();
    private PlayerSettingsSaveData playerSettings;

    private readonly int[] frameRateOptions = { 30, 60, 120, 0 };
    private readonly string[] frameRateLabels = { "30 FPS", "60 FPS", "120 FPS", "Unlimited" };

    private void OnEnable()
    {
        audioTab.SetActive(true);
        videoTab.SetActive(false);
    }

    private void Start()
    {
        InitializeSliders();
        LoadPlayerSettings();
        PopulateResolutionDropDown();
        PopulateFrameRateDropDown();
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

        if (frameRateDropDown != null)
            frameRateDropDown.onValueChanged.RemoveListener(OnFrameRateChanged);
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

        if (resolutionsDropDown != null)
            resolutionsDropDown.onValueChanged.AddListener(OnResolutionValueChange);

        if (frameRateDropDown != null)
            frameRateDropDown.onValueChanged.AddListener(OnFrameRateChanged);
    }

    private void LoadPlayerSettings()
    {
        playerSettings = PlayerSettingsSaveData.LoadFromPlayerPrefs();
    }

    private void ApplyLoadedSettings()
    {
        // Apply volume settings to sliders (UI only - AudioManager already loaded these)
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

        // Set current frame rate in dropdown
        SetCurrentFrameRateInDropdown();

        // Note: AudioManager and SettingsManager already loaded and applied settings at startup
        // SettingsPanel only needs to update the UI to reflect current settings
        // Video settings are applied by SettingsManager on game start
    }

    private void SetCurrentResolutionInDropdown()
    {
        if (resolutionsDropDown == null) return;

        string currentResKey = playerSettings.resolutionWidth + "x" + playerSettings.resolutionHeight;

        for (int i = 0; i < resolutionOptions.Count; i++)
        {
            if (resolutionOptions[i] == currentResKey)
            {
                resolutionsDropDown.SetValueWithoutNotify(i);
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

    private void PopulateFrameRateDropDown()
    {
        if (frameRateDropDown == null) return;

        frameRateDropDown.ClearOptions();
        frameRateDropDown.AddOptions(new List<string>(frameRateLabels));
        Debug.Log("Frame Rates loaded");
    }

    private void SetCurrentFrameRateInDropdown()
    {
        if (frameRateDropDown == null) return;

        for (int i = 0; i < frameRateOptions.Length; i++)
        {
            if (frameRateOptions[i] == playerSettings.frameRate)
            {
                frameRateDropDown.SetValueWithoutNotify(i);
                break;
            }
        }
    }

    private void OnFrameRateChanged(int value)
    {
        if (value >= 0 && value < frameRateOptions.Length)
        {
            int selectedFrameRate = frameRateOptions[value];
            Debug.Log($"Frame rate changed to: {(selectedFrameRate == 0 ? "Unlimited" : selectedFrameRate + " FPS")}");

            playerSettings.frameRate = selectedFrameRate;
            SaveSettings();

            // Apply frame rate immediately
            Application.targetFrameRate = selectedFrameRate == 0 ? -1 : selectedFrameRate;
        }
    }

    private void PopulateResolutionDropDown()
    {
        if (resolutionsDropDown == null) return;

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

        resolutionsDropDown.ClearOptions();
        resolutionsDropDown.AddOptions(resolutionOptions);
        Debug.Log("Resolutions loaded");
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
using UnityEngine;

public class SettingsManager : MonoBehaviour {

    private static SettingsManager instance = null;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAndApplyAllSettings();
    }

    private void LoadAndApplyAllSettings() {
        PlayerSettingsSaveData settings = PlayerSettingsSaveData.LoadFromPlayerPrefs();
        ApplyVideoSettings(settings);
    }

    private void ApplyVideoSettings(PlayerSettingsSaveData settings) {
        Resolution targetRes = settings.GetResolution();
        Screen.SetResolution(targetRes.width, targetRes.height, settings.isFullScreen);
        Application.targetFrameRate = settings.frameRate == 0 ? -1 : settings.frameRate;
    }
}
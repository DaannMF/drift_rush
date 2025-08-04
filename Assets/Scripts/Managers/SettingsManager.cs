using UnityEngine;

public class SettingsManager : MonoBehaviour {

    private static SettingsManager instance = null;

    private void Awake() {
        // Verificar si ya existe una instancia
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

        // Apply video settings immediately
        ApplyVideoSettings(settings);

        Debug.Log($"Settings loaded and applied at startup: " +
                 $"Resolution={settings.resolutionWidth}x{settings.resolutionHeight}, " +
                 $"FullScreen={settings.isFullScreen}, " +
                 $"FrameRate={settings.frameRate}");
    }

    private void ApplyVideoSettings(PlayerSettingsSaveData settings) {
        // Apply resolution and fullscreen mode
        Resolution targetRes = settings.GetResolution();
        Screen.SetResolution(targetRes.width, targetRes.height, settings.isFullScreen);

        // Apply frame rate
        Application.targetFrameRate = settings.frameRate == 0 ? -1 : settings.frameRate;

        // Apply VSync (optional - you can add this to PlayerSettingsSaveData if needed)
        // QualitySettings.vSyncCount = settings.enableVSync ? 1 : 0;
    }
}
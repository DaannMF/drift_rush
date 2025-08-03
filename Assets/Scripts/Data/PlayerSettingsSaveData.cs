using UnityEngine;

[System.Serializable]
public class PlayerSettingsSaveData
{
    public float MasterVolume = 1f;
    public float MusicVolume = 0.7f;
    public float UIVolume = 1f;
    public float SFXVolume = 1f;
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
    public int refreshRate = 60;
    public bool isFullScreen = true;

    private const string PLAYER_PREFS_KEY = "PlayerSettings";

    // Save settings to PlayerPrefs as JSON
    public void SaveToPlayerPrefs()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PLAYER_PREFS_KEY, json);
        PlayerPrefs.Save();
    }

    // Load settings from PlayerPrefs
    public static PlayerSettingsSaveData LoadFromPlayerPrefs()
    {
        string json = PlayerPrefs.GetString(PLAYER_PREFS_KEY, "");

        if (string.IsNullOrEmpty(json))
        {
            // Return default settings if no saved data exists
            return new PlayerSettingsSaveData();
        }

        return JsonUtility.FromJson<PlayerSettingsSaveData>(json);
    }

    // Get Resolution object from stored values
    public Resolution GetResolution()
    {
        Resolution res = new Resolution();
        res.width = resolutionWidth;
        res.height = resolutionHeight;
        res.refreshRate = refreshRate;
        return res;
    }

    // Set resolution from Resolution object
    public void SetResolution(Resolution resolution)
    {
        resolutionWidth = resolution.width;
        resolutionHeight = resolution.height;
        refreshRate = resolution.refreshRate;
    }
}
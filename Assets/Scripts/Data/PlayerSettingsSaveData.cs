using UnityEngine;

[System.Serializable]
public class PlayerSettingsSaveData {
    public float MasterVolume = 1f;
    public float MusicVolume = 0.7f;
    public float UIVolume = 1f;
    public float SFXVolume = 1f;
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
    public bool isFullScreen = true;

    private const string PLAYER_PREFS_KEY = "PlayerSettings";

    public void SaveToPlayerPrefs() {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PLAYER_PREFS_KEY, json);
        PlayerPrefs.Save();
    }

    public static PlayerSettingsSaveData LoadFromPlayerPrefs() {
        string json = PlayerPrefs.GetString(PLAYER_PREFS_KEY, "");

        if (string.IsNullOrEmpty(json)) {
            return new PlayerSettingsSaveData();
        }

        return JsonUtility.FromJson<PlayerSettingsSaveData>(json);
    }

    public Resolution GetResolution() {
        Resolution res = new Resolution();
        res.width = resolutionWidth;
        res.height = resolutionHeight;
        return res;
    }

    public void SetResolution(Resolution resolution) {
        resolutionWidth = resolution.width;
        resolutionHeight = resolution.height;
    }
}
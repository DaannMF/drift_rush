using UnityEngine;

[System.Serializable]
public abstract class PlayerSettingsSaveData {
    public float MasterVolume;
    public float MusicVolume;
    public float UIVolume;
    public float SFXVolume;
    public Resolution resolution;
}
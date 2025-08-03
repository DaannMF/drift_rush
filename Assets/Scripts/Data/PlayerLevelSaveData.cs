using UnityEngine;
using System;

[System.Serializable]
public class PlayerLevelSaveData
{
    [Header("Save Metadata")]
    public string saveName;
    public string saveDate;
    public string sceneName;

    [Header("Player State")]
    public Vector3 playerPosition;
    public Vector3 playerRotation; // Using Vector3 for easier serialization than Quaternion
    public int coins;
    public float timeRemaining;

    [Header("Level Progress")]
    public int targetCoins;
    public float totalLevelTime;
    public bool isCompleted;

    [Header("Game State")]
    public bool isPaused;
    public float playTimeElapsed;

    public PlayerLevelSaveData()
    {
        // Default constructor for new games
        saveName = "New Game";
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        sceneName = "Level1";
        playerPosition = Vector3.zero;
        playerRotation = Vector3.zero;
        coins = 0;
        timeRemaining = 120f; // Default 2 minutes
        targetCoins = 20;
        totalLevelTime = 120f;
        isCompleted = false;
        isPaused = false;
        playTimeElapsed = 0f;
    }

    public PlayerLevelSaveData(string name, string scene)
    {
        saveName = name;
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        sceneName = scene;
        playerPosition = Vector3.zero;
        playerRotation = Vector3.zero;
        coins = 0;
        timeRemaining = 120f;
        targetCoins = 20;
        totalLevelTime = 120f;
        isCompleted = false;
        isPaused = false;
        playTimeElapsed = 0f;
    }

    // Convert Vector3 rotation to Quaternion
    public Quaternion GetPlayerRotationAsQuaternion()
    {
        return Quaternion.Euler(playerRotation);
    }

    // Set rotation from Quaternion
    public void SetPlayerRotationFromQuaternion(Quaternion rotation)
    {
        playerRotation = rotation.eulerAngles;
    }

    // Get a display-friendly save info
    public string GetSaveDisplayInfo()
    {
        return $"{saveName} - {sceneName} - {saveDate}";
    }

    // Update save date to current time
    public void UpdateSaveDate()
    {
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
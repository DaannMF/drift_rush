using UnityEngine;
using System;

[System.Serializable]
public class PlayerLevelSaveData {
    [Header("Save Metadata")]
    public string id;
    public string saveDate;
    public string sceneName;

    [Header("Player State")]
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public int coins;
    public float timeRemaining;

    [Header("Level Progress")]
    public int targetCoins;
    public float totalLevelTime;
    public bool isCompleted;

    [Header("Game State")]
    public bool isPaused;
    public float playTimeElapsed;

    public PlayerLevelSaveData() {
        if (string.IsNullOrEmpty(id))
            id = System.Guid.NewGuid().ToString();

        if (string.IsNullOrEmpty(saveDate))
            saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (string.IsNullOrEmpty(sceneName))
            sceneName = "Level1";

        // These will be overwritten by JSON if deserializing
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

    public PlayerLevelSaveData(string scene) {
        id = System.Guid.NewGuid().ToString();
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

    public static PlayerLevelSaveData CreateNewSave(string scene = "Level1") {
        return new PlayerLevelSaveData(scene);
    }

    public static PlayerLevelSaveData CreateNewSaveWithLevelData(string scene, int levelTargetCoins, float levelTimeLimit) {
        PlayerLevelSaveData newSave = new PlayerLevelSaveData();
        newSave.id = System.Guid.NewGuid().ToString();
        newSave.saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        newSave.sceneName = scene;
        newSave.playerPosition = Vector3.zero;
        newSave.playerRotation = Vector3.zero;
        newSave.coins = 0;
        newSave.timeRemaining = levelTimeLimit;
        newSave.targetCoins = levelTargetCoins;
        newSave.totalLevelTime = levelTimeLimit;
        newSave.isCompleted = false;
        newSave.isPaused = false;
        newSave.playTimeElapsed = 0f;
        return newSave;
    }

    public Quaternion GetPlayerRotationAsQuaternion() {
        return Quaternion.Euler(playerRotation);
    }

    public void SetPlayerRotationFromQuaternion(Quaternion rotation) {
        playerRotation = rotation.eulerAngles;
    }

    public void UpdateSaveDate() {
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
using UnityEngine;
using System;

[System.Serializable]
public class PlayerLevelSaveData
{
    [Header("Save Metadata")]
    public string id;
    public string saveDate;
    public string sceneName;

    [Header("Player State")]
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public int coins;           // Monedas recolectadas al momento de guardar
    public float timeRemaining; // Tiempo restante al momento de guardar

    public PlayerLevelSaveData()
    {
        // Constructor for JSON deserialization - don't overwrite existing values
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
        if (string.IsNullOrEmpty(saveDate))
        {
            saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        if (string.IsNullOrEmpty(sceneName))
        {
            sceneName = "Level1";
        }

        // Default values - will be overwritten by JSON if deserializing
        playerPosition = Vector3.zero;
        playerRotation = Vector3.zero;
        coins = 0;
        timeRemaining = 0f;
    }

    public PlayerLevelSaveData(string scene, Vector3 position, Vector3 rotation, int currentCoins, float remainingTime)
    {
        id = System.Guid.NewGuid().ToString();
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        sceneName = scene;
        playerPosition = position;
        playerRotation = rotation;
        coins = currentCoins;
        timeRemaining = remainingTime;
    }

    // Create save with current scene data
    public static PlayerLevelSaveData CreateFromCurrentScene(string sceneName)
    {
        // Get player position and rotation from current scene
        Vector3 playerPos = Vector3.zero;
        Vector3 playerRot = Vector3.zero;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPos = player.transform.position;
            playerRot = player.transform.rotation.eulerAngles;
        }

        // Get current game state
        int currentCoins = 0;
        float remainingTime = 0f;

        // These will be set by SaveGameManager using events
        return new PlayerLevelSaveData(sceneName, playerPos, playerRot, currentCoins, remainingTime);
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

    // Update save date to current time
    public void UpdateSaveDate()
    {
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class SaveGameManager : MonoBehaviour {
    private static SaveGameManager instance;
    public static SaveGameManager Instance {
        get {
            if (instance == null) {
                GameObject go = new GameObject("SaveGameManager");
                instance = go.AddComponent<SaveGameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [Header("Save Settings")]
    [SerializeField] private int maxSaveSlots = 10;

    private const string SAVE_LIST_KEY = "SaveGamesList";
    private const string SAVE_DATA_PREFIX = "SaveGame_";
    private const string LAST_SAVE_KEY = "LastSaveIndex";

    private List<Guid> saveGameIds = new();
    private PlayerLevelSaveData currentGameData;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSaveGamesList();
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Create a new game save
    public Guid CreateNewGame() {
        PlayerLevelSaveData newSave = new PlayerLevelSaveData("Level1");
        Guid saveId = newSave.id;

        // Save the game data
        SaveGameData(saveId, newSave);

        // Add to save list
        saveGameIds.Add(saveId);
        SaveSaveGamesList();

        // Set as current game and last save
        currentGameData = newSave;
        PlayerPrefs.SetString(LAST_SAVE_KEY, saveId.ToString());
        PlayerPrefs.Save();

        Debug.Log($"Created new game with ID: {saveId}");
        return saveId;
    }

    // Save current game state
    public void SaveCurrentGame() {
        if (currentGameData == null) {
            Debug.LogWarning("No current game data to save!");
            return;
        }

        // Update current game state from scene
        UpdateCurrentGameDataFromScene();

        // Find the save ID for current game
        Guid currentSaveId = GetCurrentSaveId();
        if (currentSaveId != Guid.Empty) {
            SaveGameData(currentSaveId, currentGameData);
            Debug.Log($"Game saved: {currentGameData.id}");
        }
    }

    // Load a specific save game
    public bool LoadGame(Guid id) {
        PlayerLevelSaveData saveData = LoadGameData(id);
        if (saveData != null) {
            currentGameData = saveData;
            PlayerPrefs.SetString(LAST_SAVE_KEY, id.ToString());
            PlayerPrefs.Save();

            Debug.Log($"Loaded game: {saveData.id}");
            return true;
        }

        Debug.LogError($"Failed to load game with ID: {id}");
        return false;
    }

    // Load the last played game
    public bool LoadLastGame() {
        string lastSaveId = PlayerPrefs.GetString(LAST_SAVE_KEY, "");
        if (!string.IsNullOrEmpty(lastSaveId) && Guid.TryParse(lastSaveId, out Guid lastId) && saveGameIds.Contains(lastId)) {
            return LoadGame(lastId);
        }

        // If no last save or it doesn't exist, load the most recent save
        if (saveGameIds.Count > 0) {
            return LoadGame(saveGameIds.Last());
        }

        Debug.Log("No saved games found to load");
        return false;
    }

    // Delete a save game
    public bool DeleteGame(Guid saveId) {
        if (saveGameIds.Contains(saveId)) {
            saveGameIds.Remove(saveId);
            PlayerPrefs.DeleteKey(SAVE_DATA_PREFIX + saveId);
            SaveSaveGamesList();

            Debug.Log($"Deleted save game: {saveId}");
            return true;
        }

        return false;
    }

    // Get list of all save games
    public List<PlayerLevelSaveData> GetAllSaveGames() {
        List<PlayerLevelSaveData> saves = new List<PlayerLevelSaveData>();

        foreach (Guid saveId in saveGameIds) {
            PlayerLevelSaveData saveData = LoadGameData(saveId);
            if (saveData != null) {
                saves.Add(saveData);
            }
        }

        return saves.OrderByDescending(s => s.saveDate).ToList();
    }

    // Get current game data
    public PlayerLevelSaveData GetCurrentGameData() {
        return currentGameData;
    }

    // Check if there are any saved games
    public bool HasSavedGames() {
        return saveGameIds.Count > 0;
    }

    // Update current game data from the current scene
    private void UpdateCurrentGameDataFromScene() {
        if (currentGameData == null) return;

        // Update scene name
        currentGameData.sceneName = SceneManager.GetActiveScene().name;

        // Update save date
        currentGameData.UpdateSaveDate();

        // Try to get player data from scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            currentGameData.playerPosition = player.transform.position;
            currentGameData.SetPlayerRotationFromQuaternion(player.transform.rotation);
        }

        GameEvents.onGetCurrentCoins?.Invoke(OnCurrentCoinsReceived);
        GameEvents.onGetTimeRemaining?.Invoke(OnRemainingTimeReceived);
    }

    // Apply loaded game data to current scene
    public void ApplyGameDataToScene() {
        if (currentGameData == null) return;

        // Apply player position and rotation
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            player.transform.SetPositionAndRotation(currentGameData.playerPosition, currentGameData.GetPlayerRotationAsQuaternion());
        }


        GameEvents.onSetCurrentCoins?.Invoke(currentGameData.coins);
        GameEvents.onSetTimeRemaining?.Invoke(currentGameData.timeRemaining);

        Debug.Log($"Applied save data to scene: {currentGameData.sceneName}");
    }

    private void SaveGameData(Guid id, PlayerLevelSaveData data) {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_DATA_PREFIX + id.ToString(), json);
        PlayerPrefs.Save();
    }

    private PlayerLevelSaveData LoadGameData(Guid id) {
        string json = PlayerPrefs.GetString(SAVE_DATA_PREFIX + id.ToString(), "");
        if (!string.IsNullOrEmpty(json)) {
            return JsonUtility.FromJson<PlayerLevelSaveData>(json);
        }
        return null;
    }

    private void SaveSaveGamesList() {
        string json = JsonUtility.ToJson(new SaveGamesList { saveIds = saveGameIds });
        PlayerPrefs.SetString(SAVE_LIST_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadSaveGamesList() {
        string json = PlayerPrefs.GetString(SAVE_LIST_KEY, "");
        if (!string.IsNullOrEmpty(json)) {
            SaveGamesList savesList = JsonUtility.FromJson<SaveGamesList>(json);
            saveGameIds = savesList.saveIds ?? new List<Guid>();
        }
    }

    private Guid GetCurrentSaveId() {
        string lastSaveId = PlayerPrefs.GetString(LAST_SAVE_KEY, "");
        if (!string.IsNullOrEmpty(lastSaveId) && Guid.TryParse(lastSaveId, out Guid id) && saveGameIds.Contains(id)) {
            return id;
        }
        return Guid.Empty;
    }

    private void OnCurrentCoinsReceived(int coins) {
        currentGameData.coins = coins;
    }

    private void OnRemainingTimeReceived(float time) {
        currentGameData.timeRemaining = time;
    }
}

[System.Serializable]
public class SaveGamesList {
    public List<Guid> saveIds = new();
}
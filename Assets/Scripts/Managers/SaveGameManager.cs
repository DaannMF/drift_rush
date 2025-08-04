using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class SaveGameManager : MonoBehaviour
{
    [Header("Save Settings")]
    [SerializeField] private int maxSaveSlots = 10;

    private const string SAVE_LIST_KEY = "SaveGamesList";
    private const string SAVE_DATA_PREFIX = "SaveGame_";
    private const string LAST_SAVE_KEY = "LastSaveIndex";

    private List<Guid> saveGameIds = new();
    private PlayerLevelSaveData currentGameData;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadSaveGamesList();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        SaveEvents.onCreateNewGame += OnCreateNewGame;
        SaveEvents.onSaveCurrentGame += SaveCurrentGame;
        SaveEvents.onLoadGame += OnLoadGame;
        SaveEvents.onLoadLastGame += OnLoadLastGame;
        SaveEvents.onDeleteGame += OnDeleteGame;

        SaveEvents.onGetCurrentGameData += OnGetCurrentGameData;
        SaveEvents.onGetAllSaveGames += OnGetAllSaveGames;
        SaveEvents.onHasSavedGames += OnHasSavedGames;

        SaveEvents.onApplySaveDataToScene += ApplyGameDataToScene;
        SaveEvents.onUpdateCurrentGameDataFromScene += UpdateCurrentGameDataFromScene;

        GameEvents.onGetCurrentCoins += OnGetCurrentCoinsForSave;
        GameEvents.onGetTimeRemaining += OnGetTimeRemainingForSave;
    }

    private void UnsubscribeFromEvents()
    {
        SaveEvents.onCreateNewGame -= OnCreateNewGame;
        SaveEvents.onSaveCurrentGame -= SaveCurrentGame;
        SaveEvents.onLoadGame -= OnLoadGame;
        SaveEvents.onLoadLastGame -= OnLoadLastGame;
        SaveEvents.onDeleteGame -= OnDeleteGame;

        SaveEvents.onGetCurrentGameData -= OnGetCurrentGameData;
        SaveEvents.onGetAllSaveGames -= OnGetAllSaveGames;
        SaveEvents.onHasSavedGames -= OnHasSavedGames;

        SaveEvents.onApplySaveDataToScene -= ApplyGameDataToScene;
        SaveEvents.onUpdateCurrentGameDataFromScene -= UpdateCurrentGameDataFromScene;

        GameEvents.onGetCurrentCoins -= OnGetCurrentCoinsForSave;
        GameEvents.onGetTimeRemaining -= OnGetTimeRemainingForSave;
    }

    // Create a new game save
    private void OnCreateNewGame(System.Action<Guid> callback)
    {
        string sceneName = "Level1";

        // Get level data to create save with correct values
        LevelEvents.onGetLevelDataByScene?.Invoke(sceneName, levelData =>
        {
            PlayerLevelSaveData newSave;

            if (levelData != null)
            {
                // Create save with LevelData values
                newSave = PlayerLevelSaveData.CreateNewSaveWithLevelData(
                    sceneName,
                    levelData.TargetCoins,
                    levelData.TimeLimit
                );
                Debug.Log($"Created new save with LevelData: {levelData.TargetCoins} coins, {levelData.TimeLimit}s");
            }
            else
            {
                // Fallback to default values if LevelData not found
                newSave = PlayerLevelSaveData.CreateNewSave(sceneName);
                Debug.LogWarning($"LevelData not found for {sceneName}, using default values");
            }

            // Save the game data
            SaveGameData(newSave);

            // Add to save list
            saveGameIds.Add(Guid.Parse(newSave.id));
            SaveSaveGamesList();

            // Set as current game and last save
            currentGameData = newSave;
            PlayerPrefs.SetString(LAST_SAVE_KEY, newSave.id);
            PlayerPrefs.Save();

            Debug.Log($"Created new game with ID: {newSave.id}");
            callback?.Invoke(Guid.Parse(newSave.id));
        });
    }

    // Save current game state
    public void SaveCurrentGame()
    {
        if (currentGameData == null)
        {
            Debug.LogWarning("No current game data to save!");
            return;
        }

        // Update current game state from scene
        UpdateCurrentGameDataFromScene();

        // Find the save ID for current game
        Guid currentSaveId = GetCurrentSaveId();
        if (currentSaveId != Guid.Empty)
        {
            SaveGameData(currentGameData);
            Debug.Log($"Game saved: {currentGameData.id}");
        }
    }

    // Load a specific save game
    private void OnLoadGame(Guid id)
    {
        PlayerLevelSaveData saveData = LoadGameData(id);
        if (saveData != null)
        {
            currentGameData = saveData;
            PlayerPrefs.SetString(LAST_SAVE_KEY, id.ToString());
            PlayerPrefs.Save();

            Debug.Log($"Loaded game: {saveData.id}");
        }
        else
        {
            Debug.LogError($"Failed to load game with ID: {id}");
        }
    }

    // Load the last played game
    private void OnLoadLastGame()
    {
        string lastSaveId = PlayerPrefs.GetString(LAST_SAVE_KEY, "");
        if (!string.IsNullOrEmpty(lastSaveId) && Guid.TryParse(lastSaveId, out Guid lastId) && saveGameIds.Contains(lastId))
        {
            OnLoadGame(lastId);
            return;
        }

        // If no last save or it doesn't exist, load the most recent save
        if (saveGameIds.Count > 0)
        {
            OnLoadGame(saveGameIds.Last());
            return;
        }

        Debug.Log("No saved games found to load");
    }

    // Delete a save game
    private void OnDeleteGame(Guid saveId)
    {
        if (saveGameIds.Contains(saveId))
        {
            saveGameIds.Remove(saveId);
            PlayerPrefs.DeleteKey(SAVE_DATA_PREFIX + saveId);
            SaveSaveGamesList();

            Debug.Log($"Deleted save game: {saveId}");
        }
    }

    // Get list of all save games
    private void OnGetAllSaveGames(System.Action<List<PlayerLevelSaveData>> callback)
    {
        List<PlayerLevelSaveData> saves = new List<PlayerLevelSaveData>();

        foreach (Guid saveId in saveGameIds)
        {
            PlayerLevelSaveData saveData = LoadGameData(saveId);
            if (saveData != null)
                saves.Add(saveData);
        }

        callback?.Invoke(saves.OrderByDescending(s => s.saveDate).ToList());
    }

    // Get current game data
    private void OnGetCurrentGameData(System.Action<PlayerLevelSaveData> callback)
    {
        callback?.Invoke(currentGameData);
    }

    // Check if there are any saved games
    private void OnHasSavedGames(System.Action<bool> callback)
    {
        callback?.Invoke(saveGameIds.Count > 0);
    }

    // Update current game data from the current scene
    private void UpdateCurrentGameDataFromScene()
    {
        if (currentGameData == null) return;

        // Update scene name
        currentGameData.sceneName = SceneManager.GetActiveScene().name;

        // Update save date
        currentGameData.UpdateSaveDate();

        // Try to get player data from scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            currentGameData.playerPosition = player.transform.position;
            currentGameData.SetPlayerRotationFromQuaternion(player.transform.rotation);
        }

        GameEvents.onGetCurrentCoins?.Invoke(OnCurrentCoinsReceived);
        GameEvents.onGetTimeRemaining?.Invoke(OnRemainingTimeReceived);
    }

    // Apply loaded game data to current scene
    public void ApplyGameDataToScene()
    {
        if (currentGameData == null) return;

        // Apply player position and rotation
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.SetPositionAndRotation(currentGameData.playerPosition, currentGameData.GetPlayerRotationAsQuaternion());
        }

        // Important: Apply save data AFTER level initialization
        // This ensures the HUD shows: currentCoins/targetCoins and currentTime correctly
        GameEvents.onSetCurrentCoins?.Invoke(currentGameData.coins);
        GameEvents.onSetTimeRemaining?.Invoke(currentGameData.timeRemaining);

        // Force UI update to show the correct values
        UIEvents.onForceUIUpdate?.Invoke();

        Debug.Log($"Applied save data to scene: {currentGameData.sceneName} - Coins: {currentGameData.coins}/{currentGameData.targetCoins}, Time: {currentGameData.timeRemaining}");
    }

    private void SaveGameData(PlayerLevelSaveData data)
    {
        data.UpdateSaveDate(); // Update save date before saving
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(SAVE_DATA_PREFIX + data.id, json);
        PlayerPrefs.Save();
        Debug.Log($"Saved game data for ID: {data.id}");
    }

    private PlayerLevelSaveData LoadGameData(Guid id)
    {
        string json = PlayerPrefs.GetString(SAVE_DATA_PREFIX + id.ToString(), "");
        if (!string.IsNullOrEmpty(json))
        {
            PlayerLevelSaveData loadedData = JsonUtility.FromJson<PlayerLevelSaveData>(json);
            Debug.Log($"Loaded game data for ID: {loadedData.id} (requested: {id})");
            return loadedData;
        }
        Debug.LogWarning($"No save data found for ID: {id}");
        return null;
    }

    private void SaveSaveGamesList()
    {
        var stringIds = saveGameIds.Select(guid => guid.ToString()).ToArray();
        var saveGameList = new SaveGamesList { saveIds = stringIds };
        string json = JsonUtility.ToJson(saveGameList, true);
        PlayerPrefs.SetString(SAVE_LIST_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadSaveGamesList()
    {
        string json = PlayerPrefs.GetString(SAVE_LIST_KEY, "");
        if (!string.IsNullOrEmpty(json))
        {
            SaveGamesList savesList = JsonUtility.FromJson<SaveGamesList>(json);
            if (savesList.saveIds != null)
                saveGameIds = savesList.saveIds.Select(id => Guid.Parse(id)).ToList();
            else
                saveGameIds = new List<Guid>();
        }
        else
            saveGameIds = new List<Guid>();
    }

    private Guid GetCurrentSaveId()
    {
        string lastSaveId = PlayerPrefs.GetString(LAST_SAVE_KEY, "");
        if (!string.IsNullOrEmpty(lastSaveId) && Guid.TryParse(lastSaveId, out Guid id) && saveGameIds.Contains(id))
        {
            return id;
        }
        return Guid.Empty;
    }

    private void OnGetCurrentCoinsForSave(System.Action<int> callback)
    {
        callback?.Invoke(currentGameData?.coins ?? 0);
    }

    private void OnGetTimeRemainingForSave(System.Action<float> callback)
    {
        callback?.Invoke(currentGameData?.timeRemaining ?? 0f);
    }

    private void OnCurrentCoinsReceived(int coins)
    {
        if (currentGameData != null)
            currentGameData.coins = coins;
    }

    private void OnRemainingTimeReceived(float time)
    {
        if (currentGameData != null)
            currentGameData.timeRemaining = time;
    }
}

[System.Serializable]
public class SaveGamesList
{
    public string[] saveIds;
}
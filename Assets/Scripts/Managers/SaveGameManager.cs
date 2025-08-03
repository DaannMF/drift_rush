using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    private static SaveGameManager instance;
    public static SaveGameManager Instance
    {
        get
        {
            if (instance == null)
            {
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

    private List<string> saveGameIds = new List<string>();
    private PlayerLevelSaveData currentGameData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSaveGamesList();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region Save Game Management

    // Create a new game save
    public string CreateNewGame(string saveName = "New Game")
    {
        string saveId = System.Guid.NewGuid().ToString();
        PlayerLevelSaveData newSave = new PlayerLevelSaveData(saveName, "Level1");

        // Save the game data
        SaveGameData(saveId, newSave);

        // Add to save list
        saveGameIds.Add(saveId);
        SaveSaveGamesList();

        // Set as current game and last save
        currentGameData = newSave;
        PlayerPrefs.SetString(LAST_SAVE_KEY, saveId);
        PlayerPrefs.Save();

        Debug.Log($"Created new game: {saveName} with ID: {saveId}");
        return saveId;
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
        string currentSaveId = GetCurrentSaveId();
        if (!string.IsNullOrEmpty(currentSaveId))
        {
            SaveGameData(currentSaveId, currentGameData);
            Debug.Log($"Game saved: {currentGameData.saveName}");
        }
    }

    // Load a specific save game
    public bool LoadGame(string saveId)
    {
        PlayerLevelSaveData saveData = LoadGameData(saveId);
        if (saveData != null)
        {
            currentGameData = saveData;
            PlayerPrefs.SetString(LAST_SAVE_KEY, saveId);
            PlayerPrefs.Save();

            Debug.Log($"Loaded game: {saveData.saveName}");
            return true;
        }

        Debug.LogError($"Failed to load game with ID: {saveId}");
        return false;
    }

    // Load the last played game
    public bool LoadLastGame()
    {
        string lastSaveId = PlayerPrefs.GetString(LAST_SAVE_KEY, "");
        if (!string.IsNullOrEmpty(lastSaveId) && saveGameIds.Contains(lastSaveId))
        {
            return LoadGame(lastSaveId);
        }

        // If no last save or it doesn't exist, load the most recent save
        if (saveGameIds.Count > 0)
        {
            return LoadGame(saveGameIds.Last());
        }

        Debug.Log("No saved games found to load");
        return false;
    }

    // Delete a save game
    public bool DeleteGame(string saveId)
    {
        if (saveGameIds.Contains(saveId))
        {
            saveGameIds.Remove(saveId);
            PlayerPrefs.DeleteKey(SAVE_DATA_PREFIX + saveId);
            SaveSaveGamesList();

            Debug.Log($"Deleted save game: {saveId}");
            return true;
        }

        return false;
    }

    #endregion

    #region Data Access

    // Get list of all save games
    public List<PlayerLevelSaveData> GetAllSaveGames()
    {
        List<PlayerLevelSaveData> saves = new List<PlayerLevelSaveData>();

        foreach (string saveId in saveGameIds)
        {
            PlayerLevelSaveData saveData = LoadGameData(saveId);
            if (saveData != null)
            {
                saves.Add(saveData);
            }
        }

        return saves.OrderByDescending(s => s.saveDate).ToList();
    }

    // Get current game data
    public PlayerLevelSaveData GetCurrentGameData()
    {
        return currentGameData;
    }

    // Check if there are any saved games
    public bool HasSavedGames()
    {
        return saveGameIds.Count > 0;
    }

    // Get save game by ID
    public PlayerLevelSaveData GetSaveGameById(string saveId)
    {
        return LoadGameData(saveId);
    }

    #endregion

    #region Scene Integration

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

        // Try to get game state from GameManager
        if (GameManager.Instance != null)
        {
            // This would need to be implemented in GameManager
            // currentGameData.coins = GameManager.Instance.GetCurrentCoins();
            // currentGameData.timeRemaining = GameManager.Instance.GetTimeRemaining();
        }
    }

    // Apply loaded game data to current scene
    public void ApplyGameDataToScene()
    {
        if (currentGameData == null) return;

        // Apply player position and rotation
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = currentGameData.playerPosition;
            player.transform.rotation = currentGameData.GetPlayerRotationAsQuaternion();
        }

        // Apply game state to GameManager
        if (GameManager.Instance != null)
        {
            // This would need to be implemented in GameManager
            // GameManager.Instance.SetCoins(currentGameData.coins);
            // GameManager.Instance.SetTimeRemaining(currentGameData.timeRemaining);
        }

        Debug.Log($"Applied save data to scene: {currentGameData.sceneName}");
    }

    #endregion

    #region Internal Methods

    private void SaveGameData(string saveId, PlayerLevelSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_DATA_PREFIX + saveId, json);
        PlayerPrefs.Save();
    }

    private PlayerLevelSaveData LoadGameData(string saveId)
    {
        string json = PlayerPrefs.GetString(SAVE_DATA_PREFIX + saveId, "");
        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<PlayerLevelSaveData>(json);
        }
        return null;
    }

    private void SaveSaveGamesList()
    {
        string json = JsonUtility.ToJson(new SaveGamesList { saveIds = saveGameIds });
        PlayerPrefs.SetString(SAVE_LIST_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadSaveGamesList()
    {
        string json = PlayerPrefs.GetString(SAVE_LIST_KEY, "");
        if (!string.IsNullOrEmpty(json))
        {
            SaveGamesList savesList = JsonUtility.FromJson<SaveGamesList>(json);
            saveGameIds = savesList.saveIds ?? new List<string>();
        }
    }

    private string GetCurrentSaveId()
    {
        string lastSaveId = PlayerPrefs.GetString(LAST_SAVE_KEY, "");
        if (!string.IsNullOrEmpty(lastSaveId) && saveGameIds.Contains(lastSaveId))
        {
            return lastSaveId;
        }
        return "";
    }

    #endregion
}

[System.Serializable]
public class SaveGamesList
{
    public List<string> saveIds = new List<string>();
}
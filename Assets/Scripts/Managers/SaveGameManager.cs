using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SaveGameManager : MonoBehaviour {
    private const string SAVE_LIST_KEY = "SaveGamesList";
    private const string SAVE_DATA_PREFIX = "SaveGame_";
    private const string LAST_SAVE_KEY = "LastSaveIndex";

    private static SaveGameManager instance = null;
    private List<Guid> saveGameIds = new();
    private PlayerLevelSaveData currentGameData;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSaveGamesList();
    }

    private void Start() {
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        SaveEvents.onCreateNewGame += OnCreateNewGame;
        SaveEvents.onSaveCurrentGame += SaveCurrentGame;
        SaveEvents.onLoadGame += OnLoadGame;
        SaveEvents.onLoadLastGame += OnLoadLastGame;
        SaveEvents.onDeleteGame += OnDeleteGame;

        SaveEvents.onGetCurrentGameData += OnGetCurrentGameData;
        SaveEvents.onGetAllSaveGames += OnGetAllSaveGames;
        SaveEvents.onHasSavedGames += OnHasSavedGames;

        SaveEvents.onApplySaveDataToScene += ApplyGameDataToScene;
    }

    private void UnsubscribeFromEvents() {
        SaveEvents.onCreateNewGame -= OnCreateNewGame;
        SaveEvents.onSaveCurrentGame -= SaveCurrentGame;
        SaveEvents.onLoadGame -= OnLoadGame;
        SaveEvents.onLoadLastGame -= OnLoadLastGame;
        SaveEvents.onDeleteGame -= OnDeleteGame;

        SaveEvents.onGetCurrentGameData -= OnGetCurrentGameData;
        SaveEvents.onGetAllSaveGames -= OnGetAllSaveGames;
        SaveEvents.onHasSavedGames -= OnHasSavedGames;

        SaveEvents.onApplySaveDataToScene -= ApplyGameDataToScene;
    }

    private void OnCreateNewGame(System.Action<Guid> callback) {
        string sceneName = "Level1";

        LevelEvents.onLoadSceneByNameOnly?.Invoke(sceneName);
        StartCoroutine(CreateSaveAfterSceneLoad(sceneName, callback));
    }

    private System.Collections.IEnumerator CreateSaveAfterSceneLoad(string sceneName, System.Action<Guid> callback) {
        yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == sceneName);
        yield return new WaitForEndOfFrame();

        UIEvents.onShowGameUI?.Invoke();
        yield return new WaitForEndOfFrame();

        LevelEvents.onGetLevelDataByScene?.Invoke(sceneName, levelData => {
            float targetTime = levelData != null ? levelData.TimeLimit : 120f;
            int targetCoins = levelData != null ? levelData.TargetCoins : 10;

            PlayerLevelSaveData newSave = PlayerLevelSaveData.CreateFromCurrentScene(sceneName);
            newSave.coins = 0;
            newSave.timeRemaining = targetTime;

            SaveGameData(newSave);

            saveGameIds.Add(Guid.Parse(newSave.id));
            SaveSaveGamesList();

            currentGameData = newSave;
            PlayerPrefs.SetString(LAST_SAVE_KEY, newSave.id);
            PlayerPrefs.Save();

            GameEvents.onInitializeLevel?.Invoke(targetCoins, targetTime);

            callback?.Invoke(Guid.Parse(newSave.id));
        });
    }

    public void SaveCurrentGame() {
        if (currentGameData == null) {
            return;
        }

        currentGameData.saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        currentGameData.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            currentGameData.playerPosition = player.transform.position;
            currentGameData.playerRotation = player.transform.rotation.eulerAngles;
        }

        GameEvents.onGetCurrentCoins?.Invoke(coins => {
            currentGameData.coins = coins;

            GameEvents.onGetTimeRemaining?.Invoke(time => {
                currentGameData.timeRemaining = time;
                SaveGameData(currentGameData);
            });
        });
    }

    private void OnLoadGame(Guid id) {


        PlayerLevelSaveData saveData = LoadGameData(id);
        if (saveData != null) {
            currentGameData = saveData;
            PlayerPrefs.SetString(LAST_SAVE_KEY, id.ToString());
            PlayerPrefs.Save();



            // Step 2: Load the scene from save data (without auto-initialization)
            LevelEvents.onLoadSceneByNameOnly?.Invoke(saveData.sceneName);

            // Step 3: After scene loads, apply save data
            StartCoroutine(ApplySaveAfterSceneLoad());
        }
        else {

        }
    }

    private System.Collections.IEnumerator ApplySaveAfterSceneLoad() {
        if (currentGameData == null) yield break;

        // Wait for scene to load
        yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == currentGameData.sceneName);
        yield return new WaitForEndOfFrame(); // Ensure everything is initialized

        // Step 3: Show game HUD FIRST so it can receive the data updates
        UIEvents.onShowGameUI?.Invoke();

        // Wait one more frame to ensure UI is fully active
        yield return new WaitForEndOfFrame();

        // Step 4: Apply save data to scene (position, coins, time)
        ApplyGameDataToScene();


    }

    // Load the last played game
    private void OnLoadLastGame() {
        string lastSaveId = PlayerPrefs.GetString(LAST_SAVE_KEY, "");
        if (!string.IsNullOrEmpty(lastSaveId) && Guid.TryParse(lastSaveId, out Guid lastId) && saveGameIds.Contains(lastId)) {
            OnLoadGame(lastId);
            return;
        }

        // If no last save or it doesn't exist, load the most recent save
        if (saveGameIds.Count > 0) {
            OnLoadGame(saveGameIds.Last());
            return;
        }
    }

    // Delete a save game
    private void OnDeleteGame(Guid saveId) {
        if (saveGameIds.Contains(saveId)) {
            saveGameIds.Remove(saveId);
            PlayerPrefs.DeleteKey(SAVE_DATA_PREFIX + saveId);
            SaveSaveGamesList();
        }
    }

    // Get list of all save games
    private void OnGetAllSaveGames(System.Action<List<PlayerLevelSaveData>> callback) {
        List<PlayerLevelSaveData> saves = new List<PlayerLevelSaveData>();

        foreach (Guid saveId in saveGameIds) {
            PlayerLevelSaveData saveData = LoadGameData(saveId);
            if (saveData != null)
                saves.Add(saveData);
        }

        callback?.Invoke(saves.OrderByDescending(s => s.saveDate).ToList());
    }

    // Get current game data
    private void OnGetCurrentGameData(System.Action<PlayerLevelSaveData> callback) {
        callback?.Invoke(currentGameData);
    }

    // Check if there are any saved games
    private void OnHasSavedGames(System.Action<bool> callback) {
        callback?.Invoke(saveGameIds.Count > 0);
    }

    // Apply loaded game data to current scene
    public void ApplyGameDataToScene() {
        if (currentGameData == null) return;

        // Apply player position and rotation
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            player.transform.SetPositionAndRotation(currentGameData.playerPosition, currentGameData.GetPlayerRotationAsQuaternion());

        }

        // IMPORTANT: First get level data to set target values correctly
        LevelEvents.onGetLevelDataByScene?.Invoke(currentGameData.sceneName, levelData => {
            if (levelData != null) {
                // Initialize level with correct target values from LevelData FIRST
                GameEvents.onInitializeLevel?.Invoke(levelData.TargetCoins, levelData.TimeLimit);

                // Wait a frame to ensure GameManager processed the initialization
                StartCoroutine(ApplySaveDataAfterInitialization());
            }
            else {

                // Fallback: apply saved values directly
                GameEvents.onSetCurrentCoins?.Invoke(currentGameData.coins);
                GameEvents.onSetTimeRemaining?.Invoke(currentGameData.timeRemaining);
                UIEvents.onForceUIUpdate?.Invoke();
            }
        });
    }

    private System.Collections.IEnumerator ApplySaveDataAfterInitialization() {
        // Wait a frame to ensure GameManager processed the initialization
        yield return new WaitForEndOfFrame();

        // Now apply the saved current values over the initialized values
        GameEvents.onSetCurrentCoins?.Invoke(currentGameData.coins);
        GameEvents.onSetTimeRemaining?.Invoke(currentGameData.timeRemaining);

        // Force UI update to show the correct values
        UIEvents.onForceUIUpdate?.Invoke();
    }

    private void SaveGameData(PlayerLevelSaveData data) {
        data.UpdateSaveDate(); // Update save date before saving
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(SAVE_DATA_PREFIX + data.id, json);
        PlayerPrefs.Save();

    }

    private PlayerLevelSaveData LoadGameData(Guid id) {
        string json = PlayerPrefs.GetString(SAVE_DATA_PREFIX + id.ToString(), "");
        if (!string.IsNullOrEmpty(json)) {
            PlayerLevelSaveData loadedData = JsonUtility.FromJson<PlayerLevelSaveData>(json);
            return loadedData;
        }

        return null;
    }

    private void SaveSaveGamesList() {
        var stringIds = saveGameIds.Select(guid => guid.ToString()).ToArray();
        var saveGameList = new SaveGamesList { saveIds = stringIds };
        string json = JsonUtility.ToJson(saveGameList, true);
        PlayerPrefs.SetString(SAVE_LIST_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadSaveGamesList() {
        string json = PlayerPrefs.GetString(SAVE_LIST_KEY, "");
        if (!string.IsNullOrEmpty(json)) {
            SaveGamesList savesList = JsonUtility.FromJson<SaveGamesList>(json);
            if (savesList.saveIds != null)
                saveGameIds = savesList.saveIds.Select(id => Guid.Parse(id)).ToList();
            else
                saveGameIds = new List<Guid>();
        }
        else
            saveGameIds = new List<Guid>();
    }
}

[System.Serializable]
public class SaveGamesList {
    public string[] saveIds;
}
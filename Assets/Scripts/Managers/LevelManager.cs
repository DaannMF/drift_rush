using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [Header("Level Settings")]
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private static LevelManager instance;

    private int currentLevelIndex = 0;
    private bool isLoading = false;
    private GameObject playerInstance;
    private SpawnPoint currentSpawnPoint;

    public LevelData CurrentLevel => levels != null && currentLevelIndex < levels.Count ? levels[currentLevelIndex] : null;
    public bool IsInMainMenu => SceneManager.GetActiveScene().name == mainMenuSceneName;
    public bool IsInLevel => !IsInMainMenu;
    public int TotalLevels => levels != null ? levels.Count : 0;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        LevelEvents.onRestartLevel += RestartCurrentLevel;

        // LevelManager Events
        LevelEvents.onLoadLevel += LoadLevel;
        LevelEvents.onLoadNextLevel += LoadNextLevel;
        LevelEvents.onLoadMainMenu += LoadMainMenu;

        // LevelManager Queries
        LevelEvents.onGetCurrentLevelIndex += callback => callback?.Invoke(currentLevelIndex);
        LevelEvents.onGetIsLoading += callback => callback?.Invoke(isLoading);
        LevelEvents.onGetIsInMainMenu += callback => callback?.Invoke(IsInMainMenu);
        LevelEvents.onGetIsInLevel += callback => callback?.Invoke(IsInLevel);
        LevelEvents.onGetTotalLevels += callback => callback?.Invoke(TotalLevels);
        LevelEvents.onGetLevelData += OnGetLevelData;

        // Car Reset
        CarEvents.onResetCar += OnResetCar;
    }

    private void OnGetLevelData(System.Action<List<LevelData>> callback) {
        callback?.Invoke(levels);
    }

    private void UnsubscribeFromEvents() {
        LevelEvents.onRestartLevel -= RestartCurrentLevel;

        // LevelManager Events
        LevelEvents.onLoadLevel -= LoadLevel;
        LevelEvents.onLoadNextLevel -= LoadNextLevel;
        LevelEvents.onLoadMainMenu -= LoadMainMenu;

        // LevelManager Queries
        LevelEvents.onGetCurrentLevelIndex -= callback => callback?.Invoke(currentLevelIndex);
        LevelEvents.onGetIsLoading -= callback => callback?.Invoke(isLoading);
        LevelEvents.onGetIsInMainMenu -= callback => callback?.Invoke(IsInMainMenu);
        LevelEvents.onGetIsInLevel -= callback => callback?.Invoke(IsInLevel);
        LevelEvents.onGetTotalLevels -= callback => callback?.Invoke(TotalLevels);
        LevelEvents.onGetLevelData -= callback => callback?.Invoke(levels);

        // Car Reset
        CarEvents.onResetCar -= OnResetCar;
    }

    public void LoadLevel(int levelIndex) {
        if (isLoading || levelIndex >= levels.Count)
            return;

        currentLevelIndex = levelIndex;
        StartCoroutine(LoadLevelAsync());
    }

    public void LoadNextLevel() {
        if (currentLevelIndex + 1 < levels.Count)
            LoadLevel(currentLevelIndex + 1);
    }

    public void RestartCurrentLevel() {
        if (CurrentLevel != null) LoadLevel(currentLevelIndex);
    }

    public void LoadMainMenu() {
        if (isLoading) return;
        StartCoroutine(LoadMainMenuAsync());
    }

    private IEnumerator LoadLevelAsync() {
        isLoading = true;
        LevelEvents.onLevelLoadStarted?.Invoke();

        LevelData levelData = levels[currentLevelIndex];
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelData.SceneName);

        while (!asyncLoad.isDone) {
            LevelEvents.onLevelLoadProgress?.Invoke(asyncLoad.progress);
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        InitializeLevel();
        ConfigureUIForLevel();

        UIEvents.onForceUIUpdate?.Invoke();

        isLoading = false;
        LevelEvents.onLevelLoadCompleted?.Invoke();
    }

    private IEnumerator LoadMainMenuAsync() {
        isLoading = true;
        LevelEvents.onLevelLoadStarted?.Invoke();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuSceneName);

        while (!asyncLoad.isDone) {
            LevelEvents.onLevelLoadProgress?.Invoke(asyncLoad.progress);
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        ConfigureUIForMainMenu();

        UIEvents.onForceUIUpdate?.Invoke();

        isLoading = false;
        LevelEvents.onLevelLoadCompleted?.Invoke();
    }

    private void InitializeLevel() {
        LevelData levelData = CurrentLevel;
        if (levelData == null) return;

        FindAndSetupSpawnPoint();
        InitializePlayer();
        InitializeGameState();
    }

    private void FindAndSetupSpawnPoint() {
        currentSpawnPoint = FindObjectOfType<SpawnPoint>();
        if (currentSpawnPoint == null) {
            GameObject spawnObj = new GameObject("DefaultSpawnPoint");
            currentSpawnPoint = spawnObj.AddComponent<SpawnPoint>();
        }
    }

    private void InitializePlayer() {
        playerInstance = GameObject.FindGameObjectWithTag("Player");
        if (playerInstance == null) return;

        if (currentSpawnPoint != null)
            currentSpawnPoint.SpawnObject(playerInstance);
    }

    private void InitializeGameState() {
        LevelData levelData = CurrentLevel;
        if (levelData == null) return;

        GameEvents.onInitializeLevel?.Invoke(levelData.TargetCoins, levelData.TimeLimit);
    }

    private void ConfigureUIForLevel() {
        UIEvents.onShowGameUI?.Invoke();
        GameEvents.onResumeGame?.Invoke();
    }

    private void ConfigureUIForMainMenu() {
        UIEvents.onShowMainMenu?.Invoke();
        GameEvents.onPauseGame?.Invoke();
    }

    private void OnResetCar() {
        if (playerInstance != null && currentSpawnPoint != null)
            currentSpawnPoint.SpawnObject(playerInstance);
    }
}
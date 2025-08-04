using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [Header("Level Settings")]
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private int currentLevelIndex = 0;
    private bool isLoading = false;
    private GameObject playerInstance;
    private SpawnPoint currentSpawnPoint;

    public LevelData CurrentLevel => levels != null && currentLevelIndex < levels.Count ? levels[currentLevelIndex] : null;
    public bool IsInMainMenu => SceneManager.GetActiveScene().name == mainMenuSceneName;
    public bool IsInLevel => !IsInMainMenu;
    public int TotalLevels => levels != null ? levels.Count : 0;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
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
        LevelEvents.onLoadSceneByName += LoadSceneByName;
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
        LevelEvents.onLoadSceneByName -= LoadSceneByName;
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

    public void LoadSceneByName(string sceneName) {
        if (isLoading) return;
        StartCoroutine(LoadSceneByNameAsync(sceneName));
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

    private IEnumerator LoadSceneByNameAsync(string sceneName) {
        isLoading = true;
        LevelEvents.onLevelLoadStarted?.Invoke();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Simulate progress for smooth loading bar
        float fakeProgress = 0f;
        while (fakeProgress < 0.9f) {
            fakeProgress = Mathf.MoveTowards(fakeProgress, asyncLoad.progress / 0.9f, Time.unscaledDeltaTime * 0.5f);
            LevelEvents.onLevelLoadProgress?.Invoke(fakeProgress);
            yield return null;
        }

        // Complete the loading
        while (fakeProgress < 1f) {
            fakeProgress = Mathf.MoveTowards(fakeProgress, 1f, Time.unscaledDeltaTime * 2f);
            LevelEvents.onLevelLoadProgress?.Invoke(fakeProgress);
            yield return null;
        }

        // Activate scene
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);

        yield return new WaitForEndOfFrame();

        // Apply saved game data if available
        SaveEvents.onApplySaveDataToScene?.Invoke();

        // Configure UI based on scene
        if (sceneName == mainMenuSceneName) {
            ConfigureUIForMainMenu();
        }
        else {
            // Find level data for this scene
            var levelData = levels?.Find(l => l.SceneName == sceneName);
            if (levelData != null) {
                currentLevelIndex = levels.IndexOf(levelData);
                InitializeLevel();
                ConfigureUIForLevel();
            }
        }

        UIEvents.onForceUIUpdate?.Invoke();

        isLoading = false;
        LevelEvents.onLevelLoadCompleted?.Invoke();
    }

    private IEnumerator LoadMainMenuAsync() {
        isLoading = true;
        LevelEvents.onLevelLoadStarted?.Invoke();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuSceneName);
        asyncLoad.allowSceneActivation = false;

        // Simulate progress for smooth loading bar
        float fakeProgress = 0f;
        while (fakeProgress < 0.9f) {
            fakeProgress = Mathf.MoveTowards(fakeProgress, asyncLoad.progress / 0.9f, Time.unscaledDeltaTime * 0.5f);
            LevelEvents.onLevelLoadProgress?.Invoke(fakeProgress);
            yield return null;
        }

        // Complete the loading
        while (fakeProgress < 1f) {
            fakeProgress = Mathf.MoveTowards(fakeProgress, 1f, Time.unscaledDeltaTime * 2f);
            LevelEvents.onLevelLoadProgress?.Invoke(fakeProgress);
            yield return null;
        }

        // Activate scene
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);

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
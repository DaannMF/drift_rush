using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Game Settings")]
    [SerializeField] private int targetCoins = 10;
    [SerializeField] private float timeLimit = 60;

    private bool isGameWon;

    private int currentCoins;
    private float currentTime;
    private bool gameStarted;
    private bool isLevelInitialized;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        SubscribeToEvents();

        if (isLevelInitialized) {
            ConfigureUIForCurrentScene();
        }
    }


    void Update() {
        HandleCountdownTimer();
        HandleCheetCodes();
    }

    private void HandleCheetCodes() {
        if (Input.GetKeyDown(KeyCode.F1))
            WinImmediate();


        if (Input.GetKeyDown(KeyCode.F2))
            LoseImmediate();


        if (Input.GetKeyDown(KeyCode.F3))
            StopTimer();
    }

    private void WinImmediate() {
        isGameWon = true;
        GameEvents.onGameFinished?.Invoke();
    }

    private void LoseImmediate() {
        isGameWon = false;
        GameEvents.onGameFinished?.Invoke();
    }

    private void StopTimer() {
        gameStarted = !gameStarted;
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        GameEvents.onPauseGame += PauseGame;
        GameEvents.onResumeGame += ResumeGame;
        GameEvents.onAddCoin += AddCoin;
        GameEvents.onInitializeLevel += InitializeLevel;
        UIEvents.onForceUIUpdate += ForceUIUpdate;
        GameEvents.onGetIsGameWon += HandleGetIsGameWon;
        GameEvents.onGetCurrentCoins += HandleGetCurrentCoins;
        GameEvents.onGetTimeRemaining += HandleGetRemainingTime;
        GameEvents.onSetCurrentCoins += SetCurrentCoinsFromLoad;
        GameEvents.onSetTimeRemaining += SetRemainingTimeFromLoad;
    }

    private void UnsubscribeFromEvents() {
        GameEvents.onPauseGame -= PauseGame;
        GameEvents.onResumeGame -= ResumeGame;
        GameEvents.onAddCoin -= AddCoin;
        GameEvents.onInitializeLevel -= InitializeLevel;
        UIEvents.onForceUIUpdate -= ForceUIUpdate;
        GameEvents.onGetIsGameWon -= HandleGetIsGameWon;
        GameEvents.onGetCurrentCoins -= HandleGetCurrentCoins;
        GameEvents.onGetTimeRemaining -= HandleGetRemainingTime;
        GameEvents.onSetCurrentCoins -= SetCurrentCoinsFromLoad;
        GameEvents.onSetTimeRemaining -= SetRemainingTimeFromLoad;
    }

    public void InitializeLevel(int levelTargetCoins, float levelTimeLimit) {
        targetCoins = levelTargetCoins;
        timeLimit = levelTimeLimit;

        // Only reset coins and time if this is a fresh level initialization (not loading a save)
        if (currentCoins == 0 && currentTime == 0) {
            currentCoins = 0;
            currentTime = timeLimit;
        }

        // Always update UI with current values
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);
        GameEvents.onCurrentTimeChanged?.Invoke(currentTime);

        gameStarted = false;
        isGameWon = false;
        isLevelInitialized = true;

        ConfigureUIForCurrentScene();
        AudioEvents.onPlayGameMusic?.Invoke();
    }

    private void HandleCountdownTimer() {
        if (!gameStarted) return;

        if (currentTime > 0) {
            currentTime -= Time.deltaTime;
            GameEvents.onCurrentTimeChanged?.Invoke(currentTime);
        }

        if (currentTime <= 0)
            HandleGameCondition();
    }

    private void HandleGameCondition() {
        PauseGame();
        AudioEvents.onStopAllCarAudio?.Invoke();

        isGameWon = currentCoins >= targetCoins;

        if (isGameWon) {
            AudioEvents.onLevelWin?.Invoke();
        }
        else {
            AudioEvents.onLevelLose?.Invoke();
        }

        GameEvents.onGameFinished?.Invoke();
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        gameStarted = false;
        AudioEvents.onStopAllCarAudio?.Invoke();
        AudioEvents.onPauseMusic?.Invoke();
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        ConfigureUIForCurrentScene();
        AudioEvents.onResumeMusic?.Invoke();
    }

    private void ConfigureUIForCurrentScene() {
        LevelEvents.onGetIsInMainMenu?.Invoke(isInMainMenu => {
            if (isInMainMenu) {
                UIEvents.onShowMainMenu?.Invoke();
            }
            else {
                UIEvents.onShowGameUI?.Invoke();
            }
        });
    }

    public void AddCoin() {
        if (currentCoins == 0)
            gameStarted = true;

        currentCoins++;
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);

        if (currentCoins >= targetCoins) {
            HandleGameCondition();
        }
    }

    public void ForceUIUpdate() {
        ConfigureUIForCurrentScene();
    }

    private void HandleGetIsGameWon(System.Action<bool> callback) {
        callback?.Invoke(isGameWon);
    }

    private void HandleGetCurrentCoins(System.Action<int> callback) {
        callback?.Invoke(currentCoins);
    }

    private void HandleGetRemainingTime(System.Action<float> callback) {
        callback?.Invoke(currentTime);
    }

    private void SetCurrentCoinsFromLoad(int coins) {
        currentCoins = coins;
        // Update UI immediately when loading save data
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);
        Debug.Log($"Set coins from save: {currentCoins}/{targetCoins}");
    }

    private void SetRemainingTimeFromLoad(float time) {
        currentTime = time;
        // Update UI immediately when loading save data
        GameEvents.onCurrentTimeChanged?.Invoke(currentTime);
        Debug.Log($"Set time from save: {currentTime}");
    }
}

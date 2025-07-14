using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Game Settings")]
    [SerializeField] private int targetCoins = 10;
    [SerializeField] private float timeLimit = 60;

    private static GameManager instance;

    private bool isGameWon;

    private int currentCoins;
    private float currentTime;
    private bool gameStarted;
    private bool isLevelInitialized;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        SubscribeToEvents();

        if (!isLevelInitialized) {
            InitializeLevel(targetCoins, timeLimit);
        }
        else {
            ConfigureUIForCurrentScene();
        }
    }


    void Update() {
        HandleCountdownTimer();
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
    }

    private void UnsubscribeFromEvents() {
        GameEvents.onPauseGame -= PauseGame;
        GameEvents.onResumeGame -= ResumeGame;
        GameEvents.onAddCoin -= AddCoin;
        GameEvents.onInitializeLevel -= InitializeLevel;
        UIEvents.onForceUIUpdate -= ForceUIUpdate;
        GameEvents.onGetIsGameWon -= HandleGetIsGameWon;
    }

    public void InitializeLevel(int levelTargetCoins, float levelTimeLimit) {
        targetCoins = levelTargetCoins;
        timeLimit = levelTimeLimit;

        currentCoins = 0;
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);
        currentTime = timeLimit;
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
}

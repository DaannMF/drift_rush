using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Game Settings")]
    [SerializeField] private int targetCoins = 10;
    [SerializeField] private float timeLimit = 60;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject endGamePanel;

    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameManager>();
                if (instance == null) {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private bool isGameWon;
    public bool IsGameWon { get => isGameWon; }

    private int currentCoins;
    private float currentTime;
    private bool gameStarted;

    void Start() {
        currentCoins = 0;
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);
        currentTime = timeLimit;
        GameEvents.onCurrentTimeChanged?.Invoke(currentTime);
        gameStarted = false;
        isGameWon = false;
        ResumeGame();
    }

    // Update is called once per frame
    void Update() {
        HandlePauseInput();
        HandleCountdownTimer();
    }

    private void HandlePauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            mainPanel.SetActive(true);
            pausePanel.SetActive(true);
            gamePanel.SetActive(false);
        }
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
        isGameWon = currentCoins >= targetCoins;
        mainPanel.SetActive(true);
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        endGamePanel.SetActive(true);
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        gameStarted = false;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        if (mainPanel) mainPanel.SetActive(false);
        if (gamePanel) gamePanel.SetActive(true);
    }

    public void AddCoin() {
        if (currentCoins == 0)
            gameStarted = true;

        currentCoins++;
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);
    }
}

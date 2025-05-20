using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Game Settings")]
    [SerializeField] private int targetCoins = 10;
    [SerializeField] private float timeLimit = 60;

    [SerializeField] private GameObject pauseMenu;

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

    private int currentCoins;
    private float currentTime;
    private bool gameStarted;

    void Start() {
        currentCoins = 0;
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);
        currentTime = timeLimit;
        GameEvents.onCurrentTimeChanged?.Invoke(currentTime);
    }

    // Update is called once per frame
    void Update() {
        HandlePauseInput();
        HandleCountdownTimer();
    }

    private void HandlePauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseMenu.SetActive(true);
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
        gameStarted = false;

        if (currentCoins >= targetCoins) {
            Debug.Log("You win!");
            // Handle win condition
        }
        else {
            Debug.Log("You lose!");
            // Handle lose condition
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void AddCoin() {
        if (currentCoins == 0)
            gameStarted = true;

        currentCoins++;
        GameEvents.onCurrentCoinsChanged?.Invoke(currentCoins, targetCoins);
    }
}

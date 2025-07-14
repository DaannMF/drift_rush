using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour {
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject winTitle;
    [SerializeField] private GameObject loseTitle;

    void Awake() {
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        if (nextLevelButton != null) {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void Start() {
        GameEvents.onGetIsGameWon?.Invoke(OnGameWonReceived);
    }

    private void OnGameWonReceived(bool isGameWon) {
        if (isGameWon) {
            winTitle.SetActive(true);
            loseTitle.SetActive(false);

            if (nextLevelButton != null) {
                LevelEvents.onGetCurrentLevelIndex?.Invoke(currentLevel => {
                    LevelEvents.onGetTotalLevels?.Invoke(totalLevels => {
                        bool hasNextLevel = currentLevel + 1 < totalLevels;
                        nextLevelButton.gameObject.SetActive(hasNextLevel);
                    });
                });
            }
        }
        else {
            winTitle.SetActive(false);
            loseTitle.SetActive(true);

            if (nextLevelButton != null) {
                nextLevelButton.gameObject.SetActive(false);
            }
        }
    }

    private void OnPlayAgainButtonClicked() {
        LevelEvents.onRestartLevel?.Invoke();
    }

    private void OnNextLevelButtonClicked() {
        LevelEvents.onLoadNextLevel?.Invoke();
    }

    private void OnMainMenuButtonClicked() {
        LevelEvents.onLoadMainMenu?.Invoke();
    }

    private void OnExitButtonClicked() {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log("Exiting game");
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }
}

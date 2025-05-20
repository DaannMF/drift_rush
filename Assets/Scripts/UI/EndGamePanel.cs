using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour {
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject winTitle;
    [SerializeField] private GameObject loseTitle;

    void Awake() {
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    // Start is called before the first frame update
    void Start() {
        // Check if the game is won or lost
        if (GameManager.Instance.IsGameWon) {
            winTitle.SetActive(true);
            loseTitle.SetActive(false);
        }
        else {
            winTitle.SetActive(false);
            loseTitle.SetActive(true);
        }
    }

    private void OnPlayAgainButtonClicked() {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnMainMenuButtonClicked() {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
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

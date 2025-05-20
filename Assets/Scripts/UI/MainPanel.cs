using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {

    [Header("UI Elements")]
    [SerializeField] Button playButton;
    [SerializeField] Button backButton;
    [SerializeField] Button exitButton;

    void Awake() {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);


        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu") {
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
        else
            backButton.gameObject.SetActive(false);
    }

    void OnEnable() {
        GameManager.Instance.PauseGame();
    }

    void OnDestroy() {
        playButton.onClick.RemoveListener(OnPlayButtonClicked);
        exitButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    private void OnPlayButtonClicked() {
        // Check if where are in te game scene
        gameObject.SetActive(false);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level1") {
            GameManager.Instance.ResumeGame();
            return;
        }

        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    private void OnBackButtonClicked() {
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
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {

    [Header("UI Elements")]
    [SerializeField] Button backButton;
    [SerializeField] Button exitButton;

    void Awake() {
        exitButton.onClick.AddListener(OnExitButtonClicked);


        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu") {
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
        else
            backButton.gameObject.SetActive(false);
    }

    void OnEnable() {
        GameEvents.onPauseGame?.Invoke();
    }

    void OnDestroy() {
        exitButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    private void OnBackButtonClicked() {
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
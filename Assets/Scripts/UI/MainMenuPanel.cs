using UnityEngine;
using UnityEngine.UI;

public class MainPanelPanel : MonoBehaviour {

    [Header("UI Elements")]
    [SerializeField] Button backButton;
    [SerializeField] Button exitButton;

    void Awake() {
#if UNITY_WEBGL
        exitButton.gameObject.SetActive(false);
#else
        exitButton.onClick.AddListener(OnExitButtonClicked);
#endif

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu") {
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
        else
            backButton.gameObject.SetActive(false);
    }

    void OnEnable() {
        LevelEvents.onGetIsInLevel?.Invoke(isInLevel => {
            if (isInLevel) {
                GameEvents.onPauseGame?.Invoke();
            }
        });
    }

    void OnDestroy() {
#if !UNITY_WEBGL
        exitButton.onClick.RemoveListener(OnExitButtonClicked);
#endif
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
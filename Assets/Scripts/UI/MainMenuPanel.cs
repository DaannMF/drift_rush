using UnityEngine;
using UnityEngine.UI;

public class MainPanelPanel : MonoBehaviour {

    [Header("UI Elements")]
    [SerializeField] Button playButton;
    [SerializeField] Button backButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button saveButton;

    void OnEnable() {
#if UNITY_WEBGL
        exitButton.gameObject.SetActive(false);
#else
        exitButton.onClick.AddListener(OnExitButtonClicked);
#endif

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu") {
            playButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(OnBackButtonClicked);

            // Show save button only when not in main menu and we have current game data
            if (saveButton != null) {
                SaveEvents.onGetCurrentGameData?.Invoke(currentData => {
                    bool hasSaveData = currentData != null;
                    saveButton.gameObject.SetActive(hasSaveData);
                    if (hasSaveData) {
                        saveButton.onClick.AddListener(OnSaveButtonClicked);
                    }
                });
            }
        }
        else {
            // In MainMenu, hide back button and save button
            playButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            if (saveButton != null) {
                saveButton.gameObject.SetActive(false);
            }
        }

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

        if (backButton != null) {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        if (saveButton != null) {
            saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        }
    }

    private void OnBackButtonClicked() {
        LevelEvents.onLoadMainMenu?.Invoke();
    }

    private void OnSaveButtonClicked() {
        SaveEvents.onSaveCurrentGame?.Invoke();

        // You could show a confirmation message here
        Debug.Log("Game saved successfully!");
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
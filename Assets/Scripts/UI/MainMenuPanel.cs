using UnityEngine;
using UnityEngine.UI;

public class MainPanelPanel : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] Button backButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button saveButton;

    void Awake()
    {
#if UNITY_WEBGL
        exitButton.gameObject.SetActive(false);
#else
        exitButton.onClick.AddListener(OnExitButtonClicked);
#endif

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(OnBackButtonClicked);

            // Show save button only when not in main menu and we have current game data
            if (saveButton != null)
            {
                bool hasSaveData = SaveGameManager.Instance.GetCurrentGameData() != null;
                saveButton.gameObject.SetActive(hasSaveData);
                if (hasSaveData)
                {
                    saveButton.onClick.AddListener(OnSaveButtonClicked);
                }
            }
        }
        else
        {
            backButton.gameObject.SetActive(false);
            if (saveButton != null)
            {
                saveButton.gameObject.SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        LevelEvents.onGetIsInLevel?.Invoke(isInLevel =>
        {
            if (isInLevel)
            {
                GameEvents.onPauseGame?.Invoke();
            }
        });
    }

    void OnDestroy()
    {
#if !UNITY_WEBGL
        exitButton.onClick.RemoveListener(OnExitButtonClicked);
#endif

        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        if (saveButton != null)
        {
            saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        }
    }

    private void OnBackButtonClicked()
    {
        LevelEvents.onLoadMainMenu?.Invoke();
    }

    private void OnSaveButtonClicked()
    {
        SaveGameManager.Instance.SaveCurrentGame();

        // You could show a confirmation message here
        Debug.Log("Game saved successfully!");

        // Optional: Show a brief UI feedback
        ShowSaveConfirmation();
    }

    private void OnExitButtonClicked()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log("Exiting game");
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }

    private void ShowSaveConfirmation()
    {
        // This could trigger a UI animation or message
        // For now, just log the success
        Debug.Log("Save confirmation shown");
    }
}
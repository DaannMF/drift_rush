using UnityEngine;

/// <summary>
/// Manages the UI panels and their visibility
/// </summary>
public class CanvasManager : MonoBehaviour {
    [Header("UI Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject audioSettingsPanel;

    private static CanvasManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SubscribeToEvents();
    }


    private void Update() {
        HandlePauseInput();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        UIEvents.onShowMainMenu += ShowMainMenu;
        UIEvents.onShowGameUI += ShowGameUI;
        UIEvents.onShowPauseMenu += ShowPauseMenu;
        UIEvents.onShowEndGamePanel += ShowEndGamePanel;
        UIEvents.onHideAllPanels += HideAllPanels;
        GameEvents.onGameFinished += OnGameFinished;

        UIEvents.onShowAudioSettings += ShowAudioSettings;
        UIEvents.onHideAudioSettings += HideAudioSettings;
        UIEvents.onToggleAudioSettings += ToggleAudioSettings;

        UIEvents.onSetupMainMenuUI += ShowMainMenu;
        UIEvents.onSetupGameUI += ShowGameUI;
    }

    private void UnsubscribeFromEvents() {
        UIEvents.onShowMainMenu -= ShowMainMenu;
        UIEvents.onShowGameUI -= ShowGameUI;
        UIEvents.onShowPauseMenu -= ShowPauseMenu;
        UIEvents.onShowEndGamePanel -= ShowEndGamePanel;
        UIEvents.onHideAllPanels -= HideAllPanels;
        GameEvents.onGameFinished -= OnGameFinished;

        UIEvents.onShowAudioSettings -= ShowAudioSettings;
        UIEvents.onHideAudioSettings -= HideAudioSettings;
        UIEvents.onToggleAudioSettings -= ToggleAudioSettings;

        UIEvents.onSetupMainMenuUI -= ShowMainMenu;
        UIEvents.onSetupGameUI -= ShowGameUI;
    }

    public void ShowMainMenu() {
        HideAllPanels();
        if (mainPanel) mainPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(true);

        AudioEvents.onPlayMenuMusic?.Invoke();
    }

    public void ShowGameUI() {
        HideAllPanels();
        if (gamePanel) gamePanel.SetActive(true);
    }

    public void ShowPauseMenu() {
        if (mainPanel) mainPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(true);
        if (gamePanel) gamePanel.SetActive(false);
    }

    public void ShowEndGamePanel() {
        HideAllPanels();
        if (mainPanel) mainPanel.SetActive(true);
        if (endGamePanel) endGamePanel.SetActive(true);
    }

    public void ShowAudioSettings() {
        if (audioSettingsPanel != null) {
            audioSettingsPanel.SetActive(true);
        }
    }

    public void HideAudioSettings() {
        if (audioSettingsPanel != null) {
            audioSettingsPanel.SetActive(false);
        }
    }

    public void ToggleAudioSettings() {
        if (audioSettingsPanel != null) {
            bool isActive = audioSettingsPanel.activeSelf;
            audioSettingsPanel.SetActive(!isActive);
        }
    }

    public void HideAllPanels() {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(false);
        if (endGamePanel != null) endGamePanel.SetActive(false);
        if (audioSettingsPanel != null) audioSettingsPanel.SetActive(false);
    }

    private void OnGameFinished() {
        ShowEndGamePanel();
    }

    private void HandlePauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ShowPauseMenu();
        }
    }
}
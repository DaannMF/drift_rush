using UnityEngine;

public class CanvasManager : MonoBehaviour {
    [Header("UI Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject endGamePanel;

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

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        UIEvents.onShowMainMenu += ShowMainMenu;
        UIEvents.onShowMainMenuPanel += ShowMainMenu;
        UIEvents.onShowGameUI += ShowGameUI;
        UIEvents.onShowPauseMenu += ShowPauseMenu;
        UIEvents.onShowEndGamePanel += ShowEndGamePanel;
        UIEvents.onHideAllPanels += HideAllPanels;
        GameEvents.onGameFinished += OnGameFinished;

    }

    private void UnsubscribeFromEvents() {
        UIEvents.onShowMainMenu -= ShowMainMenu;
        UIEvents.onShowMainMenuPanel -= ShowMainMenu;
        UIEvents.onShowGameUI -= ShowGameUI;
        UIEvents.onShowPauseMenu -= ShowPauseMenu;
        UIEvents.onShowEndGamePanel -= ShowEndGamePanel;
        UIEvents.onHideAllPanels -= HideAllPanels;
        GameEvents.onGameFinished -= OnGameFinished;

    }

    public void ShowMainMenu() {
        HideAllPanels();
        if (mainPanel) mainPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(true);

        AudioEvents.onPlayMenuMusic?.Invoke();
    }

    public void ShowGameUI() {
        HideAllPanels();
        if (hudCanvas) hudCanvas.SetActive(true);
    }

    public void ShowPauseMenu() {
        if (mainPanel) mainPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(true);
        if (hudCanvas) hudCanvas.SetActive(false);
    }

    public void ShowEndGamePanel() {
        HideAllPanels();
        if (mainPanel) mainPanel.SetActive(true);
        if (endGamePanel) endGamePanel.SetActive(true);
    }

    public void HideAllPanels() {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (hudCanvas != null) hudCanvas.SetActive(false);
        if (endGamePanel != null) endGamePanel.SetActive(false);
    }

    private void OnGameFinished() {
        ShowEndGamePanel();
    }
}
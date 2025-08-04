using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FakeLoadingBar : MonoBehaviour {
    [Header("Loading Bar References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI loadingMessageText;
    [SerializeField] private GameObject loadingPanel;

    [Header("Loading Messages")]
    [SerializeField]
    private string[] loadingMessages = {
        "Loading world...",
        "Preparing environment...",
        "Starting engines...",
        "Loading game data...",
        "Almost ready..."
    };

    private void Awake() {
        InitializeLoadingBar();
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        LevelEvents.onLevelLoadStarted += ShowLoadingScreen;
        LevelEvents.onLevelLoadCompleted += HideLoadingScreen;
        LevelEvents.onLevelLoadProgress += UpdateProgressFromLevel;
    }

    private void UnsubscribeFromEvents() {
        LevelEvents.onLevelLoadStarted -= ShowLoadingScreen;
        LevelEvents.onLevelLoadCompleted -= HideLoadingScreen;
        LevelEvents.onLevelLoadProgress -= UpdateProgressFromLevel;
    }

    private void InitializeLoadingBar() {
        if (loadingPanel != null) {
            loadingPanel.SetActive(false);
        }

        if (progressBar != null) {
            progressBar.value = 0f;
        }

        if (progressText != null) {
            progressText.text = "0%";
        }
    }

    public void ShowLoadingScreen() {
        if (loadingPanel != null) {
            loadingPanel.SetActive(true);
        }

        ResetProgressBar();
    }

    public void HideLoadingScreen() {
        if (loadingPanel != null) {
            loadingPanel.SetActive(false);
        }
    }

    private void UpdateProgressFromLevel(float progress) {
        UpdateProgressBar(progress);
    }

    private void UpdateProgressBar(float progress) {
        progress = Mathf.Clamp01(progress);

        if (progressBar != null) {
            progressBar.value = progress;
        }

        if (progressText != null) {
            progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }
    }

    private void UpdateLoadingMessage(string message) {
        if (loadingMessageText != null) {
            loadingMessageText.text = message;
        }
    }

    private void ResetProgressBar() {
        UpdateProgressBar(0f);
        if (loadingMessages.Length > 0) {
            UpdateLoadingMessage(loadingMessages[0]);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : MonoBehaviour {
    [Header("Main Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadGameButton;

    void Awake() {
        SetupButtonListeners();
    }

    void OnEnable() {
        UpdateButtonStates();
    }

    void OnDestroy() {
        CleanupButtonListeners();
    }

    private void SetupButtonListeners() {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameButtonClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void CleanupButtonListeners() {
        if (newGameButton != null)
            newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);

        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
    }

    private void OnNewGameButtonClicked() {
        SaveEvents.onCreateNewGame?.Invoke(saveId => {
        });
    }

    private void OnContinueButtonClicked() {
        SaveEvents.onLoadLastGame?.Invoke();
    }

    public void UpdateButtonStates() {
        SaveEvents.onHasSavedGames?.Invoke(hasSavedGames => {
            if (continueButton != null) {
                continueButton.interactable = hasSavedGames;
            }

            if (loadGameButton != null) {
                loadGameButton.interactable = hasSavedGames;
            }
        });
    }
}
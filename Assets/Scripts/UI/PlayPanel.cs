using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PlayPanel : MonoBehaviour {
    [Header("Main Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button backButton;


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
            if (saveId != Guid.Empty) {
                Debug.Log($"New game created with id {saveId}");
            }
        });
    }

    private void OnContinueButtonClicked() {
        // SaveGameManager handles the complete loading process including scene loading
        SaveEvents.onLoadLastGame?.Invoke();
        // No need for StartLoadedGame() - SaveGameManager handles everything
    }

    private void OnBackButtonClicked() {
        UIEvents.onShowMainMenuPanel?.Invoke();
    }



    private void UpdateButtonStates() {
        SaveEvents.onHasSavedGames?.Invoke(hasSavedGames => {
            if (continueButton != null) {
                continueButton.interactable = hasSavedGames;
            }

            if (loadGameButton != null) {
                loadGameButton.interactable = hasSavedGames;
            }
        });
    }

    public void RefreshButtonStates() {
        UpdateButtonStates();
    }
}
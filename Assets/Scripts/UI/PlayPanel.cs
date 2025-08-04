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

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void CleanupButtonListeners() {
        if (newGameButton != null)
            newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);

        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);

        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);

    }

    private void OnNewGameButtonClicked() {
        Guid saveId = SaveGameManager.Instance.CreateNewGame();
        if (saveId != Guid.Empty) {
            StartNewGame();
        }
    }

    private void OnContinueButtonClicked() {
        if (SaveGameManager.Instance.HasSavedGames()) {
            if (SaveGameManager.Instance.LoadLastGame()) {
                StartLoadedGame();
            }
            else {
                Debug.LogError("Failed to load last game");
            }
        }
        else {
            Debug.Log("No saved games to continue");
        }
    }

    private void OnBackButtonClicked() {
        UIEvents.onShowMainMenuPanel?.Invoke();
    }

    private void StartNewGame() {
        // Start loading to Level1
        LoadingManager.Instance.LoadSceneWithLoading("Level1");
    }

    private void StartLoadedGame() {
        PlayerLevelSaveData currentData = SaveGameManager.Instance.GetCurrentGameData();
        if (currentData != null) {
            // Load the scene from the save data
            LoadingManager.Instance.LoadSceneWithLoading(currentData.sceneName);
        }
    }

    private void UpdateButtonStates() {
        bool hasSavedGames = SaveGameManager.Instance.HasSavedGames();

        if (continueButton != null) {
            continueButton.interactable = hasSavedGames;
        }

        if (loadGameButton != null) {
            loadGameButton.interactable = hasSavedGames;
        }
    }

    public void RefreshButtonStates() {
        UpdateButtonStates();
    }
}
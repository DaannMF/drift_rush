using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PlayPanel : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button backButton;

    [Header("Load Game Panel")]
    [SerializeField] private LoadGamePanel loadGamePanel;

    void Awake()
    {
        SetupButtonListeners();
    }

    void OnEnable()
    {
        InitializeLoadGamePanel();
        UpdateButtonStates();
        HideSubPanels();
    }

    void OnDestroy()
    {
        CleanupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameButtonClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueButtonClicked);

        if (loadGameButton != null)
            loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void CleanupButtonListeners()
    {
        if (newGameButton != null)
            newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);

        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);

        if (loadGameButton != null)
            loadGameButton.onClick.RemoveListener(OnLoadGameButtonClicked);

        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);

    }

    #region Button Handlers

    private void OnNewGameButtonClicked()
    {
        Guid saveId = SaveGameManager.Instance.CreateNewGame();
        if (saveId != Guid.Empty)
        {
            StartNewGame();
        }
    }

    private void OnContinueButtonClicked()
    {
        if (SaveGameManager.Instance.HasSavedGames())
        {
            if (SaveGameManager.Instance.LoadLastGame())
            {
                StartLoadedGame();
            }
            else
            {
                Debug.LogError("Failed to load last game");
            }
        }
        else
        {
            Debug.Log("No saved games to continue");
        }
    }

    private void OnLoadGameButtonClicked()
    {
        if (loadGamePanel != null)
        {
            loadGamePanel.ShowPanel();
        }
    }

    private void OnBackButtonClicked()
    {
        UIEvents.onShowMainMenuPanel?.Invoke();
    }

    #endregion

    #region Panel Management

    private void InitializeLoadGamePanel()
    {
        if (loadGamePanel != null)
        {
            loadGamePanel.Initialize(OnGameSelected, OnLoadGamePanelBack);
        }
    }

    private void HideSubPanels()
    {
        if (loadGamePanel != null)
            loadGamePanel.HidePanel();
    }

    private void OnGameSelected(PlayerLevelSaveData saveData)
    {
        StartLoadedGame();
    }

    private void OnLoadGamePanelBack()
    {
        if (loadGamePanel != null)
        {
            loadGamePanel.HidePanel();
        }
    }

    #endregion

    #region Save Games Management - Delegated to LoadGamePanel

    // Save games list management is now handled by LoadGamePanel
    // This region is kept for backwards compatibility and future extensions

    #endregion

    #region Game Start

    private void StartNewGame()
    {
        // Start loading to Level1
        LoadingManager.Instance.LoadSceneWithLoading("Level1");
    }

    private void StartLoadedGame()
    {
        PlayerLevelSaveData currentData = SaveGameManager.Instance.GetCurrentGameData();
        if (currentData != null)
        {
            // Load the scene from the save data
            LoadingManager.Instance.LoadSceneWithLoading(currentData.sceneName);
        }
    }

    #endregion

    #region UI State Management

    private void UpdateButtonStates()
    {
        bool hasSavedGames = SaveGameManager.Instance.HasSavedGames();

        if (continueButton != null)
        {
            continueButton.interactable = hasSavedGames;
        }

        if (loadGameButton != null)
        {
            loadGameButton.interactable = hasSavedGames;
        }
    }

    public void RefreshButtonStates()
    {
        UpdateButtonStates();
    }

    #endregion
}
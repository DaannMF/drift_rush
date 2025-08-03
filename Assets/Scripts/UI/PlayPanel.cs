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

    [Header("Load Game Panel")]
    [SerializeField] private GameObject loadGamePanel;


    private List<GameObject> saveGameItems = new List<GameObject>();

    void Awake() {
        SetupButtonListeners();
    }

    void OnEnable() {
        UpdateContinueButtonState();
        HideSubPanels();
    }

    void OnDestroy() {
        CleanupButtonListeners();
    }

    private void SetupButtonListeners() {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameButtonClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueButtonClicked);

        if (loadGameButton != null)
            loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void CleanupButtonListeners() {
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

    private void OnLoadGameButtonClicked() {
        ShowLoadGamePanel();
    }

    private void OnBackButtonClicked() {
        UIEvents.onShowMainMenuPanel?.Invoke();
    }

    private void OnLoadPanelBackButtonClicked() {
        HideLoadGamePanel();
    }

    #endregion

    #region Panel Management

    private void HideSubPanels() {
        if (loadGamePanel != null)
            loadGamePanel.SetActive(false);
    }

    private void ShowLoadGamePanel() {
        HideSubPanels();
        if (loadGamePanel != null) {
            loadGamePanel.SetActive(true);
            PopulateSaveGamesList();
        }
    }

    private void HideLoadGamePanel() {
        if (loadGamePanel != null)
            loadGamePanel.SetActive(false);
        ClearSaveGamesList();
    }

    #endregion

    #region Save Games List Management

    private void PopulateSaveGamesList() {
        ClearSaveGamesList();

        List<PlayerLevelSaveData> saveGames = SaveGameManager.Instance.GetAllSaveGames();

        foreach (PlayerLevelSaveData saveData in saveGames) {
            CreateSaveGameItem(saveData);
        }
    }

    private void CreateSaveGameItem(PlayerLevelSaveData saveData) {
        if (saveGameItemPrefab == null || saveGameContainer == null) return;

        GameObject itemObj = Instantiate(saveGameItemPrefab, saveGameContainer);
        SaveGameItem saveItem = itemObj.GetComponent<SaveGameItem>();

        if (saveItem != null) {
            saveItem.Initialize(saveData, OnSaveGameSelected, OnSaveGameDeleted);
        }

        saveGameItems.Add(itemObj);
    }

    private void ClearSaveGamesList() {
        foreach (GameObject item in saveGameItems) {
            if (item != null) {
                Destroy(item);
            }
        }
        saveGameItems.Clear();
    }

    private void OnSaveGameSelected(PlayerLevelSaveData saveData) {
        // Find the save ID for this save data
        List<PlayerLevelSaveData> allSaves = SaveGameManager.Instance.GetAllSaveGames();
        for (int i = 0; i < allSaves.Count; i++) {
            if (allSaves[i].id == saveData.id && allSaves[i].saveDate == saveData.saveDate) {
                if (SaveGameManager.Instance.LoadGame(SaveGameManager.Instance.GetAllSaveGames()[i].id)) {
                    StartLoadedGame();
                }
                break;
            }
        }
    }

    private void OnSaveGameDeleted(PlayerLevelSaveData saveData) {
        PopulateSaveGamesList();
        UpdateContinueButtonState();
    }

    #endregion

    #region Game Start

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

    #endregion

    #region UI State Management

    private void UpdateContinueButtonState() {
        if (continueButton != null) {
            bool hasSavedGames = SaveGameManager.Instance.HasSavedGames();
            continueButton.gameObject.SetActive(hasSavedGames);
        }
    }

    #endregion
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayPanel : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button backButton;

    [Header("Load Game Panel")]
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private Transform saveGameContainer;
    [SerializeField] private GameObject saveGameItemPrefab;
    [SerializeField] private Button loadPanelBackButton;

    [Header("New Game Panel")]
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private TMP_InputField saveNameInput;
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button newGameBackButton;

    private List<GameObject> saveGameItems = new List<GameObject>();

    void Awake()
    {
        SetupButtonListeners();
    }

    void OnEnable()
    {
        UpdateContinueButtonState();
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

        if (loadPanelBackButton != null)
            loadPanelBackButton.onClick.AddListener(OnLoadPanelBackButtonClicked);

        if (createGameButton != null)
            createGameButton.onClick.AddListener(OnCreateGameButtonClicked);

        if (newGameBackButton != null)
            newGameBackButton.onClick.AddListener(OnNewGameBackButtonClicked);
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

        if (loadPanelBackButton != null)
            loadPanelBackButton.onClick.RemoveListener(OnLoadPanelBackButtonClicked);

        if (createGameButton != null)
            createGameButton.onClick.RemoveListener(OnCreateGameButtonClicked);

        if (newGameBackButton != null)
            newGameBackButton.onClick.RemoveListener(OnNewGameBackButtonClicked);
    }

    #region Button Handlers

    private void OnNewGameButtonClicked()
    {
        ShowNewGamePanel();
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
        ShowLoadGamePanel();
    }

    private void OnBackButtonClicked()
    {
        UIEvents.onShowMainMenuPanel?.Invoke();
    }

    private void OnLoadPanelBackButtonClicked()
    {
        HideLoadGamePanel();
    }

    private void OnCreateGameButtonClicked()
    {
        string saveName = saveNameInput.text;
        if (string.IsNullOrEmpty(saveName))
        {
            saveName = "New Game";
        }

        string saveId = SaveGameManager.Instance.CreateNewGame(saveName);
        if (!string.IsNullOrEmpty(saveId))
        {
            StartNewGame();
        }
    }

    private void OnNewGameBackButtonClicked()
    {
        HideNewGamePanel();
    }

    #endregion

    #region Panel Management

    private void HideSubPanels()
    {
        if (loadGamePanel != null)
            loadGamePanel.SetActive(false);

        if (newGamePanel != null)
            newGamePanel.SetActive(false);
    }

    private void ShowNewGamePanel()
    {
        HideSubPanels();
        if (newGamePanel != null)
        {
            newGamePanel.SetActive(true);
            if (saveNameInput != null)
            {
                saveNameInput.text = "New Game";
                saveNameInput.Select();
            }
        }
    }

    private void HideNewGamePanel()
    {
        if (newGamePanel != null)
            newGamePanel.SetActive(false);
    }

    private void ShowLoadGamePanel()
    {
        HideSubPanels();
        if (loadGamePanel != null)
        {
            loadGamePanel.SetActive(true);
            PopulateSaveGamesList();
        }
    }

    private void HideLoadGamePanel()
    {
        if (loadGamePanel != null)
            loadGamePanel.SetActive(false);
        ClearSaveGamesList();
    }

    #endregion

    #region Save Games List Management

    private void PopulateSaveGamesList()
    {
        ClearSaveGamesList();

        List<PlayerLevelSaveData> saveGames = SaveGameManager.Instance.GetAllSaveGames();

        foreach (PlayerLevelSaveData saveData in saveGames)
        {
            CreateSaveGameItem(saveData);
        }
    }

    private void CreateSaveGameItem(PlayerLevelSaveData saveData)
    {
        if (saveGameItemPrefab == null || saveGameContainer == null) return;

        GameObject itemObj = Instantiate(saveGameItemPrefab, saveGameContainer);
        SaveGameItem saveItem = itemObj.GetComponent<SaveGameItem>();

        if (saveItem != null)
        {
            saveItem.Initialize(saveData, OnSaveGameSelected, OnSaveGameDeleted);
        }

        saveGameItems.Add(itemObj);
    }

    private void ClearSaveGamesList()
    {
        foreach (GameObject item in saveGameItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        saveGameItems.Clear();
    }

    private void OnSaveGameSelected(PlayerLevelSaveData saveData)
    {
        // Find the save ID for this save data
        List<PlayerLevelSaveData> allSaves = SaveGameManager.Instance.GetAllSaveGames();
        for (int i = 0; i < allSaves.Count; i++)
        {
            if (allSaves[i].saveName == saveData.saveName && allSaves[i].saveDate == saveData.saveDate)
            {
                if (SaveGameManager.Instance.LoadGame(SaveGameManager.Instance.GetAllSaveGames()[i].saveName))
                {
                    StartLoadedGame();
                }
                break;
            }
        }
    }

    private void OnSaveGameDeleted(PlayerLevelSaveData saveData)
    {
        PopulateSaveGamesList(); // Refresh the list
        UpdateContinueButtonState();
    }

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

    private void UpdateContinueButtonState()
    {
        if (continueButton != null)
        {
            continueButton.interactable = SaveGameManager.Instance.HasSavedGames();
        }
    }

    #endregion
}
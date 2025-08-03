using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LoadGamePanel : MonoBehaviour
{
    [Header("Load Game UI")]
    [SerializeField] private Transform saveGameContainer;
    [SerializeField] private GameObject saveGameItemPrefab;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject noSavesMessage;

    private List<GameObject> saveGameItems = new List<GameObject>();
    private Action<PlayerLevelSaveData> onGameSelectedCallback;
    private Action onBackCallback;

    void Awake()
    {
        SetupButtons();
    }

    void OnDestroy()
    {
        CleanupButtons();
    }

    private void SetupButtons()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
    }

    private void CleanupButtons()
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }

    public void Initialize(Action<PlayerLevelSaveData> gameSelectedCallback, Action backCallback)
    {
        onGameSelectedCallback = gameSelectedCallback;
        onBackCallback = backCallback;
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        PopulateSaveGamesList();
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        ClearSaveGamesList();
    }

    private void OnBackButtonClicked()
    {
        onBackCallback?.Invoke();
    }

    #region Save Games List Management

    private void PopulateSaveGamesList()
    {
        ClearSaveGamesList();

        List<PlayerLevelSaveData> saveGames = SaveGameManager.Instance.GetAllSaveGames();

        if (saveGames.Count == 0)
        {
            ShowNoSavesMessage();
            return;
        }

        HideNoSavesMessage();

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
        // Load the selected game
        if (SaveGameManager.Instance.LoadGame(saveData.id))
        {
            onGameSelectedCallback?.Invoke(saveData);
        }
        else
        {
            Debug.LogError($"Failed to load game with ID: {saveData.id}");
        }
    }

    private void OnSaveGameDeleted(PlayerLevelSaveData saveData)
    {
        // Delete the save game
        if (SaveGameManager.Instance.DeleteGame(saveData.id))
        {
            Debug.Log($"Successfully deleted save game: {saveData.id}");
            PopulateSaveGamesList(); // Refresh the list

            // Notify that a save was deleted (for updating other UI elements)
            NotifySaveDeleted();
        }
        else
        {
            Debug.LogError($"Failed to delete save game: {saveData.id}");
        }
    }

    private void ShowNoSavesMessage()
    {
        if (noSavesMessage != null)
        {
            noSavesMessage.SetActive(true);
        }
    }

    private void HideNoSavesMessage()
    {
        if (noSavesMessage != null)
        {
            noSavesMessage.SetActive(false);
        }
    }

    private void NotifySaveDeleted()
    {
        // This could be used to update other UI elements when a save is deleted
        // For example, updating the continue button state in PlayPanel
        if (transform.parent != null)
        {
            PlayPanel playPanel = transform.parent.GetComponentInParent<PlayPanel>();
            if (playPanel != null)
            {
                playPanel.RefreshButtonStates();
            }
        }
    }

    #endregion

    #region Public Interface

    public void RefreshSavesList()
    {
        if (gameObject.activeInHierarchy)
        {
            PopulateSaveGamesList();
        }
    }

    public bool HasSaveGames()
    {
        return SaveGameManager.Instance.HasSavedGames();
    }

    #endregion
}
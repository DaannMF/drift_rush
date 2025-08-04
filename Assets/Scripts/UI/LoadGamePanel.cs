using UnityEngine;
using System.Collections.Generic;
using System;

public class LoadGamePanel : MonoBehaviour {
    [Header("Load Game UI")]
    [SerializeField] private Transform saveGameContainer;
    [SerializeField] private GameObject saveGameItemPrefab;

    private List<GameObject> saveGameItems = new List<GameObject>();
    private Action<PlayerLevelSaveData> onGameSelectedCallback;
    private List<PlayerLevelSaveData> saveGames;

    void OnEnable() {
        SaveEvents.onGetAllSaveGames?.Invoke(OnGetSavedGames);
        PopulateSaveGamesList();
    }

    private void PopulateSaveGamesList() {
        ClearSaveGamesList();

        foreach (PlayerLevelSaveData saveData in saveGames)
            CreateSaveGameItem(saveData);
    }

    private void CreateSaveGameItem(PlayerLevelSaveData saveData) {
        if (saveGameItemPrefab == null || saveGameContainer == null) return;

        GameObject itemObj = Instantiate(saveGameItemPrefab, saveGameContainer);
        SaveGameItem saveItem = itemObj.GetComponent<SaveGameItem>();

        if (saveItem != null)
            saveItem.Initialize(saveData, OnSaveGameSelected, OnSaveGameDeleted);

        saveGameItems.Add(itemObj);
    }

    private void ClearSaveGamesList() {
        foreach (GameObject item in saveGameItems)
            if (item != null) Destroy(item);

        saveGameItems.Clear();
        SaveEvents.onGetAllSaveGames?.Invoke(OnGetSavedGames);
    }

    private void OnSaveGameSelected(PlayerLevelSaveData saveData) {
        // Load the selected game
        SaveEvents.onLoadGame?.Invoke(Guid.Parse(saveData.id));
        onGameSelectedCallback?.Invoke(saveData);
    }

    private void OnSaveGameDeleted(PlayerLevelSaveData saveData) {
        // Delete the save game
        SaveEvents.onDeleteGame?.Invoke(Guid.Parse(saveData.id));
        Debug.Log($"Successfully deleted save game: {saveData.id}");
        PopulateSaveGamesList(); // Refresh the list

        // Notify that a save was deleted (for updating other UI elements)
        NotifySaveDeleted();
    }

    private void NotifySaveDeleted() {
        if (transform.parent != null) {
            PlayPanel playPanel = transform.parent.GetComponentInParent<PlayPanel>();
            if (playPanel != null)
                playPanel.RefreshButtonStates();
        }
    }

    private void OnGetSavedGames(List<PlayerLevelSaveData> savedGames) {
        this.saveGames = savedGames;
    }
}
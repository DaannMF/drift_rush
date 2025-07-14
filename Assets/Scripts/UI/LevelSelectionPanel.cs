using UnityEngine;
using System.Collections.Generic;

public class LevelSelectionPanel : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Transform levelContainer;
    [SerializeField] private GameObject levelItemPrefab;

    private List<LevelData> levels = new List<LevelData>();
    private List<GameObject> levelItemObjects = new List<GameObject>();

    void Awake() {
        LevelEvents.onGetLevelData?.Invoke(OnLevelDataReceived);
    }

    private void OnLevelDataReceived(List<LevelData> levelData) {
        levels = levelData;
        SetupLevelGrid();
    }

    private void SetupLevelGrid() {
        ClearLevelItems();
        for (int i = 0; i < levels.Count; i++)
            CreateLevelItem(levels[i], i);
    }

    private void CreateLevelItem(LevelData levelData, int index) {
        GameObject levelItemObj = Instantiate(levelItemPrefab, levelContainer);
        LevelItem levelItem = levelItemObj.GetComponent<LevelItem>();

        if (levelItem != null)
            levelItem.Initialize(levelData, index, OnPlayButtonClicked);

        levelItemObjects.Add(levelItemObj);
    }

    private void OnPlayButtonClicked(int levelIndex) {
        LevelEvents.onLoadLevel?.Invoke(levelIndex);
        gameObject.SetActive(false);
    }

    private void ClearLevelItems() {
        foreach (var itemObj in levelItemObjects) {
            if (itemObj != null) {
                Destroy(itemObj);
            }
        }
        levelItemObjects.Clear();
    }
}
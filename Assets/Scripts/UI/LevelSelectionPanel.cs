using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectionPanel : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Transform levelContainer;
    [SerializeField] private GameObject levelItemPrefab;
    [SerializeField] private Button backButton;

    private List<LevelData> levels = new List<LevelData>();
    private List<GameObject> levelItemObjects = new List<GameObject>();

    private void Start() {
        SubscribeToEvents();
        SetupBackButton();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        UIEvents.onShowLevelSelectionPanel += ShowPanel;
        UIEvents.onHideLevelSelectionPanel += HidePanel;
    }

    private void UnsubscribeFromEvents() {
        UIEvents.onShowLevelSelectionPanel -= ShowPanel;
        UIEvents.onHideLevelSelectionPanel -= HidePanel;
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

    private void SetupBackButton() {
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked() {
        gameObject.SetActive(false);
        UIEvents.onShowMainMenuPanel?.Invoke();
    }

    private void ClearLevelItems() {
        foreach (var itemObj in levelItemObjects) {
            if (itemObj != null) {
                Destroy(itemObj);
            }
        }
        levelItemObjects.Clear();
    }

    public void ShowPanel() {
        gameObject.SetActive(true);
        LevelEvents.onGetLevelData?.Invoke(OnLevelDataReceived);
    }

    public void HidePanel() {
        gameObject.SetActive(false);
    }
}
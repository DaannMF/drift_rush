using UnityEngine;
using UnityEngine.UI;

public class LoadGamePanel {
    [SerializeField] private Transform saveGameContainer;
    [SerializeField] private GameObject saveGameItemPrefab;

    private void Awake() {
        SetupButtons();
    }

    private void OnDestroy() {
        ClearButtons();
    }

    private void SetupButtons() {

    }

    private void ClearButtons() {

    }
}
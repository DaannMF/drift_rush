using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveGameItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI saveNameText;
    [SerializeField] private TextMeshProUGUI sceneLevelText;
    [SerializeField] private TextMeshProUGUI saveDateText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    private PlayerLevelSaveData saveData;
    private Action<PlayerLevelSaveData> onLoadCallback;
    private Action<PlayerLevelSaveData> onDeleteCallback;

    public void Initialize(PlayerLevelSaveData data, Action<PlayerLevelSaveData> loadCallback, Action<PlayerLevelSaveData> deleteCallback)
    {
        saveData = data;
        onLoadCallback = loadCallback;
        onDeleteCallback = deleteCallback;

        SetupUI();
        SetupButtons();
    }

    private void SetupUI()
    {
        if (saveData == null) return;

        // Set save name
        if (saveNameText != null)
        {
            saveNameText.text = saveData.saveName;
        }

        // Set scene/level info
        if (sceneLevelText != null)
        {
            sceneLevelText.text = GetLevelDisplayName(saveData.sceneName);
        }

        // Set save date
        if (saveDateText != null)
        {
            saveDateText.text = FormatSaveDate(saveData.saveDate);
        }

        // Set coins info
        if (coinsText != null)
        {
            coinsText.text = $"Coins: {saveData.coins}/{saveData.targetCoins}";
        }

        // Set time info
        if (timeText != null)
        {
            timeText.text = FormatTime(saveData.timeRemaining);
        }
    }

    private void SetupButtons()
    {
        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(OnLoadButtonClicked);
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        }
    }

    private void OnLoadButtonClicked()
    {
        onLoadCallback?.Invoke(saveData);
    }

    private void OnDeleteButtonClicked()
    {
        // Show confirmation dialog or directly delete
        // For now, directly delete with confirmation via Debug.Log
        Debug.Log($"Deleting save: {saveData.saveName}");

        // Here you could show a confirmation dialog
        ConfirmDelete();
    }

    private void ConfirmDelete()
    {
        // In a real implementation, you'd show a confirmation dialog
        // For now, we'll delete directly
        onDeleteCallback?.Invoke(saveData);
    }

    #region Utility Methods

    private string GetLevelDisplayName(string sceneName)
    {
        // Convert scene names to user-friendly level names
        switch (sceneName)
        {
            case "Level1":
                return "Level 1 - Shiny Day";
            case "Level2":
                return "Level 2 - Sunset Drive";
            case "Level3":
                return "Level 3 - Night Drive";
            default:
                return sceneName.Replace("Level", "Level ");
        }
    }

    private string FormatSaveDate(string saveDate)
    {
        if (DateTime.TryParse(saveDate, out DateTime parsedDate))
        {
            TimeSpan timeDiff = DateTime.Now - parsedDate;

            if (timeDiff.TotalMinutes < 1)
            {
                return "Just now";
            }
            else if (timeDiff.TotalHours < 1)
            {
                return $"{(int)timeDiff.TotalMinutes}m ago";
            }
            else if (timeDiff.TotalDays < 1)
            {
                return $"{(int)timeDiff.TotalHours}h ago";
            }
            else if (timeDiff.TotalDays < 7)
            {
                return $"{(int)timeDiff.TotalDays}d ago";
            }
            else
            {
                return parsedDate.ToString("MM/dd/yyyy");
            }
        }

        return saveDate;
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    #endregion

    void OnDestroy()
    {
        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
        }
    }
}
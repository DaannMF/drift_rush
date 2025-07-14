using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelItem : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Image levelImage;
    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private TextMeshProUGUI coinsObjective;
    [SerializeField] private TextMeshProUGUI timeObjective;
    [SerializeField] private Button playButton;

    public void Initialize(LevelData levelData, int index, System.Action<int> onPlayCallback) {
        SetLevelData(levelData);

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => onPlayCallback?.Invoke(index));

        if (playButton.GetComponent<UIAudioHandler>() == null)
            playButton.gameObject.AddComponent<UIAudioHandler>();
    }

    private void SetLevelData(LevelData levelData) {
        if (levelData.LevelImage != null && levelImage != null)
            levelImage.sprite = levelData.LevelImage;

        if (levelTitle != null)
            levelTitle.text = levelData.LevelName;

        if (coinsObjective != null)
            coinsObjective.text = $"Coins: {levelData.TargetCoins}";

        if (timeObjective != null)
            timeObjective.text = $"Time: {FormatTime(levelData.TimeLimit)}";
    }

    private string FormatTime(float timeInSeconds) {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
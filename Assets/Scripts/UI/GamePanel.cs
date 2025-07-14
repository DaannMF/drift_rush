using TMPro;
using UnityEngine;

public class GamePanel : MonoBehaviour {
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text speedText;

    void Awake() {
        GameEvents.onCurrentCoinsChanged += OnCoinsTextChange;
        GameEvents.onCurrentTimeChanged += OnCurrentTimeChanges;
        CarEvents.onCarSpeedChanged += OnCarSpeedChanged;
    }

    void Start() {
        timerText.text = "Time: 00:00";
        coinsText.text = "Coins: 0 / 0";
        speedText.text = "0 km/h";
    }

    void OnDestroy() {
        GameEvents.onCurrentCoinsChanged -= OnCoinsTextChange;
        GameEvents.onCurrentTimeChanged -= OnCurrentTimeChanges;
        CarEvents.onCarSpeedChanged -= OnCarSpeedChanged;
    }

    private void OnCoinsTextChange(int currentCoins, int targetCoins) {
        coinsText.text = $"Coins: {currentCoins} / {targetCoins}";
    }

    private void OnCurrentTimeChanges(float currentTime) {
        if (currentTime <= 0) {
            timerText.text = "Time: 00:00";
            return;
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
    }

    private void OnCarSpeedChanged(float speed) {
        float speedKmh = speed * 3.6f;
        speedText.text = $"{speedKmh:F0} km/h";
    }
}

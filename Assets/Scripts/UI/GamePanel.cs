using TMPro;
using UnityEngine;

public class GamePanel : MonoBehaviour {
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinsText;

    void Awake() {
        GameEvents.onCurrentCoinsChanged += OnCoinsTextChange;
        GameEvents.onCurrentTimeChanged += OnCurrentTimeChanges;
    }

    // Start is called before the first frame update
    void Start() {
        // Initialize the timer and coins text
        timerText.text = "Time: 00:00";
        coinsText.text = "Coins: 0 / 0";
    }

    void OnDestroy() {
        GameEvents.onCurrentCoinsChanged -= OnCoinsTextChange;
        GameEvents.onCurrentTimeChanged -= OnCurrentTimeChanges;
    }

    private void OnCoinsTextChange(int currentCoins, int targetCoins) {
        coinsText.text = $"Coins: {currentCoins} / {targetCoins}";
    }

    private void OnCurrentTimeChanges(float currentTime) {
        if (currentTime <= 0) {
            timerText.text = "Time: 00:00";
            return;
        }

        // Format the time to minutes and seconds
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
    }
}

using UnityEngine.Events;

public abstract class GameEvents {
    public static UnityAction<float> onCurrentTimeChanged;
    public static UnityAction<int, int> onCurrentCoinsChanged;
    public static UnityAction onPauseGame;
    public static UnityAction onResumeGame;
    public static UnityAction onAddCoin;
    public static UnityAction<int, float> onInitializeLevel;
    public static UnityAction onGameFinished;

    public static UnityAction<System.Action<bool>> onGetIsGameWon;
    public static UnityAction<System.Action<int>> onGetCurrentCoins;
    public static UnityAction<System.Action<float>> onGetTimeRemaining;

    public static UnityAction<int> onSetCurrentCoins;
    public static UnityAction<float> onSetTimeRemaining;
}
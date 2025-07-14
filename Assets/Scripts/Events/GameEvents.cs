using UnityEngine.Events;

/// <summary>
/// Core game events for gameplay mechanics like coins, time, and game state
/// </summary>
public abstract class GameEvents {
    // Core Game State Events
    public static UnityAction<float> onCurrentTimeChanged;
    public static UnityAction<int, int> onCurrentCoinsChanged;
    public static UnityAction onPauseGame;
    public static UnityAction onResumeGame;
    public static UnityAction onAddCoin;
    public static UnityAction<int, float> onInitializeLevel;
    public static UnityAction onGameFinished;

    public static UnityAction<System.Action<bool>> onGetIsGameWon;
}
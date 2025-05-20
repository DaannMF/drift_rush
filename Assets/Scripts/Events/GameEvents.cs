using UnityEngine.Events;

public abstract class GameEvents {
    public static UnityAction<float> onCurrentTimeChanged;
    public static UnityAction<int, int> onCurrentCoinsChanged;
}